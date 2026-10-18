using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public float triggerDistance = 30f;   // falls when the player is this close
    public float groundY = 0.5f;          // top of the track
    public Material shadowMaterial;

    Rigidbody rb;
    Transform player;
    bool dropped;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;

        // shadow on the track below, as a warning
        GameObject shadow = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Destroy(shadow.GetComponent<Collider>());
        shadow.transform.position = new Vector3(transform.position.x, groundY + 0.01f, transform.position.z);
        shadow.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        shadow.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.z, 1f);
        shadow.GetComponent<Renderer>().sharedMaterial = shadowMaterial;
    }

    void FixedUpdate()
    {
        if (dropped) return;
        if (player.position.z > transform.position.z - triggerDistance)
        {
            dropped = true;
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}