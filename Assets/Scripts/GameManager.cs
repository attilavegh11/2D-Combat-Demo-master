using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject victoryScreen;
    public GameObject defeatScreen;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnGameWon()
    {
        Time.timeScale = 0;
        victoryScreen.SetActive(true);
    }

    public void OnGameLost()
    {
        Time.timeScale = 0;
        defeatScreen.SetActive(true);
    }

    public void OnPaused()
    {
        Time.timeScale = 0;
    }

    public void OnRetryPressed()
    {
        SceneManager.LoadScene("Battle");
    }

    public void OnMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
