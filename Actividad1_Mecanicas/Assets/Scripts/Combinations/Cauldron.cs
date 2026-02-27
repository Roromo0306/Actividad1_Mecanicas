using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public List<IngredientType> currentIngredients = new List<IngredientType>();
    public int maxIngredients = 3;

    [Header("Spawn Points")]
    public Transform azulSpawn;
    public Transform rojoSpawn;
    public Transform verdeSpawn;
    public Transform agrandarSpawn;
    public Transform encogerSpawn;
    public Transform invisibilidadSpawn;
    public Transform flotarSpawn;
    public Transform ranaSpawn;
    public Transform fuegoSpawn;
    public Transform fuegoColoresSpawn;
    public Transform brillarSpawn;
    public Transform brillarColoresSpawn;

    [Header("Potion Prefabs")]
    public GameObject potionAzulPrefab;
    public GameObject potionRojoPrefab;
    public GameObject potionVerdePrefab;
    public GameObject potionAgrandarPrefab;
    public GameObject potionEncogerPrefab;
    public GameObject potionInvisibilidadPrefab;
    public GameObject potionFlotarPrefab;
    public GameObject potionRanaPrefab;
    public GameObject potionFuegoPrefab;
    public GameObject potionFuegoColoresPrefab;
    public GameObject potionBrillarPrefab;
    public GameObject potionBrillarColoresPrefab;

    [Header("Board Points")]
    public Transform[] boardPoints = new Transform[3];

    [Header("Board Icons")]
    public GameObject iconFlorAzul;
    public GameObject iconFlorRoja;
    public GameObject iconFlorVerde;
    public GameObject iconSetas;
    public GameObject iconPolvosMagicos;
    public GameObject iconSustanciaMaligna;
    public GameObject iconEscamasCamaleon;
    public GameObject iconAncasRana;
    public GameObject iconPlumaFenix;
    public GameObject iconPiedrasPreciosas;

    [Header("Audio")]
    public AudioSource audioSource; // asigna un AudioSource en el Inspector
    public AudioClip addIngredientSFX;
    public AudioClip potionCreatedSFX;

    public void AddIngredient(Ingredient ingredient)
    {
        if (currentIngredients.Count >= maxIngredients)
        {
            Debug.Log("Caldero lleno");
            return;
        }
        Debug.Log("Reproduciendo sonido de ingrediente");
        PlaySFX(audioSource, addIngredientSFX);
        currentIngredients.Add(ingredient.ingredientType);
        currentIngredients.Add(ingredient.ingredientType);
        Debug.Log("Añadido: " + ingredient.ingredientType);

        // Instanciar icono en la pizarra
        SpawnBoardIcon(ingredient.ingredientType);
        Debug.Log("Añadido: " + ingredient.ingredientType);

       

        // Avisar al respawn
        IngredientRespawn respawn = ingredient.GetComponent<IngredientRespawn>();
        if (respawn != null)
        {
            respawn.canRespawn = true;
            respawn.TriggerRespawn();
        }

        // Ocultar el objeto (NO destruir)
        Renderer r = ingredient.GetComponent<Renderer>();
        Collider c = ingredient.GetComponent<Collider>();
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();

        if (r) r.enabled = false;
        if (c) c.enabled = false;
        if (rb)
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
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

    void OnMouseDown()
    {
        if (currentIngredients.Count == 0) return;
        StartCoroutine(BrewPotion());
    }

    IEnumerator BrewPotion()
    {
        Vector3 originalPos = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            transform.position = originalPos + Random.insideUnitSphere * 0.05f;
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;

        PotionResultType result = PotionResolver.Resolve(currentIngredients);

        if (result == PotionResultType.None)
        {
            Debug.Log("La mezcla falló");
            CamShake.Instance.TriggerShake(0.6f, 0.3f);
        }
        else
        {
            PlaySFX(audioSource, potionCreatedSFX);
            Debug.Log("Poción creada: " + result);
            SpawnResult(result);
            PotionBook.Instance.DiscoverPotion(result);
            Debug.Log("Resultado de poción: " + result);
           
        }

        currentIngredients.Clear();
    }

    void SpawnResult(PotionResultType result)
    {
        switch (result)
        {
            case PotionResultType.Azul:
                Instantiate(potionAzulPrefab, azulSpawn.position, azulSpawn.rotation);
                break;

            case PotionResultType.Rojo:
                Instantiate(potionRojoPrefab, rojoSpawn.position, rojoSpawn.rotation);
                break;

            case PotionResultType.Verde:
                Instantiate(potionVerdePrefab, verdeSpawn.position, verdeSpawn.rotation);
                break;

            case PotionResultType.Agrandar:
                Instantiate(potionAgrandarPrefab, agrandarSpawn.position, agrandarSpawn.rotation);
                break;

            case PotionResultType.Encoger:
                Instantiate(potionEncogerPrefab, encogerSpawn.position, encogerSpawn.rotation);
                break;

            case PotionResultType.Invisibilidad:
                Instantiate(potionInvisibilidadPrefab, invisibilidadSpawn.position, invisibilidadSpawn.rotation);
                break;

            case PotionResultType.Flotar:
                Instantiate(potionFlotarPrefab, flotarSpawn.position, flotarSpawn.rotation);
                break;

            case PotionResultType.Rana:
                Instantiate(potionRanaPrefab, ranaSpawn.position, ranaSpawn.rotation);
                break;

            case PotionResultType.Fuego:
                Instantiate(potionFuegoPrefab, fuegoSpawn.position, fuegoSpawn.rotation);
                break;

            case PotionResultType.FuegoColores:
                Instantiate(potionFuegoColoresPrefab, fuegoColoresSpawn.position, fuegoColoresSpawn.rotation);
                break;

            case PotionResultType.Brillar:
                Instantiate(potionBrillarPrefab, brillarSpawn.position, brillarSpawn.rotation);
                break;

            case PotionResultType.BrillarColores:
                Instantiate(potionBrillarColoresPrefab, brillarColoresSpawn.position, brillarColoresSpawn.rotation);
                break;
        }
    }

    private void SpawnBoardIcon(IngredientType type)
    {
        if (boardPoints == null || boardPoints.Length == 0) return;

        // Contamos cuántos iconos ya hay en la pizarra
        int index = Mathf.Min(currentIngredients.Count - 1, boardPoints.Length - 1);

        Transform point = boardPoints[index];

        GameObject prefab = null;

        switch (type)
        {
            case IngredientType.FlorAzul: prefab = iconFlorAzul; break;
            case IngredientType.FlorRoja: prefab = iconFlorRoja; break;
            case IngredientType.FlorVerde: prefab = iconFlorVerde; break;
            case IngredientType.Setas: prefab = iconSetas; break;
            case IngredientType.PolvosMagicos: prefab = iconPolvosMagicos; break;
            case IngredientType.SustanciaMaligna: prefab = iconSustanciaMaligna; break;
            case IngredientType.EscamasCamaleon: prefab = iconEscamasCamaleon; break;
            case IngredientType.AncasRana: prefab = iconAncasRana; break;
            case IngredientType.PlumaFenix: prefab = iconPlumaFenix; break;
            case IngredientType.PiedrasPreciosas: prefab = iconPiedrasPreciosas; break;
        }

        if (prefab != null && point != null)
        {
            GameObject icon = Instantiate(prefab, point.position, point.rotation);
            icon.transform.SetParent(point); // opcional, para mantenerlo organizado
            icon.transform.localScale = Vector3.one * 0.5f; // ajusta tamaño si hace falta
        }
    }
}