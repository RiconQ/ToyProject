using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverCanvas : MonoBehaviour
{
    //[SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        //this.gameObject.SetActive(false);
        restartButton.onClick.AddListener(GameManager.instance.RestartGame);
        mainMenuButton.onClick.AddListener(GameManager.instance.ToMainMenu);
    }

    //private void RestartGame()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //    GameManager.instance.RestartGame();
    //}
    //private void ToMainMenu()
    //{
    //    SceneManager.LoadScene("UI Test");
    //}
}
