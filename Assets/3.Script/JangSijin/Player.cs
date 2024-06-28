using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI; // UI 사용을 위해 추가

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

    // 스킬 사용 입력 처리    
    protected void HandleSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.D) && player.CanUseSkill())
        {
            player.ActivateSkill();
        }
    }
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
        // 좌우 이동 처리
        player.UpdatePlayerInputHorizontalMove();

        // 점프 입력을 받으면 점프 상태로 전환
        if (Input.GetButtonDown("Jump") && player.IsGrounded() && player.CanJump())
        {
            player.ChangeState(new JumpState(player));
        }
        // 슬라이드 입력을 받으면 슬라이드 상태로 전환
        else if (Input.GetButtonDown("Slide") && player.IsGrounded() && player.CanSlide())
        {
            player.ChangeState(new SlideState(player));
        }

        // 스킬 사용 입력 처리
        HandleSkillInput();
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
        SoundManager.instance.PlaySFX(2);
        player.anim.SetTrigger("Jump");
        Vector3 jumpVector = new Vector3(0f, player.jumpHeight, 0f);
        player.rb.AddForce(jumpVector, ForceMode.Impulse);
        player.isJumping = true; // 점프 중임을 표시
        player.StartJumpCooldown(); // 점프 쿨타임 시작
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

        // 스킬 사용 입력 처리
        HandleSkillInput();
    }

    public override void Exit()
    {
        // 상태를 벗어날 때 필요한 작업
    }
}

// Slide 상태 클래스
public class SlideState : PlayerState
{
    private float slideDuration = 1.6f; // 슬라이드 지속 시간
    private float slideTimer;

    public SlideState(Player player) : base(player) { }

    public override void Enter()
    {
        if (player.IsGrounded())
        {
            // 슬라이딩 시작 시 콜라이더의 높이를 줄이고 중심을 변경
            player.capsuleCollider.height = player.SlideColliderHeight;
            player.capsuleCollider.center = player.SlideColliderCenter;

            player.anim.SetTrigger("Slide");
            player.rb.velocity = Vector3.zero; // 이동 중지
            slideTimer = slideDuration;
            player.StartSlideCooldown(); // 슬라이드 쿨타임 시작                        
        }
        else
        {
            // 만약 땅에 있지 않으면 Run 상태로 전환
            player.ChangeState(new RunState(player));
        }

        // 스킬 사용 입력 처리
        HandleSkillInput();
    }

    public override void Update()
    {
        player.UpdatePlayerInputHorizontalMove(); // 좌우 이동은 가능하도록 설정

        slideTimer -= Time.deltaTime;

        if (slideTimer <= 0)
        {
            // 슬라이딩 정료 시 콜라이더의 높이와 중심을 기본 설정 값으로 초기화
            player.capsuleCollider.height = player.OriginColliderHeight;
            player.capsuleCollider.center = player.OriginColliderCenter;

            player.ChangeState(new RunState(player));
        }

        // 스킬 사용 입력 처리
        HandleSkillInput();
    }

    public override void Exit()
    {
        // 상태를 벗어날 때 필요한 작업
    }
}

// Dead 상태 클래스
public class DeadState : PlayerState
{
    public DeadState(Player player) : base(player) { }

    public override void Enter()
    {
        player.isLive = false;
        player.anim.SetTrigger("Dead");
        // 여기에 죽는 애니메이션 실행 코드를 추가할 수 있습니다.        
    }

    public override void Update()
    {
        // 상태가 업데이트되는 동안 추가 작업이 필요한 경우 처리
    }

    public override void Exit()
    {
        // 상태를 벗어날 때 필요한 작업
    }
}

public class Player : MonoBehaviour
{
    public Transform groundCheck; // 땅 체크를 위한 위치                                                     
    ParticleSystem[] particleSystems; // 현재 게임 오브젝트의 모든 파티클 시스템 비활성화

    [HideInInspector] public Vector3 OriginColliderCenter = new Vector3(0f, 1f, 0f);
    [HideInInspector] public float OriginColliderHeight = 2f;
    [HideInInspector] public Vector3 SlideColliderCenter = new Vector3(0f, 0.5f, 0f);
    [HideInInspector] public float SlideColliderHeight = 0.5f;

    public Image jumpCooldownImage; // 점프 쿨타임 UI 이미지
    public Image slideCooldownImage; // 슬라이드 쿨타임 UI 이미지
    public Image skillCooldownBGImage; // 스킬 쿨타임 백그라운드 UI 이미지
    public Image skillCooldownImage; // 스킬 쿨타임 UI 이미지

