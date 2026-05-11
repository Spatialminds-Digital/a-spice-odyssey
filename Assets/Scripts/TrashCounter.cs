using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void OnInteract()
    {
        base.OnInteract();

        if (Inventory.IsEmpty) return;

        Inventory.ClearAll();
    }
}
