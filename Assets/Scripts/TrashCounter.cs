using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void OnInteract()
    {
        if (Inventory.IsEmpty) return;

        Inventory.ClearAll();
    }
}
