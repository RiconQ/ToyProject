using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    public float forwardSpeed = 5.0f;   // 캐릭터의 앞으로 이동 속도
    public float lateralSpeed = 5.0f;   // 캐릭터의 좌우 이동 속도

    public void UpdateMovePlayer(Transform playerTransform)
    {
        // 캐릭터가 계속 앞으로 이동
        playerTransform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // 좌우 이동
        float horizontalInput = Input.GetAxis("Horizontal");
        playerTransform.Translate(Vector3.right * horizontalInput * lateralSpeed * Time.deltaTime);
    }
}