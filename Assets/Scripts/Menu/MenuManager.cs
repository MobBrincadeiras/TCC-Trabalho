using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string SceneDoJogo;

    [SerializeField] private GameObject PlayPainel;
    [SerializeField] private GameObject MenuPainel;
    [SerializeField] private GameObject ConfigPainel;
    [SerializeField] private GameObject MascotObject;

    [SerializeField] private TMP_Text MoneyText;
    [SerializeField] private TMP_Text LevelText;
    [SerializeField] private TMP_Text ExpText;
    [SerializeField] private TMP_Text RecordText;

    private void Start()
    {
        UpdatePlayerData();
    }

    public void UpdatePlayerData()
    {

        MoneyText.text = PlayerData.Money.ToString();

        LevelText.text = "Nível " + PlayerData.Level;

        ExpText.text = PlayerData.Experience + " / " +
                       PlayerData.ExperienceToNextLevel + " XP";

        RecordText.text = "Recorde: " + PlayerData.PongRecord;
    }

    public void StartPong()
    {
        SceneManager.LoadScene(SceneDoJogo);
    }

    public void OpenConfig()
    {
        MenuPainel.SetActive(false);
        MascotObject.SetActive(false);
        ConfigPainel.SetActive(true);
    }

    public void CloseConfig()
    {
        MenuPainel.SetActive(true);
        MascotObject.SetActive(true);
        ConfigPainel.SetActive(false);
    }

    public void OpenPlay()
    {
        MenuPainel.SetActive(false);
        MascotObject.SetActive(false);
        PlayPainel.SetActive(true);
    }

    public void ClosePlay()
    {
        MenuPainel.SetActive(true);
        MascotObject.SetActive(true);
        PlayPainel.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("sair");
        Application.Quit();
    }
}