    public TextMeshProUGUI jumpCooldownTextMesh; // 점프 쿨타임 UI 텍스트
    public TextMeshProUGUI slideCooldownTextMesh; // 슬라이드 쿨타임 UI 텍스트
    public TextMeshProUGUI skillCooldownTextMesh; // 스킬 쿨타임 UI 텍스트

    public float forwardSpeed = 20.0f;   // 캐릭터의 앞으로 이동 속도
    public float lateralSpeed = 5.0f;   // 캐릭터의 좌우 이동 속도
    public float jumpHeight = 10f; // 점프 높이 조절을 위한 변수
    public float groundCheckRadius = 0.1f; // 땅 체크를 위한 구의 반지름    

    public float jumpCooldownTime = 2.0f; // 점프 쿨타임 시간
    public float slideCooldownTime = 2.0f; // 슬라이드 쿨타임 시간
    public float skillCooldownTime = 10.0f; // 스킬 쿨타임 시간

    public float skillDurationTime = 5.0f; // 스킬 지속 시간

    [HideInInspector] public bool isLive = true; // 플레이어가 살아있는지 확인을 위한 변수
    [HideInInspector] public bool isJumping = false; // 점프 중인지 여부    
    [HideInInspector] public Animator anim; // Animator 컴포넌트 참조를 위한 변수
    [HideInInspector] public Rigidbody rb; // Rigidbody 컴포넌트 참조를 위한 변수
    [HideInInspector] public CapsuleCollider capsuleCollider; // Rigidbody 컴포넌트 참조를 위한 변수

    private PlayerState currentState; // 플레이어 행동 상태
    private bool canJump = true; // 점프 가능 여부
    private bool canSlide = true; // 슬라이드 가능 여부
    private bool canUseSkill = true; // 스킬 사용 가능 여부

    private float jumpCooldownEndTime;
    private float slideCooldownEndTime;
    private float skillCooldownEndTime;

    [HideInInspector] public float SkillValue = 2.0f;

    public enum PlayerType
    {
        FastMan,
        ShildMan,
        ZeroGravityMan,
        MaxType
    }

    public PlayerType SelectPlayerType = PlayerType.FastMan;

    // 직업별 스킬 이미지
    public Sprite[] SkillSprites = new Sprite[(int)PlayerType.MaxType];

    // 직업별 스킬 파티클
    public GameObject[] SkillParticles = new GameObject[(int)PlayerType.MaxType];

    [SerializeField] public RoadSpawner spawner;



    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 할당
        anim = GetComponent<Animator>(); // Animator 컴포넌트 할당
        capsuleCollider = GetComponent<CapsuleCollider>(); // CapsuleCollider 컴포넌트 할당               

        // 직업별 스킬 파티클 초기 세팅
        particleSystems = SkillParticles[(int)SelectPlayerType].gameObject.GetComponentsInChildren<ParticleSystem>(); // 스킬 파티클들을 할당

        // SkillParticles 배열에 있는 모든 파티클 시스템을 중지 // Linq
        SkillParticles
            .Where(sp => sp != null) // null 체크
            .SelectMany(sp => sp.GetComponentsInChildren<ParticleSystem>())
            .ToList()
            .ForEach(ps => ps.Stop());

        // 직업별 스킬 UI 이미지 세팅
        skillCooldownBGImage.sprite = SkillSprites[(int)SelectPlayerType];
        skillCooldownImage.sprite = SkillSprites[(int)SelectPlayerType];

