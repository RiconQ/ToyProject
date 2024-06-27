using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform InitTransform { get; private set; }
        
    void Start()
    {
        InitTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
