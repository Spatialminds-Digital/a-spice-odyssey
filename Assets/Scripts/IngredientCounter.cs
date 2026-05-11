using UnityEngine;

public class IngredientCounter : BaseCounter
{
    [SerializeField] private RecipeItem ingredient;

    public override void OnInteract()
    {
        base.OnInteract();

        //cannot pick if recipe in hand
        if(PlayerInventory.Instance.HasRecipe) {
            OnCounterInteractionError?.Invoke();
            return;
        }

        if (ingredient == null) return;

        Inventory.AddIngredient(ingredient);
    }
}
