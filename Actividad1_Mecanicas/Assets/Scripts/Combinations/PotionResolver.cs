using System.Collections.Generic;
using UnityEngine;

public class PotionResolver : MonoBehaviour
{
    public static string Resolve(List<IngredientType> ingredients)
    {
        ingredients.Sort();

        if (ingredients.Contains(IngredientType.FlorAzul) &&
            ingredients.Contains(IngredientType.PolvosMagicos))
            return "Volver de color azul";

        if (ingredients.Contains(IngredientType.Setas) &&
            ingredients.Contains(IngredientType.SustanciaMaligna))
            return "Encoger";

        if (ingredients.Contains(IngredientType.PlumaFenix) &&
            ingredients.Contains(IngredientType.PolvosMagicos))
            return "Efecto de fuego";

        return "💥 Explosión";
    }
}
