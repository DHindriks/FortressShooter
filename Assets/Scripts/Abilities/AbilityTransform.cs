using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTransform : AbilityBase
{
    [SerializeField]
    GameObject TransformInto;

    void OnMouseDown()
    {
        if (!AbilityLocked)
        {
            GameObject Transformed = Instantiate(TransformInto);
            Transformed.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

}
