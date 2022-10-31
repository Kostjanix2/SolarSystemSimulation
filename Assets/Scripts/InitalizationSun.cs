using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitalizationSun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
