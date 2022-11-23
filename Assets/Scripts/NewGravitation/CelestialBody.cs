using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class CelestialBody : MonoBehaviour
{
    [SerializeField]
    public float mass = 1f;

    [SerializeField]
    float radius = 1f;

    [SerializeField]
    public Vector3 initialVelocity;

    Vector3 currentVelocity;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        currentVelocity = initialVelocity;
    }

    public void UpdateVelocity(CelestialBody[] celestialBodies, float timeStep)
    {
        foreach (var otherBody in celestialBodies)
        {
            if (this != otherBody)
            {
                float sqrDst = (otherBody.rb.position - this.rb.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.rb.position - this.rb.position).normalized;

                Vector3 acceleration = forceDir * Universe.gravitationalConstant * otherBody.mass / sqrDst;
                currentVelocity += acceleration * timeStep;
            }
        }
    }

    public void UpdateVelocity(Vector3 acceleration, float timeStep)
    {
        currentVelocity += acceleration * timeStep;
    }

    public void UpdatePosition(float timeStep)
    {
        if(rb != null)
            rb.MovePosition(rb.position + currentVelocity * timeStep);
    }

    public Rigidbody Rigidbody
    {
        get
        {
            return rb;
        }
    }

    public Vector3 Position
    {
        get
        {
            if (rb != null)
                return rb.position;
            else
                return Vector3.zero;
        }
    }

}
