using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private int shakeStrength = 1;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private int shakeVibrato = 10;

    private Vector3 originalPosition;
    private Tweener shakeTween;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake()
    {
        Shake(shakeStrength);
    }

    public void Shake(int strength)
    {
        shakeTween?.Kill();
        transform.localPosition = originalPosition;

        float shakeAmount = strength * 0.1f;
        shakeTween = transform.DOShakePosition(shakeDuration, shakeAmount, shakeVibrato)
            .OnComplete(() => transform.localPosition = originalPosition);
    }

    public void SetStrength(int strength)
    {
        shakeStrength = strength;
    }

    private void OnDestroy()
    {
        shakeTween?.Kill();
    }
}
