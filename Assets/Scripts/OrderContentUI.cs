using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderContentUI : MonoBehaviour
{
    [SerializeField] private TrayItemUI trayItemPrefab;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private TMP_Text txtRecipeName;
    [SerializeField] private Image imgRecipeSprite;

    private readonly List<TrayItemUI> _itemPool = new();

    private void Awake()
    {
        CollectExistingItems();
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

    public void UpdateOrderContent(Recipe recipe)
    {
        imgRecipeSprite.sprite = recipe.sprite;
        txtRecipeName.SetText(recipe.RecipeName);

        HideAllItems();

        for (int i = 0; i < recipe.recipeItems.Length; i++)
        {
            var recipeItem = recipe.recipeItems[i];
            var item = GetOrCreateItem(i);
            item.gameObject.SetActive(true);
            item.UpdateVisual(recipeItem.item.itemSprite, recipeItem.count);
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
