using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CookingCounter : BaseCounter
{
    [SerializeField] private Recipe[] availableRecipes;
    [SerializeField] private Recipe trashRecipe;
    [SerializeField] private QuickTimeEvent quickTimeEvent;
    [SerializeField] private GameObject cookingParticle;

    private Recipe _cookedRecipe;
    private List<RecipeItem> _pendingIngredients;

    public bool HasCookedRecipe => _cookedRecipe != null;

    public UnityEvent OnCookingStart;
    public UnityEvent OnCookingComplete;

    private void OnEnable()
    {
        if (quickTimeEvent != null)
            quickTimeEvent.OnQTEComplete += OnQTEComplete;
    }

    private void OnDisable()
    {
        if (quickTimeEvent != null)
            quickTimeEvent.OnQTEComplete -= OnQTEComplete;
    }

    public override void OnInteract()
    {
        base.OnInteract();

        //no interaction if a recipe at hand
        if(PlayerInventory.Instance.HasRecipe) {
            OnCounterInteractionError?.Invoke();
            return;
        }

        if (quickTimeEvent != null && quickTimeEvent.IsActive)
            return;

        if (_cookedRecipe != null)
        {
            TryPickupCookedRecipe();
        }
        else if (Inventory.HasIngredients)
        {
            StartCooking();
        }
    }

    private void TryPickupCookedRecipe()
    {
        if (!Inventory.IsEmpty) return;

        Inventory.SetRecipe(_cookedRecipe);
        _cookedRecipe = null;
    }

    private void StartCooking()
    {
        _pendingIngredients = Inventory.TakeAllIngredients();

        if (quickTimeEvent != null)
        {
            quickTimeEvent.StartQTE();
            //stop player movement
            GameplayManager.Instance.playerMovement.enabled = false;
            cookingParticle?.SetActive(true);
            OnCookingStart?.Invoke();
        }
        else
        {
            CompleteCooking(true);


        }
    }

    private void OnQTEComplete(bool success)
    {
        CompleteCooking(success);
    }

    private void CompleteCooking(bool qteSuccess)
    {
        if (_pendingIngredients == null) return;

        if (!qteSuccess)
        {
            _cookedRecipe = trashRecipe;
        }
        else
        {
            var matchedRecipe = FindMatchingRecipe(_pendingIngredients);
            _cookedRecipe = matchedRecipe != null ? matchedRecipe : trashRecipe;
        }

        _pendingIngredients = null;

        
        GameplayManager.Instance.playerMovement.enabled = true;
        cookingParticle?.SetActive(false);
        OnCookingComplete?.Invoke();
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
