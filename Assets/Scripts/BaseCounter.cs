using UnityEngine;
using UnityEngine.Events;

public abstract class BaseCounter : MonoBehaviour, IInteractable
{
    protected PlayerInventory Inventory => PlayerInventory.Instance;

    public UnityEvent OnHoverInEvent;
    public UnityEvent OnHoverOutEvent;
    public UnityEvent OnInteractEvent;

    public virtual void OnHoverIn()
    {
        OnHoverInEvent?.Invoke();
    }

    public virtual void OnHoverOut()
    {
        OnHoverOutEvent?.Invoke();
    }

    public virtual void OnInteract()
    {
        OnInteractEvent?.Invoke();
    }
}
