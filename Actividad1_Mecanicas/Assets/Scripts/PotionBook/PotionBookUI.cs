using UnityEngine;
using UnityEngine.UI;

public class PotionBookUI : MonoBehaviour
{
    public PotionData potionData;
    public Image iconImage;

    void Start()
    {
        UpdateIcon();
        PotionBook.Instance.OnPotionDiscovered += OnPotionDiscovered;
    }

    void OnDestroy()
    {
        if (PotionBook.Instance != null)
            PotionBook.Instance.OnPotionDiscovered -= OnPotionDiscovered;
    }

    void OnPotionDiscovered(PotionResultType potion)
    {
        if (potion == potionData.potionType)
            UpdateIcon();
    }

    void UpdateIcon()
    {
        if (PotionBook.Instance.IsDiscovered(potionData.potionType))
            iconImage.sprite = potionData.discoveredIcon;
        else
            iconImage.sprite = potionData.undiscoveredIcon;
    }
}