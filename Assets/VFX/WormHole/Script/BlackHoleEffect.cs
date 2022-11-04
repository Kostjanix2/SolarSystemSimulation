using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BlackHoleEffect : MonoBehaviour
{
    public Shader shader;
    public Transform blackHole;
    public float ratio;
    public float radius;

    Camera cam;
    Material _material;

    Material matieral
    {
        get 
        { 
            if(_material == null)
            {
                _material = new Material(shader);
                _material.hideFlags = HideFlags.HideAndDontSave;
            }
            return _material;
        }
    }
    private void OnEnable()
    {
        cam = GetComponent<Camera>();
        ratio = 1f / cam.aspect;
    }

    public virtual void OnDisable()
    {
        if(_material)
        {
            DestroyImmediate(_material);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        
    }
}
