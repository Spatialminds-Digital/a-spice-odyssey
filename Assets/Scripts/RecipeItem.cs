using UnityEngine;

[CreateAssetMenu(fileName = "RecipeItem", menuName = "Scriptable Objects/RecipeItem")]
public class RecipeItem : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public int itemValue;
}
