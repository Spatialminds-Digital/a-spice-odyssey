using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverContainer;
    [SerializeField] private TMP_Text txtFinalScore;
    [SerializeField] private TMP_Text txtHighScore;
    [SerializeField] private Button btnRestart;
    [SerializeField] private Button btnMainMenu;

    [Header("Settings")]
    [SerializeField] private string finalScoreFormat = "Score: {0}";
    [SerializeField] private string highScoreFormat = "High Score: {0}";
    [SerializeField] private string highScoreKey = "HighScore";

    private int _highScore;

    private void Awake()
    {
        _highScore = PlayerPrefs.GetInt(highScoreKey, 0);
    }

    private void OnEnable()
    {
        GameplayManager.OnGameOver += HandleGameOver;
        GameplayManager.OnGameStarted += HandleGameStarted;

        if (btnRestart != null)
            btnRestart.onClick.AddListener(OnRestartClicked);

        if (btnMainMenu != null)
            btnMainMenu.onClick.AddListener(OnMainMenuClicked);
    }

    private void OnDisable()
    {
        GameplayManager.OnGameOver -= HandleGameOver;
        GameplayManager.OnGameStarted -= HandleGameStarted;

        if (btnRestart != null)
            btnRestart.onClick.RemoveListener(OnRestartClicked);

        if (btnMainMenu != null)
            btnMainMenu.onClick.RemoveListener(OnMainMenuClicked);
    }

    private void Start()
    {
        Hide();
    }

    private void HandleGameOver()
    {
        Show();
        UpdateScoreDisplay();
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
        if (gameOverContainer != null)
            gameOverContainer.SetActive(false);
    }

    private void UpdateScoreDisplay()
    {
        var manager = GameplayManager.Instance;
        if (manager == null) return;

        int finalScore = manager.Score;

        if (txtFinalScore != null)
        {
            txtFinalScore.SetText(string.Format(finalScoreFormat, finalScore));
        }

        // Check and update high score
        if (finalScore > _highScore)
        {
            _highScore = finalScore;
            PlayerPrefs.SetInt(highScoreKey, _highScore);
            PlayerPrefs.Save();
        }

        if (txtHighScore != null)
        {
            txtHighScore.SetText(string.Format(highScoreFormat, _highScore));
        }
    }

    private void OnRestartClicked()
    {
        var manager = GameplayManager.Instance;
        if (manager != null)
        {
            manager.RestartGame();
        }
    }

    private void OnMainMenuClicked()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
