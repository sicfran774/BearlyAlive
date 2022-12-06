using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameController : MonoBehaviour
{

    public GameObject pauseButton, pausePanel, exitButton;


    private void Start()
    {
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        exitButton.SetActive(false);
    }


    public void PauseGame()
    {
        //Play audio clip
        SoundManager.instance.playPauseSound();

        pauseButton.SetActive(false);
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        //Play audio clip
        SoundManager.instance.playUnpauseSound();

        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }


}
