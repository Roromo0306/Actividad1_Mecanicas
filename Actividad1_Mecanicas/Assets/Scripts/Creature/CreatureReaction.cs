using System.Collections;
using UnityEngine;

public class CreatureReaction : MonoBehaviour
{
    [Header("References")]
    public Renderer creatureRenderer;
    public ParticleSystem fireParticles;
    public Transform shrinkPoint;

    [Header("Effect Settings")]
    public float effectDuration = 3f;
    public float shrinkScale = 0.5f;
    public float shrinkDuration = 0.5f;

    Vector3 originalScale;
    Color originalColor;
    Coroutine currentEffect;

    void Start()
    {
        originalScale = transform.localScale;
        if (creatureRenderer)
            originalColor = creatureRenderer.material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        Potion potion = other.GetComponent<Potion>();
        if (potion != null)
        {
            GivePotion(potion);
        }
    }

    public void GivePotion(Potion potion)
    {
        
        if (currentEffect != null)
            StopCoroutine(currentEffect);

        ResetState();

        switch (potion.potionType)
        {
            case PotionResultType.Azul:
                currentEffect = StartCoroutine(BlueEffect());
                break;

            case PotionResultType.Encoger:
                currentEffect = StartCoroutine(ShrinkEffect());
                break;

            case PotionResultType.Fuego:
                currentEffect = StartCoroutine(FireEffect());
                break;
        }
        Debug.Log("Criatura recibe poción: " + potion.potionType);

        Destroy(potion.gameObject);
    }

    
    IEnumerator BlueEffect()
    {
        creatureRenderer.material.color = Color.blue;
        yield return new WaitForSeconds(effectDuration);
        ResetState();
    }

    IEnumerator FireEffect()
    {
        fireParticles.Play();
        yield return new WaitForSeconds(effectDuration);
        fireParticles.Stop();
        ResetState();
    }

    
    IEnumerator ShrinkEffect()
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = originalScale * shrinkScale;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / shrinkDuration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        yield return new WaitForSeconds(effectDuration);

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / shrinkDuration;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        ResetState();
    }

    
    void ResetState()
    {
        if (creatureRenderer)
            creatureRenderer.material.color = originalColor;

        transform.localScale = originalScale;

        if (fireParticles && fireParticles.isPlaying)
            fireParticles.Stop();
    }
}