using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Vector3 direction;
    private void Update()
    {
        this.transform.Rotate(direction * rotationSpeed * Time.deltaTime);
    }
}
