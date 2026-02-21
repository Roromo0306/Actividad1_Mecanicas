using System.Collections.Generic;
using UnityEngine;

public class PotionDatabase : MonoBehaviour
{
    public static PotionDatabase Instance;

    public List<PotionData> potions = new List<PotionData>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public PotionData GetPotionData(PotionResultType type)
    {
        return potions.Find(p => p.potionType == type);
    }
}