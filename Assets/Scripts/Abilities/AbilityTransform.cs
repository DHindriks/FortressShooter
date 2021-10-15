using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTransform : AbilityBase
{
    [SerializeField]
    GameObject TransformInto;

    void Update()
    {
        if (!AbilityLocked && Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject Transformed = Instantiate(TransformInto);
            Transformed.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

}
