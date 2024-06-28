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

    [Header("CharacterSelect")]
    [SerializeField] private GameObject characterSelectPanel;
    //[SerializeField] private GameObject characterModel;
    [SerializeField] private Button closeCharacterSelect;
    

    

    private void Start()
    {
        settingFramePanel.SetActive(false);
        bestScorePanel.SetActive(false);
        characterSelectPanel.SetActive(false);
        //characterModel.SetActive(false);

        UpdateBestScore();
        SetEvent();
    }

    private void SetEvent()
    {
        startGameButton.onClick.AddListener(ToggleCharacterSelect);
        settingButton.onClick.AddListener(ToggleSetting);
        quitButton.onClick.AddListener(QuitGame);

        onBestScore.onClick.AddListener(ToggleBestScore);
        offBestScore.onClick.AddListener(ToggleBestScore);
        closeCharacterSelect.onClick.AddListener(ToggleCharacterSelect);
    }

    private void UpdateBestScore()
    {
        DataManager.instance.LoadScore();
        bestScoreText.text = DataManager.instance.scoreData.score.ToString();
    }

    private void ToggleCharacterSelect()
    {
        SoundManager.instance.PlaySFX(0);
        characterSelectPanel.SetActive(!characterSelectPanel.activeSelf);
        //characterModel.SetActive(characterSelectPanel.activeSelf);
    }

    public void ToggleSetting()
    {
        // 게임 설정 버튼
        // 사운드 설정
        SoundManager.instance.PlaySFX(0);
        settingFramePanel.SetActive(!settingFramePanel.activeSelf);
    }

    public void QuitGame()
    {
        //게임 종료 버튼

        //사운드 설정 저장
        SoundManager.instance.PlaySFX(0);
        DataManager.instance.SaveSound();

        Application.Quit();
    }

    public void ToggleBestScore()
    {
        UpdateBestScore();
        SoundManager.instance.PlaySFX(0);
        bestScorePanel.SetActive(!bestScorePanel.activeSelf);
    }
}
