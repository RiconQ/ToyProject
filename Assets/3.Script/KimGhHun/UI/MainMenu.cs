using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private GameObject settingFramePanel;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button quitButton;

    [Header("BestScore")]
    [SerializeField] private GameObject bestScorePanel;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private Button onBestScore;
    [SerializeField] private Button offBestScore;


    private void Start()
    {
        settingFramePanel.SetActive(false);
        bestScorePanel.SetActive(false);

        UpdateBestScore();
        SetEvent();
    }

    private void SetEvent()
    {
        startGameButton.onClick.AddListener(GameStart);
        settingButton.onClick.AddListener(ToggleSetting);
        quitButton.onClick.AddListener(QuitGame);

        onBestScore.onClick.AddListener(ToggleBestScore);
        offBestScore.onClick.AddListener(ToggleBestScore);
    }

    private void UpdateBestScore()
    {
        bestScoreText.text = DataManager.instance.scoreData.score.ToString();
    }

    private void GameStart()
    {
        // 게임 시작 버튼 
        // 캐릭터 선택화면으로
    }

    public void ToggleSetting()
    {
        // 게임 설정 버튼
        // 사운드 설정
        settingFramePanel.SetActive(!settingFramePanel.activeSelf);
    }

    public void QuitGame()
    {
        //게임 종료 버튼

        //사운드 설정 저장
        DataManager.instance.SaveSound();

        Application.Quit();
    }

    public void ToggleBestScore()
    {
        bestScorePanel.SetActive(!bestScorePanel.activeSelf);
    }
}
