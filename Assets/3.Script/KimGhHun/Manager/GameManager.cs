using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임 점수 계산
// 씬 넘어가는거 관리
// 게임 오버, 시작 관리
// 캐릭터 정보(스킬등)


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    [SerializeField] private string playScene;
    [SerializeField] private int score = 0;
    [SerializeField] private Player player;
    [SerializeField] private ScoreCanvas scoreCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject MainMenuCanvasObject;
    [SerializeField] private GameObject helpWindow;
    [SerializeField] private GameObject selectWindow;

    public List<BoxCollider> obstacleColliders;

    public int classIndex = 0;

    private void Start()
    {
        obstacleColliders = new List<BoxCollider>();
        scoreCanvas.gameObject.SetActive(false);
        gameOverCanvas.SetActive(false);
    }
    public void SelectCharacter(int index)
    {
        Debug.Log($"선택된 캐릭터 : {index}");
        classIndex = index;
        // 캐릭터 데이터 할당
        

        SceneManager.LoadScene(playScene);
        StartGameTimer();
        SoundManager.instance.PlayMusic(1);
    }
    public void SetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // 플레이어 스킬 설정 -> Index에 따라서
        player.SelectPlayerType = (Player.PlayerType)classIndex;
    }
    public void StartGameTimer()
    {
        //player 참조
        score = 0;
        MainMenuCanvasObject.SetActive(false);
        scoreCanvas.gameObject.SetActive(true);
        scoreCanvas.ShowBestScore();
    }

    public void AddScore()
    {
        if (player.isLive)
        {
            score += 1;
            scoreCanvas.UpdateCurrentScore(score);
            //점수 업데이트
        }
    }

    private void Update()
    {

        //Debug.Log(gameOverCanvas.activeSelf);

        try
        {
            if (!player.isLive)
            {
                if (!gameOverCanvas.activeSelf)
                {
                    gameOverCanvas.SetActive(true);
                    OnDead();
                }
            }
            else
            {
                if (gameOverCanvas.activeSelf)
                    gameOverCanvas.SetActive(false);
            }
        }
        catch
        {

        }
    }

    public void OnDead()
    {
        // 게임 오버 화면 출력
        // 현재 점수 비교
        //      점수 갱신시 저장
        //player.transform.GetComponent<CapsuleCollider>().isTrigger = true;
        SoundManager.instance.PlaySFX(1);
        int bestScore = DataManager.instance.scoreData.score;
        if (bestScore < score)
        {

            DataManager.instance.scoreData.score = score;
            DataManager.instance.SaveScore();
        }
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame");
        obstacleColliders = new List<BoxCollider>();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        scoreCanvas.ShowBestScore();
        score = 0;
        //scoreCanvas.gameObject.SetActive(false);
        //Debug.Log(gameOverCanvas.gameObject.activeSelf);
    }

    public void ToMainMenu()
    {
        obstacleColliders = new List<BoxCollider>();
        SceneManager.LoadScene("UI Test");
        scoreCanvas.gameObject.SetActive(false);
        MainMenuCanvasObject.SetActive(true);
        helpWindow.SetActive(false);
        selectWindow.SetActive(false);
        SoundManager.instance.PlayMusic(0);
    }

    public void SetObstacleCollider(bool value)
    {
        foreach(var item in obstacleColliders)
        {
            item.enabled = value;
        }
    }

    public void AddCollider(BoxCollider col)
    {
        obstacleColliders.Add(col);
    }
}
