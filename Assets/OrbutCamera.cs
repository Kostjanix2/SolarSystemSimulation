using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbutCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 2.0f;
    public float rollSpeed = 40f;
    public float rotSpeed = 20.0f;
    public float moveSpeed = 10f;
    public float distanceMin = 20f;
    public float distanceMax = 30f;
    public float smoothTime = 20f;
    float radius;

    private void Start()
    {
        float radius = distanceMin;
    }

    void OnMouseDrag(float r)
    {
        this.transform.position =
           target.position + (this.transform.position - target.position).normalized * r;

        float rotX = Input.GetAxis("Mouse X") * rotSpeed;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed;
        if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(transform.position, transform.forward, -rollSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(transform.position, transform.forward, rollSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 right = Vector3.Cross(transform.up, target.position - transform.position);
            Vector3 up = Vector3.Cross(target.position - transform.position, right);
            transform.RotateAround(target.position, up, rotX * rotSpeed * Time.deltaTime);
            transform.RotateAround(target.position, right, rotY * rotSpeed * Time.deltaTime);
        }
    }

    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0){
            radius += moveSpeed * Time.deltaTime;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            radius -= moveSpeed * Time.deltaTime;
        }
        radius = Mathf.Clamp(radius, distanceMin, distanceMax);
        OnMouseDrag(radius);
    }
}
