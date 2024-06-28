using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI; // UI ����� ���� �߰�

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
        if (Input.GetButtonDown("Jump") && player.IsGrounded() && player.CanJump())
        {
            player.ChangeState(new JumpState(player));
        }
        // �����̵� �Է��� ������ �����̵� ���·� ��ȯ
        else if (Input.GetButtonDown("Slide") && player.IsGrounded() && player.CanSlide())
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
        player.StartJumpCooldown(); // ���� ��Ÿ�� ����
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
    private float slideDuration = 1.6f; // �����̵� ���� �ð�
    private float slideTimer;

    public SlideState(Player player) : base(player) { }

    public override void Enter()
    {
        if (player.IsGrounded())
        {
            // �����̵� ���� �� �ݶ��̴��� ���̸� ���̰� �߽��� ����
            player.capsuleCollider.height = player.SlideColliderHeight;
            player.capsuleCollider.center = player.SlideColliderCenter;

            player.anim.SetTrigger("Slide");
            player.rb.velocity = Vector3.zero; // �̵� ����
            slideTimer = slideDuration;
            player.StartSlideCooldown(); // �����̵� ��Ÿ�� ����                        
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
            // �����̵� ���� �� �ݶ��̴��� ���̿� �߽��� �⺻ ���� ������ �ʱ�ȭ
            player.capsuleCollider.height = player.OriginColliderHeight;
            player.capsuleCollider.center = player.OriginColliderCenter;

            player.ChangeState(new RunState(player));
        }
    }

    public override void Exit()
    {
        // ���¸� ��� �� �ʿ��� �۾�
    }
}

// Dead ���� Ŭ����
public class DeadState : PlayerState
{
    public DeadState(Player player) : base(player) { }

    public override void Enter()
    {
        player.isLive = false;
        player.anim.SetTrigger("Dead");
        // ���⿡ �״� �ִϸ��̼� ���� �ڵ带 �߰��� �� �ֽ��ϴ�.        
    }

    public override void Update()
    {
        // ���°� ������Ʈ�Ǵ� ���� �߰� �۾��� �ʿ��� ��� ó��
    }

    public override void Exit()
    {
        // ���¸� ��� �� �ʿ��� �۾�
    }
}


public class Player : MonoBehaviour
{
    public Transform groundCheck; // �� üũ�� ���� ��ġ
    [HideInInspector] public Vector3 OriginColliderCenter = new Vector3(0f, 1f, 0f);
    [HideInInspector] public float OriginColliderHeight = 2f;
    [HideInInspector] public Vector3 SlideColliderCenter = new Vector3(0f, 0.5f, 0f);
    [HideInInspector] public float SlideColliderHeight = 0.5f;

    public Image jumpCooldownImage; // ���� ��Ÿ�� UI �̹���
    public Image slideCooldownImage; // �����̵� ��Ÿ�� UI �̹���

    public TextMeshProUGUI jumpCooldownTextMesh; // ���� ��Ÿ�� UI �ؽ�Ʈ
    public TextMeshProUGUI slideCooldownTextMesh; // �����̵� ��Ÿ�� UI �ؽ�Ʈ

    public float forwardSpeed = 5.0f;   // ĳ������ ������ �̵� �ӵ�
    public float lateralSpeed = 5.0f;   // ĳ������ �¿� �̵� �ӵ�
    public float jumpHeight = 10f; // ���� ���� ������ ���� ����
    public float groundCheckRadius = 0.1f; // �� üũ�� ���� ���� ������    

    public float jumpCooldownTime = 2.0f; // ���� ��Ÿ�� �ð�
    public float slideCooldownTime = 2.0f; // �����̵� ��Ÿ�� �ð�

    [HideInInspector] public bool isLive = true; // �÷��̾ ����ִ��� Ȯ���� ���� ����
    [HideInInspector] public bool isJumping = false; // ���� ������ ����    
    [HideInInspector] public Animator anim; // Animator ������Ʈ ������ ���� ����
    [HideInInspector] public Rigidbody rb; // Rigidbody ������Ʈ ������ ���� ����
    [HideInInspector] public CapsuleCollider capsuleCollider; // Rigidbody ������Ʈ ������ ���� ����

    private PlayerState currentState; // �÷��̾� �ൿ ����
    private bool canJump = true; // ���� ���� ����
    private bool canSlide = true; // �����̵� ���� ����

    private float jumpCooldownEndTime;
    private float slideCooldownEndTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ �Ҵ�
        anim = GetComponent<Animator>(); // Animator ������Ʈ �Ҵ�
        capsuleCollider = GetComponent<CapsuleCollider>(); // CapsuleCollider ������Ʈ �Ҵ�

        ChangeState(new RunState(this)); // �ʱ� ���¸� Run���� ����
    }

    void Update()
    {
        DefaultMove();

        currentState.Update(); // ���� ������ Update �޼��� ȣ��

        // ��Ÿ�� UI ������Ʈ
        UpdateCooldownUI();
    }

    private void DefaultMove()
    {
        if (isLive)
        {
            // ĳ���ʹ� ��� ������ �̵��Ѵ�.
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
        }

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
        if (!isLive)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * lateralSpeed * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        // �÷��̾ ���� ��Ҵ��� ���θ� üũ
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));
    }

    void OnCollisionEnter(Collision other)
    {
        // Water Layer�� �浹���� �� ó��
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            ChangeState(new DeadState(this)); // �״� ���·� ��ȯ
        }
    }

    // ���� ��Ÿ�� ����
    public void StartJumpCooldown()
    {
        canJump = false;
        jumpCooldownEndTime = Time.time + jumpCooldownTime;
        Invoke(nameof(ResetJumpCooldown), jumpCooldownTime);
    }

    // �����̵� ��Ÿ�� ����
    public void StartSlideCooldown()
    {
        canSlide = false;
        slideCooldownEndTime = Time.time + slideCooldownTime;
        Invoke(nameof(ResetSlideCooldown), slideCooldownTime);
    }

    // ���� ��Ÿ�� ����
    private void ResetJumpCooldown()
    {
        canJump = true;
    }

    // �����̵� ��Ÿ�� ����
    private void ResetSlideCooldown()
    {
        canSlide = true;
    }

    // ���� ���� ���� Ȯ��
    public bool CanJump()
    {
        return canJump;
    }

    // �����̵� ���� ���� Ȯ��
    public bool CanSlide()
    {
        return canSlide;
    }

    // ��Ÿ�� UI ������Ʈ
    private void UpdateCooldownUI()
    {
        if (!canJump)
        {
            float remainingJumpTime = jumpCooldownEndTime - Time.time;
            jumpCooldownImage.fillAmount = remainingJumpTime / jumpCooldownTime;            
            jumpCooldownTextMesh.text = remainingJumpTime.ToString("N1");
        }
        else
        {
            jumpCooldownImage.fillAmount = 0;
            jumpCooldownTextMesh.text = "";
        }

        if (!canSlide)
        {
            float remainingSlideTime = slideCooldownEndTime - Time.time;
            slideCooldownImage.fillAmount = remainingSlideTime / slideCooldownTime;
            slideCooldownTextMesh.text = remainingSlideTime.ToString("N1");
        }
        else
        {
            slideCooldownImage.fillAmount = 0;
            slideCooldownTextMesh.text = "";
        }
    }
}
