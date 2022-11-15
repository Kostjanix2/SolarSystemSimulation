using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonGravity : MonoBehaviour
{
    [SerializeField]
    private float G = 10f;

    [SerializeField]
    private float GFactor = 6.0f;

    [SerializeField]
    private GameObject[] moonObject;

    void Start()
    {
        foreach (var obj1 in moonObject)
        {
            float velocity = CalculateOrbitVelocity(obj1.GetComponent<Rigidbody>(), this.gameObject.GetComponent<Rigidbody>());
            obj1.transform.LookAt(this.transform);
            Vector3 velocityVector3 = velocity * obj1.transform.right;

            obj1.GetComponent<Rigidbody>().velocity += velocityVector3;
        }

    }

    void FixedUpdate()
    {
        foreach (var obj1 in moonObject)
        {
            float force = CalculateForce(obj1.GetComponent<Rigidbody>(), this.gameObject.GetComponent<Rigidbody>());
            Vector3 directionForce = (this.transform.position - obj1.transform.position).normalized;
            Vector3 forceVector3 = force * directionForce;

            obj1.GetComponent<Rigidbody>().AddForce(forceVector3);
        }

    }

    float CalculateForce(Rigidbody r1, Rigidbody r2)
    {
        // Calculation Gravitational Force of to bodies
        float force = Mathf.Pow(10, GFactor) * G * ((r1.mass * r2.mass) / Mathf.Pow(Vector3.Distance(r1.position, r2.position), 2));

        return force;
    }

    float CalculateOrbitVelocity(Rigidbody r1, Rigidbody r2)
    {
        // Calculation Gravitational Force of to bodies
        float velocity = Mathf.Sqrt((Mathf.Pow(10, GFactor) * G * r2.mass) / Vector3.Distance(r1.position, r2.position));

        return velocity;
    }
}
