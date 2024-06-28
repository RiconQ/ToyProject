using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraController : MonoBehaviour
{
    [SerializeField]private Transform player;
    [SerializeField] private float zOffset = -7f;
    [SerializeField] private float yOffset = 4f;

    private void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y + yOffset, player.position.z + zOffset);
    }
}
