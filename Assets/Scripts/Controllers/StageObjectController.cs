using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObjectController : MonoBehaviour
{
    Vector3 currentPos;
    float len;

    [SerializeField] private float friction = 0.05f;

    void FixedUpdate()
    {
        len = (currentPos - transform.position).magnitude;
        currentPos = transform.position;
    }

    public bool IsMoved()
    {
        if (len > friction)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
