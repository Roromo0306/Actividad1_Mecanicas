using UnityEngine;

public class CauldronTrigger : MonoBehaviour
{
    private Cauldron cauldron;

    void Start()
    {
        cauldron = GetComponentInParent<Cauldron>();
    }

    void OnTriggerEnter(Collider other)
    {
        Ingredient ingredient = other.GetComponent<Ingredient>();

        if (ingredient != null)
        {
            cauldron.AddIngredient(ingredient);
        }
    }
}
