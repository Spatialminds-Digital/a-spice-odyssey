using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoreInput : MonoBehaviour
{
    [SerializeField] private GameObject highScoreInputUI;
    [SerializeField] private Transform indicator;
    [SerializeField] private TMP_Text[] charTexts;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string nextScene = "main";

    private short _selectedCharacter = 0;
    private short _charFromAlphabet = 0;
    private static string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private int _score;
    

    void OnEnable()
    {
        InputService.Instance.OnUIUp += UpdateCharUp;
        InputService.Instance.OnUIDown += UpdateCharDown;
        InputService.Instance.OnUISelect += NextChar;
        InputService.Instance.OnUIStart += SaveScore;
    }

    void OnDisable()
    {
        
        InputService.Instance.OnUIUp -= UpdateCharUp;
        InputService.Instance.OnUIDown -= UpdateCharDown;
        InputService.Instance.OnUISelect -= NextChar;
        InputService.Instance.OnUIStart -= SaveScore;
    }

    private void SaveScore()
    {
        string username = "";
        foreach(TMP_Text text in charTexts)
        {
            username += text.text;
        }

        HighScoreService.Instance.AddHighScore(username, _score);
        SceneManager.LoadScene(nextScene);
    }

    private void NextChar()
    {
        _selectedCharacter++;
        if(_selectedCharacter > charTexts.Length) _selectedCharacter = 0;

        indicator.position = new Vector2(charTexts[_selectedCharacter].transform.position.x, indicator.position.y);
    }

    private void UpdateCharDown()
    {
        _charFromAlphabet--;
        if(_charFromAlphabet < 0) _charFromAlphabet = (short)(alphabet.Length -1);
        charTexts[_selectedCharacter].SetText(alphabet[_charFromAlphabet].ToString());
    }

    private void UpdateCharUp()
    {
        _charFromAlphabet++;
        if(_charFromAlphabet > alphabet.Length) _charFromAlphabet = 0;
        charTexts[_selectedCharacter].SetText(alphabet[_charFromAlphabet].ToString());
    }

    public void ShowHighScoreInput(int score)
    {
        _selectedCharacter = 0;
        _score = score;

        indicator.position = new Vector2(charTexts[_selectedCharacter].transform.position.x, indicator.position.y);
        highScoreInputUI.SetActive(true);
    }
}
