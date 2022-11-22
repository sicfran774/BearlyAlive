using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    // Where the score value will be shown
    public Text score;

    // Where the high score value will be shown
    public Text highScore;

    // Run on the first frame
     private void DisplayResults()
    {
        // Show the score and high score
        score.text = "Score: " + GameManager.instance.score;
        highScore.text = "High Score: " + GameManager.instance.highScore;
    }

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
