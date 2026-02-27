using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PotionBookUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public PotionData potionData;
    public Image iconImage;

    [Header("Description UI")]
    public TMP_Text descriptionText;

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
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PotionBook.Instance.IsDiscovered(potionData.potionType))
        {
            descriptionText.text = potionData.description;
            descriptionText.gameObject.SetActive(true);
        }
        else
        {
            descriptionText.text = "???";
            descriptionText.gameObject.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionText.gameObject.SetActive(false);
        descriptionText.text = "";
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