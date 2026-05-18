using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    [Header("Countdown")]
    [SerializeField] private GameObject countdownContainer;
    [SerializeField] private TMP_Text txtCountdown;
    [SerializeField] private string goText = "GO!";

    [Header("Score")]
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] private string scoreFormat = "{0}";

    [Header("Lives")]
    [SerializeField] private TMP_Text txtLives;
    [SerializeField] private string livesFormat = "{0}";

    [Header("Complexity")]
    [SerializeField] private TMP_Text txtComplexity;
    [SerializeField] private string complexityFormat = "Level {0}";

    private void OnEnable()
    {
        GameplayManager.OnCountdownTick += HandleCountdownTick;
        GameplayManager.OnGameStarted += HandleGameStarted;
        GameplayManager.OnScoreChanged += HandleScoreChanged;
        GameplayManager.OnLivesChanged += HandleLivesChanged;
        GameplayManager.OnComplexityChanged += HandleComplexityChanged;

        InputService.Instance.OnUIStart += HandleExit;
    }

    private void OnDisable()
    {
        GameplayManager.OnCountdownTick -= HandleCountdownTick;
        GameplayManager.OnGameStarted -= HandleGameStarted;
        GameplayManager.OnScoreChanged -= HandleScoreChanged;
        GameplayManager.OnLivesChanged -= HandleLivesChanged;
        GameplayManager.OnComplexityChanged -= HandleComplexityChanged;

        InputService.Instance.OnUIStart -= HandleExit;

    }

    private void HandleExit()
    {
        SceneManager.LoadScene("Main");
    }


    private void Start()
    {
        if (countdownContainer != null)
            countdownContainer.SetActive(false);
    }

    private void HandleCountdownTick(int count)
    {
        if (countdownContainer != null)
            countdownContainer.SetActive(true);

        if (txtCountdown != null)
        {
            txtCountdown.SetText(count > 0 ? count.ToString() : goText);
            AudioService.Instance.PlayCountdown();
        }
    }

    private void HandleGameStarted()
    {
        if (countdownContainer != null)
            countdownContainer.SetActive(false);
    }

    private void HandleScoreChanged(int score)
    {
        if (txtScore != null)
        {
            txtScore.SetText(string.Format(scoreFormat, score));
        }
    }

    private void HandleLivesChanged(int lives)
    {
        if (txtLives != null)
        {
            txtLives.SetText(string.Format(livesFormat, lives));
        }
    }

    private void HandleComplexityChanged(int complexity)
    {
        if (txtComplexity != null)
        {
            txtComplexity.SetText(string.Format(complexityFormat, complexity));
        }
    }

    public void RefreshUI()
    {
        var manager = GameplayManager.Instance;
        if (manager == null) return;

        HandleScoreChanged(manager.Score);
        HandleLivesChanged(manager.Lives);
        HandleComplexityChanged(manager.Complexity);
    }
}
