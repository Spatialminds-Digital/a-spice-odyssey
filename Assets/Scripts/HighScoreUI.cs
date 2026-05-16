using UnityEngine;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] private Transform itemContainer;
    [SerializeField] private HighScoreUIElement scoreItemPrefab;

    void OnEnable()
    {
        HighScoreService.Instance.OnHighScoresLoaded += HandleHighScoreLoad;
    }

    void Oestroy()
    {
        HighScoreService.Instance.OnHighScoresLoaded -= HandleHighScoreLoad;
        
    }

    void OnDisable()
    {
        HighScoreService.Instance.OnHighScoresLoaded -= HandleHighScoreLoad;
    }

    private void HandleHighScoreLoad(SavedHighScores scores)
    {
        ClearAll();

        foreach(SavedScore score in scores.scores)
        {
            var element = Instantiate(scoreItemPrefab, itemContainer);
            element.SetScore(score.name, score.score);
        }
    }

    private void ClearAll()
    {
        foreach(Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
