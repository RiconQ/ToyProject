using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float movespeed;
    [SerializeField] private Vector3 moveDirection = Vector3.zero;

    void Update()
    {
        transform.position += moveDirection * movespeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}
