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

    [Header("Extra Effects")]
    public MeshFilter meshFilter;
    public Mesh normalMesh;
    public Mesh frogMesh;

    public ParticleSystem colorParticles;
    public ParticleSystem glowParticles;

    public float growScale = 1.5f;
    public float floatHeight = 1.5f;
    public float floatSpeed = 2f;

    [Header("Materials")]
    public Material normalMaterial;
    public Material invisibleMaterial;
    Material runtimeNormalMaterial;
    Material runtimeInvisibleMaterial;

    [Header("AudioEffects")]
    public AudioSource audioSource;
    public AudioClip frogSFX;
    public AudioClip fireSFX;
    public AudioClip popSFX;

    Vector3 originalScale;
    Color originalColor;
    Coroutine currentEffect;

    void Start()
    {
        originalScale = transform.localScale;

        if (creatureRenderer)
        {
            // crear instancias separadas de los materiales
            runtimeNormalMaterial = new Material(normalMaterial);
            runtimeInvisibleMaterial = new Material(invisibleMaterial);

            creatureRenderer.material = runtimeNormalMaterial; // empieza visible
            originalColor = runtimeNormalMaterial.color;
        }
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

            case PotionResultType.Rojo:
                currentEffect = StartCoroutine(ColorEffect(Color.red));
                break;

            case PotionResultType.Verde:
                currentEffect = StartCoroutine(ColorEffect(Color.green));
                break;

            case PotionResultType.Agrandar:
                currentEffect = StartCoroutine(GrowFromPoint());
                break;

            case PotionResultType.Invisibilidad:
                currentEffect = StartCoroutine(InvisibilityEffect());
                break;

            case PotionResultType.Flotar:
                currentEffect = StartCoroutine(FloatSEffect());
                break;

            case PotionResultType.Rana:
                currentEffect = StartCoroutine(FrogEffect());
                break;

            case PotionResultType.FuegoColores:
                currentEffect = StartCoroutine(ColorFireEffect());
                break;

            case PotionResultType.Brillar:
                currentEffect = StartCoroutine(GlowAura());
                break;

            case PotionResultType.BrillarColores:
                currentEffect = StartCoroutine(RainbowEffect());
                break;
        }
        Debug.Log("Criatura recibe poción: " + potion.potionType);

        Destroy(potion.gameObject);
    }

    IEnumerator ColorEffect(Color c)
    {
        creatureRenderer.material.color = c;
        PlaySFX(audioSource, popSFX);
        yield return new WaitForSeconds(
               Mathf.Max(effectDuration, popSFX.length));
        
        ResetState();
    }

    IEnumerator InvisibilityEffect()
    {
        creatureRenderer.material = runtimeInvisibleMaterial;

        Color c = runtimeInvisibleMaterial.color;
        c.a = 0.15f;
        runtimeInvisibleMaterial.color = c;

        yield return new WaitForSeconds(effectDuration);

        ResetState();
    }
    IEnumerator FloatSEffect()
    {
        Vector3 startPos = transform.position;
        float t = 0f;

        while (t < effectDuration)
        {
            float y = Mathf.Sin(t * floatSpeed) * 0.5f;
            float x = Mathf.Sin(t * floatSpeed * 0.5f) * 0.5f;

            transform.position = startPos + new Vector3(x, y + floatHeight, 0);
            t += Time.deltaTime;
            yield return null;
        }

        // caída
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, startPos, t);
            yield return null;
        }

        transform.position = startPos;
        ResetState();
    }
    IEnumerator FrogEffect()
    {
        meshFilter.mesh = frogMesh;
        PlaySFX(audioSource, frogSFX);
        yield return new WaitForSeconds(
             Mathf.Max(effectDuration, frogSFX.length)
         ); meshFilter.mesh = normalMesh;
        ResetState();
    }

    IEnumerator ColorFireEffect()
    {
        fireParticles.Play();
        colorParticles.Play();

        yield return new WaitForSeconds(effectDuration);

        fireParticles.Stop();
        colorParticles.Stop();
        ResetState();
    }
    IEnumerator RainbowEffect()
    {
        float t = 0f;

        while (t < effectDuration)
        {
            float h = Mathf.PingPong(Time.time, 1f);
            creatureRenderer.material.color = Color.HSVToRGB(h, 1f, 1f);
            t += Time.deltaTime;
            yield return null;
        }

        ResetState();
    }
    IEnumerator GlowAura()
    {
        glowParticles.Play();
        yield return new WaitForSeconds(effectDuration);
        glowParticles.Stop();
        ResetState();
    }
    IEnumerator GrowFromPoint()
    {
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = originalScale * growScale;
        transform.localScale = startScale;

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
    IEnumerator BlueEffect()
    {
        creatureRenderer.material.color = Color.blue;
        PlaySFX(audioSource, popSFX);
        yield return new WaitForSeconds(
               Mathf.Max(effectDuration, popSFX.length));
        
        ResetState();
    }

    IEnumerator FireEffect()
    {
        fireParticles.Play();
        PlaySFX(audioSource, fireSFX);
        yield return new WaitForSeconds(
                    Mathf.Max(effectDuration, fireSFX.length)
                ); fireParticles.Stop();
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
        {
            creatureRenderer.material = runtimeNormalMaterial;
            creatureRenderer.material.color = originalColor;
        }

        transform.localScale = originalScale;

        if (fireParticles && fireParticles.isPlaying)
            fireParticles.Stop();
    }

    public void PlaySFX(AudioSource source, AudioClip clip)
    {
        if (source == null || clip == null) return;

        // Crea un AudioSource temporal para no cortar otros sonidos
        AudioSource temp = source.gameObject.AddComponent<AudioSource>();
        temp.clip = clip;
        temp.volume = source.volume;
        temp.spatialBlend = source.spatialBlend;
        temp.Play();

        
    }
}