        ChangeState(new RunState(this)); // 초기 상태를 Run으로 설정
    }

    void Update()
    {
        DefaultMove();

        // 현재 상태의 Update 메서드 호출
        currentState.Update();

        // 스킬 사용을 위한 입력을 각 상태에서 받을 수 있도록 함
        HandleSkillInput();

        // 쿨타임 UI 업데이트
        UpdateCooldownUI();
    }

    private void DefaultMove()
    {
        if (isLive)
        {
            // 캐릭터는 계속 앞으로 이동한다.
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
        }

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
        if (!isLive)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * lateralSpeed * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        // 플레이어가 땅에 닿았는지 여부를 체크
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, LayerMask.GetMask("Ground"));
    }
    private void OnTriggerEnter(Collider other)
    {
        // 맵(도로) 풀링
        if (spawner != null)
        {
            if (other.gameObject.CompareTag("MapSpawnTrigger"))
            {
                Debug.Log("맵 풀링");
                spawner.SpawnTriggerEntered();
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        

        // Water Layer에 충돌했을 때 처리
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            ChangeState(new DeadState(this)); // 죽는 상태로 전환
        }
    }

    // 점프 쿨타임 시작
    public void StartJumpCooldown()
    {
        canJump = false;
        jumpCooldownEndTime = Time.time + jumpCooldownTime;
        Invoke(nameof(ResetJumpCooldown), jumpCooldownTime);
    }

    // 슬라이드 쿨타임 시작
    public void StartSlideCooldown()
    {
        canSlide = false;
        slideCooldownEndTime = Time.time + slideCooldownTime;
        Invoke(nameof(ResetSlideCooldown), slideCooldownTime);
    }

    // 스킬 쿨타임 시작
    public void StartSkillCooldown()
    {
        canUseSkill = false;
        skillCooldownEndTime = Time.time + skillCooldownTime;
        Invoke(nameof(ResetSkillDuration), skillDurationTime); // 스킬 지속 시간 리셋
        Invoke(nameof(ResetSkillCooldown), skillCooldownTime); // 스킬 쿨타임 리셋
    }

    // 점프 쿨타임 리셋
    private void ResetJumpCooldown()
    {
        canJump = true;
    }

    // 슬라이드 쿨타임 리셋
    private void ResetSlideCooldown()
    {
        canSlide = true;
    }

    // 스킬 쿨타임 리셋
    private void ResetSkillCooldown()
    {
        canUseSkill = true;
    }

    // 스킬 지속시간 리셋
    private void ResetSkillDuration()
    {
        particleSystems.ToList().ForEach(ps => ps.Stop());

        switch (SelectPlayerType)
        {
            case PlayerType.FastMan:
                // 이동 속도 증가 초기화
                anim.SetFloat("SkillSpeed", 1.0f);
                forwardSpeed /= SkillValue;
                lateralSpeed /= SkillValue;
                break;
            case PlayerType.ShildMan:
                // 무적 상태 초기화
                // 게임 매니저에서 장애물 모든 오브젝트 콜라이더 활성화      
                          
                GameManager.instance.SetObstacleCollider(true);
                break;
            case PlayerType.ZeroGravityMan:
                // 무중력 상태 초기화
                rb.mass = 1f;
                anim.SetFloat("SkillGravityValue", 1f);
                break;
        }
    }

    // 점프 가능 여부 확인
    public bool CanJump()
    {
        return canJump;
    }

    // 슬라이드 가능 여부 확인
    public bool CanSlide()
    {
        return canSlide;
    }

    // 스킬 사용 가능 여부 확인
    public bool CanUseSkill()
    {
        return canUseSkill;
    }

    // 쿨타임 UI 업데이트
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

        if (!canUseSkill)
        {
            float remainingSlideTime = skillCooldownEndTime - Time.time;
            skillCooldownImage.fillAmount = remainingSlideTime / skillCooldownTime;
            skillCooldownTextMesh.text = remainingSlideTime.ToString("N1");
        }
        else
        {
            skillCooldownImage.fillAmount = 0;
            skillCooldownTextMesh.text = "";
        }
    }

    // 스킬 사용 입력 처리
    private void HandleSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.D) && CanUseSkill())
        {
            ActivateSkill();
        }
    }

    // 스킬 사용
    public void ActivateSkill()
    {
        if (CanUseSkill())
        {
            SoundManager.instance.PlaySFX(3);
            // 스킬 이펙트 활성화           
            particleSystems.ToList().ForEach(ps => ps.Play());

            switch (SelectPlayerType)
            {
                case PlayerType.FastMan:
                    // 이동 속도 증가
                    anim.SetFloat("SkillSpeed", SkillValue);
                    forwardSpeed *= SkillValue;
                    lateralSpeed *= SkillValue;
                    break;
                case PlayerType.ShildMan:
                    // 무적 상태
                    // 게임 매니저에서 장애물 모든 오브젝트 콜라이더 비활성화
                    GameManager.instance.SetObstacleCollider(false);
                    break;
                case PlayerType.ZeroGravityMan:
                    // 무중력 상태
                    rb.mass = 0.26f;
                    anim.SetFloat("SkillGravityValue", 0.25f);
                    break;
            }

            // 예시로 스킬 쿨타임 시작 메서드 호출
            StartSkillCooldown();
        }
    }
}