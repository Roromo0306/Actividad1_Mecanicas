using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PotionDiscoverPopup : MonoBehaviour
{
    public Image iconImage;

    [Header("Animation Settings")]
    public float growDuration = 0.5f;
    public float stayDuration = 5f;
    public Vector3 startScale = Vector3.zero;
    public Vector3 endScale = Vector3.one;
    public float overshoot = 1.2f; 

    private Coroutine currentRoutine;

    void Start()
    {
        transform.localScale = startScale;
        iconImage.enabled = false; 
        PotionBook.Instance.OnPotionDiscovered += ShowPotion;
    }

    void OnDestroy()
    {
        if (PotionBook.Instance != null)
            PotionBook.Instance.OnPotionDiscovered -= ShowPotion;
    }

    void ShowPotion(PotionResultType potionType)
    {
        PotionData data = PotionDatabase.Instance.GetPotionData(potionType);
        if (data == null) return;

        iconImage.sprite = data.discoveredIcon;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(PopupRoutine());
    }

    IEnumerator PopupRoutine()
    {
        iconImage.enabled = true;
        transform.localScale = startScale;

        float t = 0f;

        // 🔹 Escala con “overshoot” (rebote)
        while (t < 1f)
        {
            t += Time.deltaTime / growDuration;
            // Escala usando función de rebote simple
            float scaleFactor = Mathf.Lerp(0f, overshoot, t);
            scaleFactor = Mathf.Sin(scaleFactor * Mathf.PI * 0.5f); // suaviza el rebote
            transform.localScale = Vector3.Lerp(startScale, endScale, scaleFactor);
            yield return null;
        }

        transform.localScale = endScale;

       
        yield return new WaitForSeconds(stayDuration);

       
        iconImage.enabled = false;
    }
}