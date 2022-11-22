using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUIManager : MonoBehaviour
{
    // Allow user to try agian 
    public void RestartGame()
    {
        GameManager.instance.TryAgain();
    }
    
    //Reset the game
    public void MainMenu()
    {
       GameManager.instance.Reset();
    }
}
