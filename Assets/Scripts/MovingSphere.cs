using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    //max speed
    [SerializeField, Range(0f, 100f)]
    public float maxSpeed = 10f;

    //max acceleration
    [SerializeField, Range(0f, 100f)]
    public float maxAcceleration = 10f, maxAirAcceleration = 1f;

    //jumpHeight
    [SerializeField, Range(0f, 200f)]
    float jumpHeight = 2f;

    //maxAirJumps   
    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;

    //maxGroundAngle
    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 25f;

    public Transform playerInputSpace = default;

    Vector3 velocity, desiredVelocity, contactNormal;
    Rigidbody body;
    //bool onGround;
    int jumpPhase;
    int groundContactCount;
    int stepsSinceLastGrounded;
    float minGroundDotProduct;
    bool desiredJump;

    bool OnGround => groundContactCount > 0;

    void Awake()
    {
        body = GetComponent<Rigidbody>(); //Access own RB
        OnValidate(); //Calculate MaxGroundAngle
    }

    // Update is called once per frame
    void Update()
    {
        //GetPlayerInput (WASD & Jump)
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        desiredJump |= Input.GetButtonDown("Jump");

        //Get Plane from CameraInputSpace
        if (playerInputSpace)
        {
            Vector3 forward = playerInputSpace.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 right = playerInputSpace.right;
            right.y = 0f;
            right.Normalize();
            desiredVelocity = (forward * playerInput.y + right * playerInput.x) * maxSpeed; //Combine CameraInputSpace with PlayerInput
        }
        this.transform.forward = Vector3.Lerp(this.transform.forward, Vector3.ProjectOnPlane(playerInputSpace.forward,Vector3.up), 10 * Time.deltaTime);
    }


    void FixedUpdate()
    {
        UpdateState();
        AdjustVelocity();

        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }

        body.velocity = velocity;
        ClearState();
    }

    void UpdateState()
    {
        stepsSinceLastGrounded += 1;
        velocity = body.velocity;
        if (OnGround || SnapToGround())
        {
            stepsSinceLastGrounded = 0;
            jumpPhase = 0;
            if (groundContactCount > 1)
            {
                contactNormal.Normalize();
            }
        }
        else
        {
            contactNormal = Vector3.up;
        }
    }

    void ClearState()
    {
        groundContactCount = 0;
        contactNormal = Vector3.zero;
    }

    void Jump()
    {
        if (OnGround || jumpPhase < maxAirJumps)
        {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            float alignedSpeed = Vector3.Dot(velocity, contactNormal);

            if (alignedSpeed > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
            }

            if(velocity.y < 0)
                velocity.y = 0f;
            velocity += Vector3.up * jumpSpeed;
        }
    }

    bool SnapToGround()
    {
        if (stepsSinceLastGrounded > 1)
        {
            return false;
        }
        if (!Physics.Raycast(body.position, Vector3.down, out RaycastHit hit))
        {
            return false;
        }
        if (hit.normal.y < minGroundDotProduct)
        {
            return false;
        }
        return false;
    }

    void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad); //CalculateFloat From MaxGroundAngle
    }

    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            if (normal.y >= minGroundDotProduct)
            {
                groundContactCount += 1;
                contactNormal += normal;
            }
        }
    }

    Vector3 ProjectOnContactPlane(Vector3 vector)
    {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }

    void AdjustVelocity()
    {
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        float currentX = Vector3.Dot(velocity, xAxis);
        float currentZ = Vector3.Dot(velocity, zAxis);

        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }
}
