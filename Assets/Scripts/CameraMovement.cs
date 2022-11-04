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
    Transform flyTo;
    [SerializeField]
    float flySpeed = 20;
    [SerializeField]
    Vector3 inOffset;

    Vector3 realOffset;

    void Update()
    {
        if (flyTo == null)
        {
            if (sitOn != null)
            {
                realOffset = inOffset + new Vector3(0,sitOn.localScale.y,0);
                this.transform.position = sitOn.position + realOffset;
            }
            if(target != null)
            {
                this.transform.LookAt(target);
            }
        }
        else
        {
            realOffset = inOffset + new Vector3(0, flyTo.localScale.y, 0);
            Vector3 myPos = this.transform.position;
            Vector3 flyToDir = (flyTo.position  + realOffset - myPos);
            float distance = flyToDir.magnitude;
            if(Mathf.Round(distance) > 10)
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
    }
}
