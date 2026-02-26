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

    public void AddIngredient(Ingredient ingredient)
    {
        if (currentIngredients.Count >= maxIngredients)
        {
            Debug.Log("Caldero lleno");
            return;
        }

        currentIngredients.Add(ingredient.ingredientType);
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
            Debug.Log("Poción creada: " + result);
            SpawnResult(result);
            PotionBook.Instance.DiscoverPotion(result);
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
}