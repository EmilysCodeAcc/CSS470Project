using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SnowFollower : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
 