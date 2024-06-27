using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float forwardSpeed = 5.0f;   // ĳ������ ������ �̵� �ӵ�
    public float lateralSpeed = 5.0f;   // ĳ������ �¿� �̵� �ӵ�

    public float jumpHeight = 10f; // ���� ���� ������ ���� ����
    public float groundHeight = 0.5f; // ���� ���� (�÷��̾ �� �ִ� ����)

    public Animator anim; // Animator ������Ʈ ������ ���� ����
    private Rigidbody rb; // Rigidbody ������Ʈ ������ ���� ����        
    private bool isJumping = false; // ���� ������ ���θ� ��Ÿ���� ����
    private bool isSlide = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ �Ҵ�
        anim = GetComponent<Animator>(); // Animator ������Ʈ �Ҵ�
    }

    private void Update()
    {
        PlayerInput();
    }

    public void PlayerInput()
    {
        // ĳ���Ͱ� ��� ������ �̵�
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        UpdateMovePlayer();


        
        if (Input.GetKeyDown(KeyCode.Space)/* && !isJumping && IsGrounded()*/)
        {
            isJumping = true;
            anim.SetTrigger("Jump"); // ���� �ִϸ��̼��� ���
        
            // �÷��̾��� y�� ���̸� jumpHeight ��ŭ ����
            Vector3 jumpVector = new Vector3(0f, jumpHeight, 0f);
             rb.AddForce(jumpVector, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isSlide = true;

            anim.SetTrigger("Slide"); // �����̵� �ִϸ��̼��� ���
            // �۾� ���� �߰���
            // �����̵� ���� ���� �ʿ�
        }
    }

    public void UpdateMovePlayer()
    {
        // �¿� �̵�
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * lateralSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        // �÷��̾ ���� ��Ҵ��� ���θ� üũ
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundHeight))
        {
            isJumping = false; // ���� ���� �ƴ��� ǥ��
            return true;
        }
        return false;
    }
}
