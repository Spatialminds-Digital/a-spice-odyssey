using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 5f;
    [SerializeField] private float Acceleration = 20f;
    [SerializeField] private float Deceleration = 15f;

    private float _moveInput;
    private float _currentVelocity;
    private Camera _mainCamera;
    private bool _isFacingRight = true;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void OnEnable()
    {
        if (InputService.Instance != null)
        {
            InputService.Instance.OnMove += HandleMove;
        }
    }

    void OnDisable()
    {
        if (InputService.Instance != null)
        {
            InputService.Instance.OnMove -= HandleMove;
        }
    }

    void Update()
    {
        ApplyMovement();
        UpdateFacingDirection();
        ClampToScreenBounds();
    }

    private void HandleMove(float input)
    {
        _moveInput = input;
    }

    private void ApplyMovement()
    {
        // Arcade-style movement with smooth acceleration/deceleration
        if (Mathf.Abs(_moveInput) > 0.01f)
        {
            // Accelerate towards target speed
            _currentVelocity = Mathf.MoveTowards(_currentVelocity, _moveInput * MoveSpeed, Acceleration * Time.deltaTime);
        }
        else
        {
            // Decelerate to stop
            _currentVelocity = Mathf.MoveTowards(_currentVelocity, 0f, Deceleration * Time.deltaTime);
        }

        // Apply movement
        transform.position += new Vector3(_currentVelocity * Time.deltaTime, 0f, 0f);
    }

    private void ClampToScreenBounds()
    {
        if (_mainCamera == null)
            return;

        // Get screen bounds in world space
        Vector3 viewportPosition = _mainCamera.WorldToViewportPoint(transform.position);

        // Clamp to screen bounds (0 to 1 in viewport space)
        viewportPosition.x = Mathf.Clamp01(viewportPosition.x);

        // Convert back to world position
        Vector3 clampedPosition = _mainCamera.ViewportToWorldPoint(viewportPosition);
        clampedPosition.z = transform.position.z; // Maintain original z position

        transform.position = clampedPosition;
    }

    private void UpdateFacingDirection()
    {
        // Only update facing direction when there's actual input
        if (Mathf.Abs(_moveInput) > 0.01f)
        {
            if (_moveInput > 0 && !_isFacingRight)
            {
                Flip();
            }
            else if (_moveInput < 0 && _isFacingRight)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
