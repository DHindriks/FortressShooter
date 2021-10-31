using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityExplode : AbilityBase
{
    [SerializeField]
    int ExplosionRange;

    [SerializeField]
    GameObject ParticlePrefab;

    void Update()
    {
        if (!AbilityLocked && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Collider[] objects = Physics.OverlapSphere(transform.position, ExplosionRange);

            foreach (Collider c in objects)
            {
                Rigidbody r = c.GetComponent<Rigidbody>();
                if (r != null)
                {
                    r.AddExplosionForce(40000, transform.position, ExplosionRange);
                }

            }

            GameObject Particle = Instantiate(ParticlePrefab);
            Particle.transform.position = transform.position;

            Destroy(gameObject);
        }
    }

}
