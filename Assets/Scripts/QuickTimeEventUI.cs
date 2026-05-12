using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEventUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuickTimeEvent quickTimeEvent;
    [SerializeField] private GameObject uiContainer;
    [SerializeField] private RectTransform barBackground;
    [SerializeField] private RectTransform greenZone;
    [SerializeField] private RectTransform indicator;

    [Header("Colors")]
    [SerializeField] private Color greenColor = Color.green;
    [SerializeField] private Color redColor = Color.red;

    private float _barWidth;

    private void Awake()
    {
        if (barBackground != null)
            _barWidth = barBackground.rect.width;

        Hide();
    }

    private void OnEnable()
    {
        if (quickTimeEvent != null)
        {
            quickTimeEvent.OnQTEStarted += Show;
            quickTimeEvent.OnQTEComplete += OnQTEComplete;
            quickTimeEvent.OnIndicatorMoved += UpdateIndicator;
        }
    }

    private void OnDisable()
    {
        if (quickTimeEvent != null)
        {
            quickTimeEvent.OnQTEStarted -= Show;
            quickTimeEvent.OnQTEComplete -= OnQTEComplete;
            quickTimeEvent.OnIndicatorMoved -= UpdateIndicator;
        }
    }

    private void Show()
    {
        UpdateGreenZone();
        uiContainer.SetActive(true);
    }

    private void Hide()
    {
        uiContainer.SetActive(false);
    }

    private void OnQTEComplete(bool success)
    {
        Hide();
    }

    private void UpdateGreenZone()
    {
        if (greenZone == null || quickTimeEvent == null) return;

        float zoneWidth = _barWidth * quickTimeEvent.GreenZoneSize;

        // Use the clamped center to ensure the green zone stays within the bar
        float centerPosition = _barWidth * quickTimeEvent.ClampedGreenZoneCenter;

        greenZone.sizeDelta = new Vector2(zoneWidth, greenZone.sizeDelta.y);
        greenZone.anchoredPosition = new Vector2(centerPosition - (_barWidth / 2f), greenZone.anchoredPosition.y);

        var image = greenZone.GetComponent<Image>();
        if (image != null)
            image.color = greenColor;
    }

    private void UpdateIndicator(float normalizedPosition)
    {
        if (indicator == null) return;

        float xPos = (normalizedPosition * _barWidth) - (_barWidth / 2f);
        indicator.anchoredPosition = new Vector2(xPos, indicator.anchoredPosition.y);
    }
}
