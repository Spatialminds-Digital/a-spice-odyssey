using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-99)]
public class HighScoreService : MonoBehaviour
{
    private const string PlayerPrefsKey = "HighScores";
    private const int MaxScores = 5;

    public event Action<SavedHighScores> OnHighScoresLoaded;
    public event Action<SavedHighScores> OnHighScoresSaved;

    private SavedHighScores cachedHighScores;

    public static HighScoreService Instance;

    void Awake()
    {

        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
        //DontDestroyOnLoad(this.gameObject);

    }

    void Start()
    {
        LoadHighScores();
    }

    public SavedHighScores LoadHighScores()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            cachedHighScores = new SavedHighScores { scores = new List<SavedScore>() };
            OnHighScoresLoaded?.Invoke(cachedHighScores);
            return cachedHighScores;
        }

        string json = PlayerPrefs.GetString(PlayerPrefsKey);
        cachedHighScores = JsonUtility.FromJson<SavedHighScores>(json);

        if (cachedHighScores == null || cachedHighScores.scores == null)
        {
            cachedHighScores = new SavedHighScores { scores = new List<SavedScore>() };
        }
        else
        {
            cachedHighScores.scores = cachedHighScores.scores
                .OrderByDescending(s => s.score)
                .ToList();
        }

        OnHighScoresLoaded?.Invoke(cachedHighScores);
        return cachedHighScores;
    }

    public bool CheckIfScoreIsHigh(int score)
    {
        if (cachedHighScores == null)
        {
            LoadHighScores();
        }

        if (cachedHighScores.scores.Count < MaxScores)
        {
            return true;
        }

        int lowestScore = cachedHighScores.scores[cachedHighScores.scores.Count - 1].score;
        return score > lowestScore;
    }

    public bool CheckIfScoreIsHighest(int score)
    {
        if (cachedHighScores == null)
        {
            LoadHighScores();
        }

        if (cachedHighScores.scores.Count == 0)
        {
            return true;
        }

        return score > cachedHighScores.scores[0].score;
    }

    public void AddHighScore(string name, int score)
    {
        if (cachedHighScores == null)
        {
            LoadHighScores();
        }

        var newScore = new SavedScore { name = name, score = score };
        cachedHighScores.scores.Add(newScore);

        cachedHighScores.scores = cachedHighScores.scores
            .OrderByDescending(s => s.score)
            .Take(MaxScores)
            .ToList();

        SaveHighScores();
    }

    private void SaveHighScores()
    {
        string json = JsonUtility.ToJson(cachedHighScores);
        PlayerPrefs.SetString(PlayerPrefsKey, json);
        PlayerPrefs.Save();

        OnHighScoresSaved?.Invoke(cachedHighScores);
    }
}
