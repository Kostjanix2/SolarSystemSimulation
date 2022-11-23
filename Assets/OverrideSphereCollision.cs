using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class OverrideSphereCollision : MonoBehaviour
{
    public Transform sphereCollider;
    VisualEffect vfx;
    string VFXPositionPostfix = "_position";
    string VFXRotationPostfix = "_angles";
    string VFXScalePostfix = "_scale";

    void Start()
    {
        vfx = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        SetVFXTransformProperty(vfx, "SphereCollision", sphereCollider);
    }
    public void SetVFXTransformProperty(VisualEffect visualEffect, string propertyName, Transform transform)
    {
        string position = propertyName + VFXPositionPostfix;
        string angles = propertyName + VFXRotationPostfix;
        string scale = propertyName + VFXScalePostfix;

        visualEffect.SetVector3(position, transform.position);
        visualEffect.SetVector3(angles, transform.eulerAngles);
        visualEffect.SetVector3(scale, transform.localScale);
    }
}
