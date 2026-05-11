using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CookingCounter : BaseCounter
{
    [SerializeField] private Recipe[] availableRecipes;

    private Recipe _cookedRecipe;

    public bool HasCookedRecipe => _cookedRecipe != null;

    public override void OnInteract()
    {
        if (_cookedRecipe != null)
        {
            TryPickupCookedRecipe();
        }
        else if (Inventory.HasIngredients)
        {
            TryCookRecipe();
        }
    }

    private void TryPickupCookedRecipe()
    {
        if (!Inventory.IsEmpty) return;

        Inventory.SetRecipe(_cookedRecipe);
        _cookedRecipe = null;
    }

    private void TryCookRecipe()
    {
        var ingredients = Inventory.TakeAllIngredients();
        var matchedRecipe = FindMatchingRecipe(ingredients);

        if (matchedRecipe != null)
        {
            _cookedRecipe = matchedRecipe;
        }
    }

    private Recipe FindMatchingRecipe(List<RecipeItem> ingredients)
    {
        var ingredientCounts = ingredients
            .GroupBy(item => item)
            .ToDictionary(g => g.Key, g => g.Count());

        foreach (var recipe in availableRecipes)
        {
            if (RecipeMatches(recipe, ingredientCounts))
                return recipe;
        }

        return null;
    }

    private bool RecipeMatches(Recipe recipe, Dictionary<RecipeItem, int> ingredientCounts)
    {
        if (recipe.recipeItems.Length != ingredientCounts.Count)
            return false;

        foreach (var recipeItem in recipe.recipeItems)
        {
            if (!ingredientCounts.TryGetValue(recipeItem.item, out int count))
                return false;

            if (count != recipeItem.count)
                return false;
        }

        return true;
    }
}
