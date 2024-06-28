using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI currentScoreText;


    private void Start()
    {
        GameManager.instance.scoreCanvas = this;
        bestScoreText.text = DataManager.instance.scoreData.score.ToString();
    }

    public void UpdateCurrentScore(int score)
    {
        currentScoreText.text = score.ToString();
    }
}
