using DG.Tweening;
using TMPro;
using UnityEngine;

public class EffectService : MonoBehaviour
{
    public static EffectService Instance;

    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private GameObject redVignette;
    [SerializeField] private TMP_Text messageText;

    public CameraShake camShake => cameraShake;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
    }

    void Start()
    {
        redVignette.SetActive(false);
        messageText.gameObject.SetActive(false);
    }

    public void ShowRedVignette(float time)
    {
        redVignette.SetActive(true);
        Invoke(nameof(HideRedVignette), time);
    }

    public void HideRedVignette()
    {
        redVignette.SetActive(false);
    }

    public void ShowMessage(string text, Color color, float time=.5f)
    {
        messageText.transform.localScale = Vector2.zero;
        messageText.SetText(text);
        messageText.color = color;
                messageText.gameObject.SetActive(true); 


        messageText.transform.DOScale(Vector2.one, time).SetEase(Ease.OutBack).OnComplete(() =>
        {
           messageText.transform.DOScale(Vector2.zero, time/2).SetEase(Ease.InBack).OnComplete(() =>
            {
                messageText.gameObject.SetActive(false); 
            });
        });
        
    }


}
