using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUIManager : MonoBehaviour
{
    // Allow user to try agian 
    public void RestartGame()
    {
        //Enable gameplay music and load level1 scene
        GameManager.instance.GetComponent<AudioSource>().Play();
        GameManager.instance.TryAgain();
    }
    
    //Reset the game
    public void MainMenu()
    {
        //Stop gameplay music
        GameManager.instance.GetComponent<AudioSource>().Stop();
        GameManager.instance.Reset();
    }
}
