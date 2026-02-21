using System;
using System.Collections.Generic;
using UnityEngine;

public class PotionBook : MonoBehaviour
{
    public static PotionBook Instance;
    public GameObject potionBook;

    public List<PotionResultType> discoveredPotions = new List<PotionResultType>();

    
    public event Action<PotionResultType> OnPotionDiscovered;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        potionBook.SetActive(false);
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenPotionBook();

        }
    }

    public void OpenPotionBook()
    {
        potionBook.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePotionBook()
    {
        potionBook.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void DiscoverPotion(PotionResultType potion)
    {
        if (discoveredPotions.Contains(potion)) return;

        discoveredPotions.Add(potion);
        Debug.Log("Poción descubierta: " + potion);

        
        OnPotionDiscovered?.Invoke(potion);
    }

    public bool IsDiscovered(PotionResultType potion)
    {
        return discoveredPotions.Contains(potion);
    }
}