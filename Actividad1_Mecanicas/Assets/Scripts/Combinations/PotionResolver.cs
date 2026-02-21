using System.Collections.Generic;
using UnityEngine;

public class PotionResolver : MonoBehaviour
{
    public static PotionResultType Resolve(List<IngredientType> ingredients)
    {
        ingredients.Sort();

        if (ingredients.Contains(IngredientType.FlorAzul) &&
            ingredients.Contains(IngredientType.PolvosMagicos))
            return PotionResultType.Azul;

        if (ingredients.Contains(IngredientType.Setas) &&
            ingredients.Contains(IngredientType.SustanciaMaligna))
            return PotionResultType.Encoger;

        if (ingredients.Contains(IngredientType.PlumaFenix) &&
            ingredients.Contains(IngredientType.PolvosMagicos))
            return PotionResultType.Fuego;

        return PotionResultType.None; // 💥 Explosión
    }
}