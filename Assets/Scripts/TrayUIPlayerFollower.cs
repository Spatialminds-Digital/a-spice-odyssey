using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TrayUIPlayerFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector2 offset;
    [SerializeField] private float smoothSpeed = 10f;

    private RectTransform _rectTransform;
    private Canvas _canvas;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (target == null || mainCamera == null) return;

        Vector2 targetScreenPos = mainCamera.WorldToScreenPoint(target.position);
        targetScreenPos += offset;

        Vector2 clampedPos = ClampToScreen(targetScreenPos);

        Vector2 currentPos = _rectTransform.position;
        Vector2 smoothedPos = Vector2.Lerp(currentPos, clampedPos, smoothSpeed * Time.deltaTime);

        _rectTransform.position = smoothedPos;
    }

    private Vector2 ClampToScreen(Vector2 screenPos)
    {
        Vector2 size = _rectTransform.rect.size;
        Vector2 pivot = _rectTransform.pivot;

        float scaleFactor = _canvas != null ? _canvas.scaleFactor : 1f;
        Vector2 scaledSize = size * scaleFactor;

        float minX = scaledSize.x * pivot.x;
        float maxX = Screen.width - scaledSize.x * (1f - pivot.x);
        float minY = scaledSize.y * pivot.y;
        float maxY = Screen.height - scaledSize.y * (1f - pivot.y);

        screenPos.x = Mathf.Clamp(screenPos.x, minX, maxX);
        screenPos.y = Mathf.Clamp(screenPos.y, minY, maxY);

        return screenPos;
    }
}
