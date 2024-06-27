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
        // ���� ���� ��ư 
        // ĳ���� ����ȭ������
    }

    public void ToggleSetting()
    {
        // ���� ���� ��ư
        // ���� ����
        settingFramePanel.SetActive(!settingFramePanel.activeSelf);
    }

    public void QuitGame()
    {
        //���� ���� ��ư

        //���� ���� ����
        DataManager.instance.SaveSound();

        Application.Quit();
    }

    public void ToggleBestScore()
    {
        bestScorePanel.SetActive(!bestScorePanel.activeSelf);
    }
}
