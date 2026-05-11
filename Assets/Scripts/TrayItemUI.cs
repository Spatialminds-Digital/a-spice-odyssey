using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrayItemUI : MonoBehaviour
{
    [SerializeField] private Image imgItem;
    [SerializeField] private TMP_Text txtCount;

    public void UpdateVisual(Sprite itemSprite, int itemCount)
    {
        imgItem.sprite = itemSprite;
        txtCount.SetText(itemCount.ToString());
    }

}
