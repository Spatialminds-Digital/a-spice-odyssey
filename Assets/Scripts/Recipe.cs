using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public class Recipe : ScriptableObject
{
    public string RecipeName;
    public Sprite sprite;
    public RecipeItems[] recipeItems;
}

[System.Serializable]
public class RecipeItems
{
    public RecipeItem item;
    public int count;
}
