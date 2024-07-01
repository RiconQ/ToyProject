using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ���� ���� ���
// �� �Ѿ�°� ����
// ���� ����, ���� ����
// ĳ���� ����(��ų��)


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
        Debug.Log($"���õ� ĳ���� : {index}");
        classIndex = index;
        // ĳ���� ������ �Ҵ�
        

        SceneManager.LoadScene(playScene);
        StartGameTimer();
        SoundManager.instance.PlayMusic(1);
    }
    public void SetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // �÷��̾� ��ų ���� -> Index�� ����
        player.SelectPlayerType = (Player.PlayerType)classIndex;
    }
    public void StartGameTimer()
    {
        //player ����
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
            //���� ������Ʈ
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
        // ���� ���� ȭ�� ���
        // ���� ���� ��
        //      ���� ���Ž� ����
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
