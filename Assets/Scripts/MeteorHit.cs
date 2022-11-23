using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorHit : MonoBehaviour
{
    [SerializeField]
    GameObject impactPrefab;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        if(impactPrefab != null)
        {
            GameObject impact = Instantiate(impactPrefab, pos,rot);
            Destroy(impactPrefab, 10);
        }
        Destroy(gameObject);
    }
}
