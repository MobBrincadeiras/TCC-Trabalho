using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string SceneDoJogo;
    [SerializeField] private GameObject MenuPainel;
    [SerializeField] private GameObject ConfigPainel;
    public void StartPlay()
    {
        SceneManager.LoadScene(SceneDoJogo);
    }

    public void OpenConfig()
    {
        MenuPainel.SetActive(false);
        ConfigPainel.SetActive(true);
    }

    public void CloseConfig()
    {
        MenuPainel.SetActive(true);
        ConfigPainel.SetActive(false);
    }
    public void ExitGame()
    {
        Debug.Log("PINTOs");
        Application.Quit();
    }
}