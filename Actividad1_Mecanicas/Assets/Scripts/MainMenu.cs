using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject optionsPanel;

    // Start is called before the first frame update
    void Start()
    {
        creditsPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }

   public void Jugar()
    {
        SceneManager.LoadScene("Game");
    }

    public void Creditos()
    {
        creditsPanel.SetActive(true);
    }

    public void Options()
    {
        optionsPanel.SetActive(true);
    }

    public void Cerrar()
    {
        Application.Quit();
    }

    public void Return()
    {
        creditsPanel.SetActive(false);
        optionsPanel.SetActive(false);
    }
}
