using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);


            SetupSound();
            SetupScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private string savePath;
    public SoundData soundData;
    public ScoreData scoreData;

    private void SetupSound()
    {
        savePath = Application.persistentDataPath + "/saves/";
        soundData = new SoundData(9, 9, 9);
        soundData = LoadSound();
    }

    private void SetupScore()
    {
        scoreData = new ScoreData(0);
        scoreData = LoadScore();
    }
    public void SaveSound()
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        string soundSaveString = JsonUtility.ToJson(soundData);
        File.WriteAllText(savePath + "soundData.json", soundSaveString);
    }
    public SoundData LoadSound()
    {
        SoundData loadSoundData = new SoundData(9, 9, 9);
        try
        {
            string soundLoadString = File.ReadAllText(savePath + "/soundData.json");
            loadSoundData = JsonUtility.FromJson<SoundData>(soundLoadString);
        }
        catch
        {
            Debug.Log("Catch sound load");
            SaveSound();
        }
        return loadSoundData;
    }

    public void SaveScore()
    {
        string scoreSaveString = JsonUtility.ToJson(scoreData);
        File.WriteAllText(savePath + "scoreData.json", scoreSaveString);
    }

    public ScoreData LoadScore()
    {
        ScoreData loadScoreData = new ScoreData(0);
        try
        {
            string scoreLoadString = File.ReadAllText(savePath + "/scoreData.json");
            loadScoreData = JsonUtility.FromJson<ScoreData>(scoreLoadString);
        }
        catch
        {
            Debug.Log("Catch score load");
            SaveScore();
        }
        return loadScoreData;
    }
}

[System.Serializable]
public class SoundData
{
    public SoundData(float master, float bgm, float sfx)
    {
        this.master = master;
        this.bgm = bgm;
        this.sfx = sfx;
    }
    public float master;
    public float bgm;
    public float sfx;
}

[System.Serializable]
public class ScoreData
{
    public ScoreData(int score)
    {
        this.score = score;
    }
    public int score;
}