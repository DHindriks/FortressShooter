using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileData : MonoBehaviour
{
    public Sprite Icon;

    public Color ProjectileColor;

    Rigidbody rb;
    bool HasReset = false;
    private void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            rb = GetComponent<Rigidbody>();
        }else
        {
            Debug.LogWarning("Could not find rigidbody on this object: " + gameObject.name);
        }

    }

    void OnCollisionStay(Collision collision)
    {
        if (rb && rb.velocity.magnitude < 5 && !HasReset)
        {
            GameManager.Instance.cameraScript.Invoke("ResetCam", 1);
            HasReset = true;
        }
    }
}
