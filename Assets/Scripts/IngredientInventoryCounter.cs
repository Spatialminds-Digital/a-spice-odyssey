using System;
using TMPro;
using UnityEngine;

public class IngredientInventoryCounter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer recipeItemImage;
    [SerializeField] private RecipeItem recipeItem;

    void Start()
    {
        recipeItemImage.sprite = recipeItem.itemSprite;
    }
    
    void OnEnable()
    {
        PlayerInventory.Instance.OnInventoryChanged += OnInventoryChanged;
    }

    private void OnInventoryChanged()
    {
    }

    void OnDisable()
    {
        PlayerInventory.Instance.OnInventoryChanged += OnInventoryChanged;
    }
}
