using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float forwardSpeed = 5.0f;   // 캐릭터의 앞으로 이동 속도
    public float lateralSpeed = 5.0f;   // 캐릭터의 좌우 이동 속도

    public float jumpHeight = 10f; // 점프 높이 조절을 위한 변수
    public float groundHeight = 0.5f; // 땅의 높이 (플레이어가 서 있는 지점)

    public Animator anim; // Animator 컴포넌트 참조를 위한 변수
    private Rigidbody rb; // Rigidbody 컴포넌트 참조를 위한 변수        
    private bool isJumping = false; // 점프 중인지 여부를 나타내는 변수
    private bool isSlide = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 할당
        anim = GetComponent<Animator>(); // Animator 컴포넌트 할당
    }

    private void Update()
    {
        PlayerInput();
    }

    public void PlayerInput()
    {
        // 캐릭터가 계속 앞으로 이동
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        UpdateMovePlayer();


        
        if (Input.GetKeyDown(KeyCode.Space)/* && !isJumping && IsGrounded()*/)
        {
            isJumping = true;
            anim.SetTrigger("Jump"); // 점프 애니메이션을 재생
        
            // 플레이어의 y축 높이를 jumpHeight 만큼 증가
            Vector3 jumpVector = new Vector3(0f, jumpHeight, 0f);
             rb.AddForce(jumpVector, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isSlide = true;

            anim.SetTrigger("Slide"); // 슬라이드 애니메이션을 재생
            // 작업 내용 추가중
            // 슬라이드 판정 설정 필요
        }
    }

    public void UpdateMovePlayer()
    {
        // 좌우 이동
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * lateralSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        // 플레이어가 땅에 닿았는지 여부를 체크
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundHeight))
        {
            isJumping = false; // 점프 중이 아님을 표시
            return true;
        }
        return false;
    }
}
