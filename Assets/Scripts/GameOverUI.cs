using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverContainer;
    [SerializeField] private TMP_Text txtHighScoreAvailable;
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
        InputService.Instance.OnUISelect += GoToScores;
    }

    private void GoToScores()
    {
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
    }

    private void Hide()
    {
        gameOverEffect?.SetActive(false);

        if (gameOverContainer != null)
            gameOverContainer.SetActive(false);
    }


}
