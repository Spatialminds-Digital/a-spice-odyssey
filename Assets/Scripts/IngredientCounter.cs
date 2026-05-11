using UnityEngine;

public class IngredientCounter : BaseCounter
{
    [SerializeField] private RecipeItem ingredient;

    public override void OnInteract()
    {
        if (ingredient == null) return;

        Inventory.AddIngredient(ingredient);
    }
}
