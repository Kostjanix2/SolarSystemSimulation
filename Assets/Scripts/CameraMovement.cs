using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    Transform sitOn;
    [SerializeField]
    bool orbitAroundTarget;
    [SerializeField]
    float maxZoom = 10;
    [SerializeField]
    float smoothZoom = 10;
    [SerializeField]
    float rotSpeed = 20.0f;
    [SerializeField]
    float moveSpeed = 10f;
    [SerializeField]
    float distanceMin = 20f;
    [SerializeField]
    float distanceMax = 30f;
    [SerializeField]
    float radius = 20f;
    [SerializeField]
    Transform flyTo;
    [SerializeField]
    float flySpeed = 20;
    [SerializeField]
    Vector3 inOffset;

    float basicFOV = 60;
    Camera cam;
    Vector3 realOffset;
    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    void Update()
    {
        float currFOV = cam.fieldOfView;
        handleZoom();
        if (flyTo == null)
        {
            if(orbitAroundTarget || sitOn == target)
            {
                handleOrbitAround();
            }
            else
            {
                handleSitOnLookAt();
            }
        }
        else
        {
            handleFlyTo();
        }
    }

    void handleOrbitAround()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            radius += moveSpeed * Time.deltaTime;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            radius -= moveSpeed * Time.deltaTime;
        }
        radius = Mathf.Clamp(radius, distanceMin, distanceMax);
        OnMouseDrag();
    }

    void handleZoom()
    {
        float currFOV = cam.fieldOfView;
        if (Input.GetKey(KeyCode.Mouse1))
        {
            cam.fieldOfView = Mathf.Lerp(currFOV, maxZoom, smoothZoom * Time.deltaTime);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(currFOV, basicFOV, smoothZoom * Time.deltaTime);
        }
    }
    void OnMouseDrag()
    {
        Vector3 targetPos = target.position;
        this.transform.position = target.position + (this.transform.position - target.position).normalized * radius;


        float rotX = Input.GetAxis("Mouse X") * rotSpeed;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed;
        
        transform.LookAt(targetPos);
        Vector3 right = Vector3.Cross(transform.up, targetPos - transform.position);
        Vector3 up = Vector3.Cross(targetPos - transform.position, right);
        transform.RotateAround(targetPos, up, rotX * rotSpeed * Time.deltaTime);
        transform.RotateAround(targetPos, right, rotY * rotSpeed * Time.deltaTime);
    }
    void handleFlyTo()
    {
        realOffset = inOffset + new Vector3(0, flyTo.localScale.y, 0);
        Vector3 myPos = this.transform.position;
        Vector3 flyToDir = (flyTo.position + realOffset - myPos);
        float distance = flyToDir.magnitude;
        if (Mathf.Round(distance) > 10)
        {
            this.transform.LookAt(flyTo.position + realOffset);
            this.transform.position = myPos + flyToDir.normalized * flySpeed * Time.deltaTime;
        }
        else
        {
            sitOn = flyTo;
            flyTo = null;
        }
    }

    void handleSitOnLookAt()
    {
        if (sitOn != null)
        {
            realOffset = inOffset + new Vector3(0, sitOn.localScale.y, 0);
            this.transform.position = sitOn.position + realOffset;
        }
        if (target != null)
        {
            this.transform.LookAt(target);
        }
    }
}
