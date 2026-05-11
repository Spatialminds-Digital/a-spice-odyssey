using System;
using UnityEngine;

public class QuickTimeEvent : MonoBehaviour
{
    [Header("QTE Settings")]
    [SerializeField] private float oscillationSpeed = 2f;
    [SerializeField, Range(0.05f, 0.5f)] private float greenZoneSize = 0.2f;
    [SerializeField, Range(0f, 1f)] private float greenZoneCenter = 0.5f;

    public event Action<bool> OnQTEComplete;
    public event Action OnQTEStarted;
    public event Action<float> OnIndicatorMoved;

    public float GreenZoneSize => greenZoneSize;
    public float GreenZoneCenter => greenZoneCenter;
    public float IndicatorPosition => _indicatorPosition;
    public bool IsActive => _isActive;

    private float _indicatorPosition;
    private bool _isActive;
    private bool _movingRight = true;

    private void OnEnable()
    {
        InputService.Instance.OnInteract += HandleInput;
    }

    private void OnDisable()
    {
        if (InputService.Instance != null)
            InputService.Instance.OnInteract -= HandleInput;
    }

    private void Update()
    {
        if (!_isActive) return;

        UpdateIndicatorPosition();
    }

    private void UpdateIndicatorPosition()
    {
        float delta = oscillationSpeed * Time.deltaTime;

        if (_movingRight)
        {
            _indicatorPosition += delta;
            if (_indicatorPosition >= 1f)
            {
                _indicatorPosition = 1f;
                _movingRight = false;
            }
        }
        else
        {
            _indicatorPosition -= delta;
            if (_indicatorPosition <= 0f)
            {
                _indicatorPosition = 0f;
                _movingRight = true;
            }
        }

        OnIndicatorMoved?.Invoke(_indicatorPosition);
    }

    public void StartQTE()
    {
        _isActive = true;
        _indicatorPosition = 0f;
        _movingRight = true;
        OnQTEStarted?.Invoke();
    }

    public void StopQTE()
    {
        _isActive = false;
    }

    private void HandleInput()
    {
        if (!_isActive) return;

        bool success = IsInGreenZone();
        _isActive = false;
        OnQTEComplete?.Invoke(success);
    }

    private bool IsInGreenZone()
    {
        float halfSize = greenZoneSize / 2f;
        float minGreen = greenZoneCenter - halfSize;
        float maxGreen = greenZoneCenter + halfSize;

        return _indicatorPosition >= minGreen && _indicatorPosition <= maxGreen;
    }
}
