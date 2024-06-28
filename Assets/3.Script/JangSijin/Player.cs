// �÷��̾� ���¸� �����ϴ� �߻� Ŭ����
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

// Run ���� Ŭ����
public class RunState : PlayerState
{
    public RunState(Player player) : base(player) { }

    public override void Enter()
    {
        player.anim.SetTrigger("Run");
    }

    public override void Update()
    {
        // �¿� �̵� ó��
        player.UpdatePlayerInputHorizontalMove();

        // ���� �Է��� ������ ���� ���·� ��ȯ
        if (Input.GetButtonDown("Jump") && player.IsGrounded())
        {
            player.ChangeState(new JumpState(player));
        }
        // �����̵� �Է��� ������ �����̵� ���·� ��ȯ
        else if (Input.GetButtonDown("Slide") && player.IsGrounded())
        {
            player.ChangeState(new SlideState(player));
        }
    }

    public override void Exit()
    {
        // ���¸� ��� �� �ʿ��� �۾�
    }
}

// Jump ���� Ŭ����
public class JumpState : PlayerState
{
    public JumpState(Player player) : base(player) { }

    public override void Enter()
    {
        player.anim.SetTrigger("Jump");
        Vector3 jumpVector = new Vector3(0f, player.jumpHeight, 0f);
        player.rb.AddForce(jumpVector, ForceMode.Impulse);
        player.isJumping = true; // ���� ������ ǥ��
    }

    public override void Update()
    {
        // ���� ����� ��� 
        if (player.IsGrounded())
        {
            player.isJumping = false; // ���� ����
            player.ChangeState(new RunState(player));
        }
        else
        {
            // ���� �߿��� �¿� �̵� ó��
            player.UpdatePlayerInputHorizontalMove();
        }
    }

    public override void Exit()
    {
        // ���¸� ��� �� �ʿ��� �۾�
    }
}

// Slide ���� Ŭ����
public class SlideState : PlayerState
{
    private float slideDuration = 1.0f; // �����̵� ���� �ð�
    private float slideTimer;

    public SlideState(Player player) : base(player) { }

    public override void Enter()
    {
        if (player.IsGrounded())
        {
            player.anim.SetTrigger("Slide");
            player.rb.velocity = Vector3.zero; // �̵� ����
            slideTimer = slideDuration;
        }
        else
        {
            // ���� ���� ���� ������ Run ���·� ��ȯ
            player.ChangeState(new RunState(player));
        }
    }

    public override void Update()
    {
        player.UpdatePlayerInputHorizontalMove(); // �¿� �̵��� �����ϵ��� ����

        slideTimer -= Time.deltaTime;

        if (slideTimer <= 0)
        {
            player.ChangeState(new RunState(player));
        }
    }

    public override void Exit()
    {
        // ���¸� ��� �� �ʿ��� �۾�
    }
}

public class Player : MonoBehaviour
{
    public Transform groundCheck; // �� üũ�� ���� ��ġ

    public float forwardSpeed = 5.0f;   // ĳ������ ������ �̵� �ӵ�
    public float lateralSpeed = 5.0f;   // ĳ������ �¿� �̵� �ӵ�
    public float jumpHeight = 10f; // ���� ���� ������ ���� ����
    public float groundCheckRadius = 0.1f; // �� üũ�� ���� ���� ������    

    [HideInInspector] public bool isJumping = false; // ���� ������ ����
    [HideInInspector] public Animator anim; // Animator ������Ʈ ������ ���� ����
    [HideInInspector] public Rigidbody rb; // Rigidbody ������Ʈ ������ ���� ����
    private PlayerState currentState; // �÷��̾� �ൿ ����

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ �Ҵ�
        anim = GetComponent<Animator>(); // Animator ������Ʈ �Ҵ�
        ChangeState(new RunState(this)); // �ʱ� ���¸� Run���� ����
    }

    void Update()
    {
        DefaultMove();

        currentState.Update(); // ���� ������ Update �޼��� ȣ��
    }

    private void DefaultMove()
    {
        // ĳ���ʹ� ��� ������ �̵��Ѵ�.
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // �¿� �̵� ó��
        if (currentState.GetType() != typeof(JumpState) && currentState.GetType() != typeof(SlideState))
        {
            UpdatePlayerInputHorizontalMove();
        }
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != null)
        {
            currentState.Exit(); // ���� ���� ����
        }

        currentState = newState;
        currentState.Enter(); // ���ο� ���� ����
    }

    public void UpdatePlayerInputHorizontalMove()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * lateralSpeed * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        // �÷��̾ ���� ��Ҵ��� ���θ� üũ
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));
    }
}
