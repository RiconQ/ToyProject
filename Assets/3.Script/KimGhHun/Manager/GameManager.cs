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

    public void SelectCharacter(int index)
    {
        Debug.Log($"선택된 캐릭터 : {index}");
        // 캐릭터 데이터 할당
        SceneManager.LoadScene(playScene);
        //StartGameTimer();
        SoundManager.instance.PlayMusic(1);
    }

    public void StartGameTimer()
    {
        //player 참조
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void AddScore()
    {
        
        if (player.isLive)
        {
            score += 1;
            //점수 업데이트
        }
    }

    public void OnDead()
    {
        // 게임 오버 화면 출력
        // 현재 점수 비교
        //      점수 갱신시 저장
    }
}
