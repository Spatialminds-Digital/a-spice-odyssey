using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreScene : MonoBehaviour
{
    [SerializeField] private HighScoreInput highScoreInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(HighScoreService.Instance.CheckIfScoreIsHigh(PlayerPrefs.GetInt("SCORE")))
        {
            highScoreInput.ShowHighScoreInput(PlayerPrefs.GetInt("SCORE"));
            return;
        }

        InputService.Instance.OnUISelect += LoadMenu;
    }

    private void LoadMenu()
    {
        InputService.Instance.OnUISelect -= LoadMenu;
        SceneManager.LoadScene("menu");
    }
}
