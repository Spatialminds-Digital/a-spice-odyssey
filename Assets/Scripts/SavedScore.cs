using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedScore
{
    public string name;
    public int score;

}

[System.Serializable]
public class SavedHighScores
{
    public List<SavedScore> scores;
}