using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private IInteractable _currentInteractable;

    private void OnEnable()
    {
        InputService.Instance.OnInteract += HandleInteract;
    }

    private void OnDisable()
    {
        InputService.Instance.OnInteract -= HandleInteract;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IInteractable interactable))
            return;

        if (interactable == _currentInteractable)
            return;

        _currentInteractable?.OnHoverOut();
        _currentInteractable = interactable;
        _currentInteractable.OnHoverIn();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out IInteractable interactable))
            return;

        if (interactable != _currentInteractable)
            return;

        _currentInteractable.OnHoverOut();
        _currentInteractable = null;
    }

    private void HandleInteract()
    {
        _currentInteractable?.OnInteract();
    }
}