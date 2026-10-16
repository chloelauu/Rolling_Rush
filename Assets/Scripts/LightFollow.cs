using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollow : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target != null)
        {
            // Match the light's position to the ball's position
            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
        }
    }
}