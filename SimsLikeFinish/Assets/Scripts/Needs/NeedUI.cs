using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image barNeed;
    [SerializeField] private TextMeshProUGUI nameNeed;

    public void Init(Sprite newIcon, float value, string newNameNeed)
    {
        icon.sprite = newIcon;
        barNeed.fillAmount = value / 100;
        nameNeed.text = newNameNeed;
    }

    public void SetNeedValue(float needValue)
    {
        barNeed.fillAmount = needValue/100;
    }
    
}
