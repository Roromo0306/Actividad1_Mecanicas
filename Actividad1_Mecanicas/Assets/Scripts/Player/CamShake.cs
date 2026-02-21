using System.Collections;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    public static CamShake Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public IEnumerator Shake(float duration = 0.3f, float magnitude = 0.2f)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public void TriggerShake(float duration = 0.3f, float magnitude = 0.2f)
    {
        StopAllCoroutines();
        StartCoroutine(Shake(duration, magnitude));
    }
}