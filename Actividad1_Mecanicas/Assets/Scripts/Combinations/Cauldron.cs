using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public List<IngredientType> currentIngredients = new List<IngredientType>();
    public int maxIngredients = 3;

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

        Debug.Log("Poción creada con " + currentIngredients.Count + " ingredientes");
        currentIngredients.Clear();
    }
}