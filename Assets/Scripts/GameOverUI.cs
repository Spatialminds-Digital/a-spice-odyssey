using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverContainer;
    [SerializeField] private GameObject lblHighScoreAvailable;
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] private GameObject gameOverEffect;

    [Header("Settings")]
    [SerializeField] private float showDelayTime = .40f;
    [SerializeField] private string scoreSceneName = "score";


    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        GameplayManager.OnGameOver += HandleGameOver;
        GameplayManager.OnGameStarted += HandleGameStarted;
    }

    private void GoToScores()
    {
        
        InputService.Instance.OnUISelect -= GoToScores;
        PlayerPrefs.SetInt("SCORE", GameplayManager.Instance.Score);
        SceneManager.LoadScene(scoreSceneName);
    }

    private void OnDisable()
    {
        GameplayManager.OnGameOver -= HandleGameOver;
        GameplayManager.OnGameStarted -= HandleGameStarted;

    }

    private void Start()
    {
        Hide();
    }

    private void HandleGameOver()
    {
        gameOverEffect?.SetActive(true);
        
        InputService.Instance.OnUISelect += GoToScores;
        Invoke(nameof(Show), showDelayTime);
    }

    private void HandleGameStarted()
    {
        Hide();
    }

    private void Show()
    {
        if (gameOverContainer != null)
            gameOverContainer.SetActive(true);

        txtScore.SetText(GameplayManager.Instance.Score.ToString());

        lblHighScoreAvailable.SetActive(HighScoreService.Instance.CheckIfScoreIsHighest(GameplayManager.Instance.Score));
        
    }

    private void Hide()
    {
        gameOverEffect?.SetActive(false);

        if (gameOverContainer != null)
            gameOverContainer.SetActive(false);
    }


}
