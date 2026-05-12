using System.Collections.Generic;
using UnityEngine;

public class RecipeItemContainerUI : MonoBehaviour
{
    [SerializeField] private TrayItemUI trayItemPrefab;
    [SerializeField] private Transform itemContainer;

    private readonly List<TrayItemUI> _itemPool = new();

    private void Start()
    {
        CollectExistingItems();
        PlayerInventory.Instance.OnInventoryChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (PlayerInventory.Instance != null)
        {
            PlayerInventory.Instance.OnInventoryChanged -= UpdateUI;
        }
    }

    private void CollectExistingItems()
    {
        var container = itemContainer != null ? itemContainer : transform;
        foreach (Transform child in container)
        {
            if (child.TryGetComponent<TrayItemUI>(out var item))
            {
                _itemPool.Add(item);
                item.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateUI()
    {
        var inventory = PlayerInventory.Instance;

        if (inventory.HasRecipe)
        {
            UpdateRecipeDisplay();
        }
        else if (inventory.HasIngredients)
        {
            UpdateIngredientsDisplay();
        }
        else
        {
            HideAllItems();
        }
    }

    private void UpdateRecipeDisplay()
    {
        HideAllItems();

        var recipe = PlayerInventory.Instance.HeldRecipe;
        var item = GetOrCreateItem(0);
        item.gameObject.SetActive(true);
        item.UpdateVisual(recipe.sprite, 1);
    }

    private void UpdateIngredientsDisplay()
    {
        HideAllItems();

        var ingredientCounts = PlayerInventory.Instance.GetIngredientCounts();
        int index = 0;

        foreach (var kvp in ingredientCounts)
        {
            var item = GetOrCreateItem(index);
            item.gameObject.SetActive(true);
            item.UpdateVisual(kvp.Key.itemSprite, kvp.Value);
            index++;
        }
    }

    private TrayItemUI GetOrCreateItem(int index)
    {
        if (index < _itemPool.Count)
        {
            return _itemPool[index];
        }

        var container = itemContainer != null ? itemContainer : transform;
        var newItem = Instantiate(trayItemPrefab, container);
        _itemPool.Add(newItem);
        return newItem;
    }

    private void HideAllItems()
    {
        foreach (var item in _itemPool)
        {
            item.gameObject.SetActive(false);
        }
    }
}
