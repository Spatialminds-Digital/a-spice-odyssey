using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IInteractable
{
    [SerializeField] protected GameObject hoverVisual;

    protected PlayerInventory Inventory => PlayerInventory.Instance;

    public virtual void OnHoverIn()
    {
        if (hoverVisual != null)
            hoverVisual.SetActive(true);
    }

    public virtual void OnHoverOut()
    {
        if (hoverVisual != null)
            hoverVisual.SetActive(false);
    }

    public abstract void OnInteract();
}
