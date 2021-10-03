using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Health))]
public class BlockBehaviour : MonoBehaviour
{
    Health health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        health.OnHealthChanged.AddListener(CheckHealth);
    }

    void CheckHealth()
    {
        if (health.health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        health.Damage(collision.relativeVelocity.magnitude);
    }
}
