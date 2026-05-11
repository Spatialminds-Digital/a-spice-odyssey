using System;
using TMPro;
using UnityEngine;

public class IngredientInventoryCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text txtItemCount;
    [SerializeField] private RecipeItem recipeItem;

    void OnEnable()
    {
        PlayerInventory.Instance.OnInventoryChanged += OnInventoryChanged;
    }

    private void OnInventoryChanged()
    {
        var itemCount = PlayerInventory.Instance.GetItemCount(recipeItem);
        txtItemCount.SetText(itemCount == 0 ? "" : itemCount.ToString());
    }

    void OnDisable()
    {
        PlayerInventory.Instance.OnInventoryChanged += OnInventoryChanged;
    }
}
