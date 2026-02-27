using System;
using System.Collections.Generic;
using UnityEngine;

public class PotionBook : MonoBehaviour
{
    public static PotionBook Instance;
    public GameObject potionBook;
    public MonoBehaviour cameraController;

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

        // Mostrar cursor y desbloquearlo
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Desactivar el script de cámara
        if (cameraController != null)
            cameraController.enabled = false;
    }

    public void ClosePotionBook()
    {
        potionBook.SetActive(false);

        // Ocultar cursor y bloquearlo
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Activar el script de cámara
        if (cameraController != null)
            cameraController.enabled = true;
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