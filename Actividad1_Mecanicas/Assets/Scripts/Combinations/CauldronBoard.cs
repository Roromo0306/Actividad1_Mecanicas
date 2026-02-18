using TMPro;
using UnityEngine;

public class CauldronBoard : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Cauldron cauldron;

    void Update()
    {
        text.text = "";

        foreach (var ingredient in cauldron.currentIngredients)
        {
            text.text += ingredient + "\n";
        }
    }
}
