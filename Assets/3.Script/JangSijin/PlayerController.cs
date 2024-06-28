using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    public float forwardSpeed = 5.0f;   // ĳ������ ������ �̵� �ӵ�
    public float lateralSpeed = 5.0f;   // ĳ������ �¿� �̵� �ӵ�

    public void UpdateMovePlayer(Transform playerTransform)
    {
        // ĳ���Ͱ� ��� ������ �̵�
        playerTransform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // �¿� �̵�
        float horizontalInput = Input.GetAxis("Horizontal");
        playerTransform.Translate(Vector3.right * horizontalInput * lateralSpeed * Time.deltaTime);
    }
}