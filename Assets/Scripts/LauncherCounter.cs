using UnityEngine;

public class LauncherCounter : BaseCounter
{
    [SerializeField] private OrderService orderService;
    public override void OnInteract()
    {
        base.OnInteract();

        if (Inventory.IsEmpty) return;

        //cannot launch ingredients
        if(PlayerInventory.Instance.HasIngredients && !PlayerInventory.Instance.HasRecipe) {
            OnCounterInteractionError?.Invoke();
            return;
        }

        //launch the recipe
        if(orderService!=null)
            orderService.CheckAndRemoveOrder(PlayerInventory.Instance.TakeRecipe());

        // TODO: Add launching logic (animation, projectile, etc.)
        Inventory.ClearAll();
    }
}
