using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFlight : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    float speed;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 attractionForce = (target.position - this.transform.position).normalized * speed * Time.deltaTime;
        rb.velocity = (attractionForce);
    }
}
