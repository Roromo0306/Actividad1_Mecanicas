using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public List<IngredientType> currentIngredients = new List<IngredientType>();
    public int maxIngredients = 3;

    [Header("Potion Result Spawns")]
    public Transform azulSpawnPoint;
    public Transform encogerSpawnPoint;
    public Transform fuegoSpawnPoint;

    public GameObject potionAzulPrefab;
    public GameObject potionEncogerPrefab;
    public GameObject potionFuegoPrefab;

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
                if (potionAzulPrefab && azulSpawnPoint)
                    Instantiate(potionAzulPrefab, azulSpawnPoint.position, azulSpawnPoint.rotation);
                break;

            case PotionResultType.Encoger:
                if (potionEncogerPrefab && encogerSpawnPoint)
                    Instantiate(potionEncogerPrefab, encogerSpawnPoint.position, encogerSpawnPoint.rotation);
                break;

            case PotionResultType.Fuego:
                if (potionFuegoPrefab && fuegoSpawnPoint)
                    Instantiate(potionFuegoPrefab, fuegoSpawnPoint.position, fuegoSpawnPoint.rotation);
                break;
        }
    }
}