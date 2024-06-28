// 플레이어 상태를 정의하는 추상 클래스
using UnityEngine;

public abstract class PlayerState
{
    protected Player player;

    public PlayerState(Player player)
    {
        this.player = player;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}

// Run 상태 클래스
public class RunState : PlayerState
{
    public RunState(Player player) : base(player) { }

    public override void Enter()
    {
        player.anim.SetTrigger("Run");
    }

    public override void Update()
    {
        // 좌우 이동 처리
        player.UpdatePlayerInputHorizontalMove();

        // 점프 입력을 받으면 점프 상태로 전환
        if (Input.GetButtonDown("Jump") && player.IsGrounded())
        {
            player.ChangeState(new JumpState(player));
        }
        // 슬라이드 입력을 받으면 슬라이드 상태로 전환
        else if (Input.GetButtonDown("Slide") && player.IsGrounded())
        {
            player.ChangeState(new SlideState(player));
        }
    }

    public override void Exit()
    {
        // 상태를 벗어날 때 필요한 작업
    }
}

// Jump 상태 클래스
public class JumpState : PlayerState
{
    public JumpState(Player player) : base(player) { }

    public override void Enter()
    {
        player.anim.SetTrigger("Jump");
        Vector3 jumpVector = new Vector3(0f, player.jumpHeight, 0f);
        player.rb.AddForce(jumpVector, ForceMode.Impulse);
        player.isJumping = true; // 점프 중임을 표시
    }

    public override void Update()
    {
        // 땅에 닿았을 경우 
        if (player.IsGrounded())
        {
            player.isJumping = false; // 점프 종료
            player.ChangeState(new RunState(player));
        }
        else
        {
            // 점프 중에도 좌우 이동 처리
            player.UpdatePlayerInputHorizontalMove();
        }
    }

    public override void Exit()
    {
        // 상태를 벗어날 때 필요한 작업
    }
}

// Slide 상태 클래스
public class SlideState : PlayerState
{
    private float slideDuration = 1.0f; // 슬라이드 지속 시간
    private float slideTimer;

    public SlideState(Player player) : base(player) { }

    public override void Enter()
    {
        if (player.IsGrounded())
        {
            player.anim.SetTrigger("Slide");
            player.rb.velocity = Vector3.zero; // 이동 중지
            slideTimer = slideDuration;
        }
        else
        {
            // 만약 땅에 있지 않으면 Run 상태로 전환
            player.ChangeState(new RunState(player));
        }
    }

    public override void Update()
    {
        player.UpdatePlayerInputHorizontalMove(); // 좌우 이동은 가능하도록 설정

        slideTimer -= Time.deltaTime;

        if (slideTimer <= 0)
        {
            player.ChangeState(new RunState(player));
        }
    }

    public override void Exit()
    {
        // 상태를 벗어날 때 필요한 작업
    }
}

public class Player : MonoBehaviour
{
    public Transform groundCheck; // 땅 체크를 위한 위치

    public float forwardSpeed = 5.0f;   // 캐릭터의 앞으로 이동 속도
    public float lateralSpeed = 5.0f;   // 캐릭터의 좌우 이동 속도
    public float jumpHeight = 10f; // 점프 높이 조절을 위한 변수
    public float groundCheckRadius = 0.1f; // 땅 체크를 위한 구의 반지름    

    [HideInInspector] public bool isJumping = false; // 점프 중인지 여부
    [HideInInspector] public Animator anim; // Animator 컴포넌트 참조를 위한 변수
    [HideInInspector] public Rigidbody rb; // Rigidbody 컴포넌트 참조를 위한 변수
    private PlayerState currentState; // 플레이어 행동 상태

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 할당
        anim = GetComponent<Animator>(); // Animator 컴포넌트 할당
        ChangeState(new RunState(this)); // 초기 상태를 Run으로 설정
    }

    void Update()
    {
        DefaultMove();

        currentState.Update(); // 현재 상태의 Update 메서드 호출
    }

    private void DefaultMove()
    {
        // 캐릭터는 계속 앞으로 이동한다.
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // 좌우 이동 처리
        if (currentState.GetType() != typeof(JumpState) && currentState.GetType() != typeof(SlideState))
        {
            UpdatePlayerInputHorizontalMove();
        }
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != null)
        {
            currentState.Exit(); // 현재 상태 종료
        }

        currentState = newState;
        currentState.Enter(); // 새로운 상태 진입
    }

    public void UpdatePlayerInputHorizontalMove()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * lateralSpeed * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        // 플레이어가 땅에 닿았는지 여부를 체크
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));
    }
}
