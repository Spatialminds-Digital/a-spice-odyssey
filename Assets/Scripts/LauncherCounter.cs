using UnityEngine;

public class LauncherCounter : BaseCounter
{
    public override void OnInteract()
    {
        if (Inventory.IsEmpty) return;

        // TODO: Add launching logic (animation, projectile, etc.)
        Inventory.ClearAll();
    }
}
