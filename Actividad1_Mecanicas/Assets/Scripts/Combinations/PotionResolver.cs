using System.Collections.Generic;
using UnityEngine;

public class PotionResolver : MonoBehaviour
{
    public static PotionResultType Resolve(List<IngredientType> ingredients)
    {
        ingredients.Sort();

        // COLORES
        if (ingredients.Contains(IngredientType.FlorAzul) &&
            ingredients.Contains(IngredientType.PolvosMagicos))
            return PotionResultType.Azul;

        if (ingredients.Contains(IngredientType.FlorRoja) &&
            ingredients.Contains(IngredientType.PolvosMagicos))
            return PotionResultType.Rojo;

        if (ingredients.Contains(IngredientType.FlorVerde) &&
            ingredients.Contains(IngredientType.PolvosMagicos))
            return PotionResultType.Verde;

        // TAMAÑO
        if (ingredients.Contains(IngredientType.Setas) &&
            ingredients.Contains(IngredientType.PolvosMagicos))
            return PotionResultType.Agrandar;

        if (ingredients.Contains(IngredientType.Setas) &&
            ingredients.Contains(IngredientType.SustanciaMaligna))
            return PotionResultType.Encoger;

        // EFECTOS
        if (ingredients.Contains(IngredientType.EscamasCamaleon) &&
            ingredients.Contains(IngredientType.PolvosMagicos))
            return PotionResultType.Invisibilidad;

        if (ingredients.Contains(IngredientType.AncasRana) &&
            ingredients.Contains(IngredientType.PolvosMagicos))
            return PotionResultType.Flotar;

        if (ingredients.Contains(IngredientType.AncasRana) &&
            ingredients.Contains(IngredientType.SustanciaMaligna))
            return PotionResultType.Rana;

        // FUEGO
        if (ingredients.Contains(IngredientType.PlumaFenix) &&
            ingredients.Contains(IngredientType.PolvosMagicos) &&
            ingredients.Contains(IngredientType.PiedrasPreciosas))
            return PotionResultType.FuegoColores;

        if (ingredients.Contains(IngredientType.PlumaFenix) &&
            ingredients.Contains(IngredientType.PolvosMagicos))
            return PotionResultType.Fuego;

        // BRILLO
        if (ingredients.Contains(IngredientType.PiedrasPreciosas) &&
            ingredients.Contains(IngredientType.FlorAzul) ||
            ingredients.Contains(IngredientType.FlorRoja) ||
            ingredients.Contains(IngredientType.FlorVerde))
            return PotionResultType.BrillarColores;

        if (ingredients.Contains(IngredientType.PiedrasPreciosas) &&
            ingredients.Contains(IngredientType.PolvosMagicos))
            return PotionResultType.Brillar;

        // 💥 EXPLOSIÓN
        if (ingredients.Contains(IngredientType.SustanciaMaligna))
            return PotionResultType.None;

        return PotionResultType.None;
    }
}