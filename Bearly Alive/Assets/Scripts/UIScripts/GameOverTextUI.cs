using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverTextUI : MonoBehaviour
{
    // Where the score value will be shown
    public Text score;

    // Where the high score value will be shown
    public Text highScore;

    private void Start()
    {
        DisplayResults();
    }

    // Run on the first frame
    private void DisplayResults()
    {
        // Show the score and high score
        score.text = "Score: " + GameManager.instance.score;
        highScore.text = "High Score: " + GameManager.instance.highScore;
    }
}
