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

    public void SelectCharacter(int index)
    {
        Debug.Log($"���õ� ĳ���� : {index}");
        // ĳ���� ������ �Ҵ�
        SceneManager.LoadScene(playScene);
        //StartGameTimer();
        SoundManager.instance.PlayMusic(1);
    }

    public void StartGameTimer()
    {
        //player ����
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void AddScore()
    {
        
        if (player.isLive)
        {
            score += 1;
            //���� ������Ʈ
        }
    }

    public void OnDead()
    {
        // ���� ���� ȭ�� ���
        // ���� ���� ��
        //      ���� ���Ž� ����
    }
}
