using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-9)]
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public event Action OnInventoryChanged;

    private readonly List<RecipeItem> _heldIngredients = new();
    private Recipe _heldRecipe;
 
    public IReadOnlyList<RecipeItem> HeldIngredients => _heldIngredients;
    public Recipe HeldRecipe => _heldRecipe;

    public bool IsEmpty => _heldIngredients.Count == 0 && _heldRecipe == null;
    public bool HasIngredients => _heldIngredients.Count > 0;
    public bool HasRecipe => _heldRecipe != null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddIngredient(RecipeItem item)
    {
        if (item == null) return;

        _heldIngredients.Add(item);
        OnInventoryChanged?.Invoke();
    }

    public List<RecipeItem> TakeAllIngredients()
    {
        var items = new List<RecipeItem>(_heldIngredients);
        _heldIngredients.Clear();
        OnInventoryChanged?.Invoke();
        return items;
    }

    public void SetRecipe(Recipe recipe)
    {
        _heldRecipe = recipe;
        OnInventoryChanged?.Invoke();
    }

    public Recipe TakeRecipe()
    {
        var recipe = _heldRecipe;
        _heldRecipe = null;
        OnInventoryChanged?.Invoke();
        return recipe;
    }

    public void ClearAll()
    {
        _heldIngredients.Clear();
        _heldRecipe = null;
        OnInventoryChanged?.Invoke();
    }

    public Dictionary<RecipeItem, int> GetIngredientCounts()
    {
        return _heldIngredients
            .GroupBy(item => item)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public int GetItemCount(RecipeItem item)
    {
        return _heldIngredients.Count(i => i == item);
    }
}
