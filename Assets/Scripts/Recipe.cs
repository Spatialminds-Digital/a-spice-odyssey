using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public class Recipe : ScriptableObject
{
    public RecipeItems[] recipeItems;
}

[System.Serializable]
public class RecipeItems
{
    public RecipeItem item;
    public int count;
}
