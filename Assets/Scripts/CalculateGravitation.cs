using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateGravitation : MonoBehaviour
{
    [SerializeField]
    private float G = 0.01f;
    private GameObject[] spaceObjects;

    // Start is called before the first frame update
    void Start()
    {
        spaceObjects = GameObject.FindGameObjectsWithTag("SpaceObjects");

        foreach (var obj1 in spaceObjects)
        {
            Vector3 velocityVector3 = Vector3.zero;

            foreach (var obj2 in spaceObjects)
            {
                if (obj1 == obj2)
                    continue;

                float velocity = CalculateOrbitVelocity(obj1.GetComponent<Rigidbody>(), obj2.GetComponent<Rigidbody>());

                obj1.transform.LookAt(obj2.transform);
                velocityVector3 += velocity * obj1.transform.right;
            }

            obj1.GetComponent<Rigidbody>().velocity = velocityVector3;
        }

    }

    void FixedUpdate()
    {
        foreach (var obj1 in spaceObjects)
        {
            Vector3 forceVector3 = Vector3.zero;

            foreach (var obj2 in spaceObjects)
            {
                if (obj1 == obj2)
                    continue;

                float force = CalculateForce(obj1.GetComponent<Rigidbody>(), obj2.GetComponent<Rigidbody>());

                Vector3 directionForce = (obj2.transform.position - obj1.transform.position).normalized;
                
                forceVector3 += force * directionForce;
            }

            obj1.GetComponent<Rigidbody>().AddForce(forceVector3);
        }

    }


    float CalculateForce(Rigidbody r1, Rigidbody r2)
    {
        // Calculation Gravitational Force of to bodies
        float force = G * ((r1.mass * r2.mass) / Mathf.Pow(Vector3.Distance(r1.position, r2.position), 2));
        
        return force;
    }

    float CalculateOrbitVelocity(Rigidbody r1, Rigidbody r2)
    {
        // Calculation Gravitational Force of to bodies
        float velocity = Mathf.Sqrt((G *  r2.mass) / Vector3.Distance(r1.position, r2.position));

        return velocity;
    }
}
