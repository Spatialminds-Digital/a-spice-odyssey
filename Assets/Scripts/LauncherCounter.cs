using UnityEngine;

public class LauncherCounter : BaseCounter
{
    public override void OnInteract()
    {
        base.OnInteract();

        if (Inventory.IsEmpty) return;

        //cannot launch ingredients
        if(PlayerInventory.Instance.HasIngredients && !PlayerInventory.Instance.HasRecipe) {
            OnCounterInteractionError?.Invoke();
            return;
        }

        // TODO: Add launching logic (animation, projectile, etc.)
        Inventory.ClearAll();
    }
}
