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

    [SerializeField]
    bool StartSlingShot = false;

    bool IsShot = false;

    float Currentcooldown = 1;

    TrajectoryPreview preview;

    float currentlayer;

    float Layer;


    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        if (StartSlingShot)
        {
            GameManager.Instance.MainSlingshot = this;
        }

        preview = GetComponent<TrajectoryPreview>();
        preview.enabled = false;
    }

    void OnMouseDrag()
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

                if (!preview.isActiveAndEnabled)
                {
                    preview.enabled = true;
                }
                else
                {
                    Vector3 vel = ((transform.position - ballRB.transform.position) * 2.5f) * ballRB.mass;
                    preview.velocity.x = vel.x;
                    preview.velocity.y = vel.y;
                }

            }

            
        }
    }

    void OnMouseUp()
    {
        //shoots the object
        if (ballRB != null && Input.GetMouseButtonUp(0) && Vector3.Distance(ballRB.position, Origin.transform.position) > 0.3f)
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

            if (preview.isActiveAndEnabled)
            {
                preview.enabled = false;
            }
        }
    }

    public void SpawnProjectile(GameObject Prefab)
    {
        if (ballRB == null)
        {
            GameObject NewProjectile = Instantiate(Prefab);
            if (NewProjectile.GetComponent<Rigidbody>())
            {
                SetProjectile(NewProjectile.GetComponent<Rigidbody>());
            }else
            {
                Debug.LogError("Couldn't find RigidBody component on spawned projectile.");
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
        if (other.gameObject.GetComponent<Rigidbody>() && ballRB == null && Currentcooldown < Time.time && !StartSlingShot)
        {
            SetProjectile(other.gameObject.GetComponent<Rigidbody>());
        }    
    }


}