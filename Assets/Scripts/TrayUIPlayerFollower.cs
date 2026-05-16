using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TrayUIPlayerFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector2 offset;
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private Canvas canvas;

    private RectTransform _rectTransform;
    private RectTransform _canvasRectTransform;
    private Camera _canvasCamera;

    private Vector2 _targetScreenPos;

    private Vector2 _smoothPos;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        _canvasRectTransform = canvas.GetComponent<RectTransform>();
        _canvasCamera = canvas.worldCamera;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (target == null || mainCamera == null) return;

        _targetScreenPos = mainCamera.WorldToScreenPoint(target.position + (Vector3)offset);

        //Vector2 clampedScreenPos = ClampToScreen(targetScreenPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRectTransform,
            _targetScreenPos,
            _canvasCamera,
            out Vector2 localPoint
        );

       // Vector2 currentLocalPos = _rectTransform.anchoredPosition;
        _smoothPos = Vector2.Lerp(_rectTransform.anchoredPosition, localPoint, smoothSpeed * Time.deltaTime);
       
       _rectTransform.anchoredPosition = _smoothPos;
       //transform.position = smoothedPos;
    }

   /* private Vector2 ClampToScreen(Vector2 screenPos)
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
    }*/
}
