using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShotScript : MonoBehaviour
{
    [SerializeField]
    Rigidbody ballRB;

    [SerializeField]
    public GameObject Origin;

    [SerializeField]
    public Camera cam;


    bool IsShot = false;

    float Currentcooldown = 1;

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if (ballRB != null)
        {
            //if ball hasn't been shot yet, move the ball to player's mouse location, max 2 units from the slingshot origin.
            if (Input.GetMouseButton(0) && !IsShot)
            {
                //calculate mouse position on the screen
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Vector3.Distance(cam.transform.position, ballRB.position) - cam.nearClipPlane;
                mousePos = cam.ScreenToWorldPoint(mousePos);
                if (Vector3.Distance(mousePos, Origin.transform.position) > 2)
                {
                    ballRB.position = Origin.transform.position + (mousePos - Origin.transform.position).normalized * 2;
                }
                else
                {
                    ballRB.position = mousePos;
                }
            }

            //shoots the object
            if (Input.GetMouseButtonUp(0) && Vector3.Distance(ballRB.position, Origin.transform.position) > 0.3f)
            {
                IsShot = true;
                ballRB.isKinematic = false;
                ballRB.AddRelativeTorque(new Vector3(Random.Range(0, 0), Random.Range(0, 0), Random.Range(0, 10)), ForceMode.VelocityChange);
                ballRB.AddForce(((transform.position - ballRB.transform.position) * 1000) * ballRB.mass);
                if (ballRB.gameObject.GetComponent<AbilityBase>())
                {
                    ballRB.GetComponent<AbilityBase>().AbilityLocked = false;
                }
                ballRB = null;
                Currentcooldown = Time.time + 1.5f;
            }
        }
    }

    void SetProjectile (Rigidbody rb)
    {
        rb.isKinematic = true;
        rb.gameObject.transform.position = transform.position;
        ballRB = rb;
        IsShot = false;
        if (ballRB.gameObject.GetComponent<AbilityBase>())
        {
            ballRB.GetComponent<AbilityBase>().AbilityLocked = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() && ballRB == null && Currentcooldown < Time.time)
        {
            SetProjectile(other.gameObject.GetComponent<Rigidbody>());
        }    
    }


}