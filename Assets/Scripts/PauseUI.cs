using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseContainer;
    [SerializeField] private Button btnResume;
    [SerializeField] private Button btnRestart;
    [SerializeField] private Button btnMainMenu;

    private void OnEnable()
    {
        GameplayManager.OnGamePaused += HandleGamePaused;

        if (btnResume != null)
            btnResume.onClick.AddListener(OnResumeClicked);

        if (btnRestart != null)
            btnRestart.onClick.AddListener(OnRestartClicked);

        if (btnMainMenu != null)
            btnMainMenu.onClick.AddListener(OnMainMenuClicked);
    }

    private void OnDisable()
    {
        GameplayManager.OnGamePaused -= HandleGamePaused;

        if (btnResume != null)
            btnResume.onClick.RemoveListener(OnResumeClicked);

        if (btnRestart != null)
            btnRestart.onClick.RemoveListener(OnRestartClicked);

        if (btnMainMenu != null)
            btnMainMenu.onClick.RemoveListener(OnMainMenuClicked);
    }

    private void Start()
    {
        Hide();
    }

    private void HandleGamePaused(bool isPaused)
    {
        if (isPaused)
            Show();
        else
            Hide();
    }

    private void Show()
    {
        if (pauseContainer != null)
            pauseContainer.SetActive(true);
    }

    private void Hide()
    {
        if (pauseContainer != null)
            pauseContainer.SetActive(false);
    }

    private void OnResumeClicked()
    {
        var manager = GameplayManager.Instance;
        if (manager != null)
        {
            manager.ResumeGame();
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
