using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignHole : MonoBehaviour
{

    [SerializeField]
    Transform camera;

    void Update()
    {
        if (camera != null)
        {
            this.transform.up = (camera.position - this.transform.position).normalized;
        }
    }
}
