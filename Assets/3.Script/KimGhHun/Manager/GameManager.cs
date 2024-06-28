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

    private bool isGameStart = false;

    [SerializeField] private string playScene;
    [SerializeField] private int score = 0;

    public void SelectCharacter(int index)
    {
        Debug.Log($"���õ� ĳ���� : {index}");
        // ĳ���� ������ �Ҵ�
        SceneManager.LoadScene(playScene);
        StartGameTimer();
        SoundManager.instance.PlayMusic(1);
    }

    public void StartGameTimer()
    {
        isGameStart = true;
    }

    public void AddScore()
    {
        if (isGameStart)
            score += 1;
    }
}
