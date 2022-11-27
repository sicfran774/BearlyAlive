using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameController : MonoBehaviour
{

    public GameObject pauseButton, pausePanel;


    private void Start()
    {
        ResumeGame();
    }


    public void PauseGame()
    {
        pauseButton.SetActive(false);
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }


}
