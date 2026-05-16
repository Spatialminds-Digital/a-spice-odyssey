using TMPro;
using UnityEngine;

public class HighScoreUIElement : MonoBehaviour
{
    [SerializeField] private TMP_Text txtName;
    [SerializeField] private TMP_Text txtScore;

    public void SetScore(string name, int score)
    {
        txtName.SetText(name);
        txtScore.SetText(score.ToString());
    }
}
