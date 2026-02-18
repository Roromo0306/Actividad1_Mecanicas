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
            Debug.Log("El caldero está lleno");
            return;
        }

        currentIngredients.Add(ingredient.ingredientType);
        Debug.Log("Añadido: " + ingredient.ingredientType);

        Destroy(ingredient.gameObject);
        UpdateBoard();
    }

    void UpdateBoard()
    {
        
    }

    void OnMouseDown()
    {
        if (currentIngredients.Count == 0) return;

        StartCoroutine(BrewPotion());
    }

    IEnumerator BrewPotion()
    {
        Vector3 originalPos = transform.position;
        float timer = 0f;

        while (timer < 1f)
        {
            float x = Random.Range(-0.05f, 0.05f);
            float z = Random.Range(-0.05f, 0.05f);

            transform.position = originalPos + new Vector3(x, 0, z);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;

        ResolvePotion();
    }

    void ResolvePotion()
    {
        string result = PotionResolver.Resolve(currentIngredients);
        Debug.Log("?? Resultado: " + result);

        currentIngredients.Clear();
    }
}
