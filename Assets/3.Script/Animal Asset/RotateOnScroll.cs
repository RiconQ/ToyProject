/* Scripted by Omabu - omabuarts@gmail.com */
using UnityEngine;
using System.Collections.Generic;

public class RotateOnScroll : MonoBehaviour
{
    public float rotationSpeed = 2000f;

    void Update()
    {

            float rotationAmount = rotationSpeed * Time.deltaTime * 10;
            transform.Rotate(Vector3.up, rotationAmount);
        
    }
}
