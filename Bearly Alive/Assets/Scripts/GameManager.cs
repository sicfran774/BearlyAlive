using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int bullets = 15;
    public int _MAX_AMMO = 15;

    //current score
    public int score = 0;

    //high score 
    public int highScore = 0;

    //current level, starting at level 1
    public int currentLevel = 1;

    //highest level available in game
    public int highestLevel = 2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Decrements weapon's ammo
    public void DecreaseAmmo(int amount)
    {
        bullets -= amount;
    }

    // Sets the weapon's ammo back to max capacity
    // Used in Reload function

    public void SetAmmo()
    {
        bullets = _MAX_AMMO;
    }

    //Increase the score 
    public void IncreaseScore(int amount)
    {
        //Increase the score by given amount 
        score += amount;

        //Print score to console 
        print("New Score: " + score.ToString());

        //adjust high score if current score is greater than high score
        if(score > highScore)
        {
            highScore = score;
            print("New high score: " + highScore);
        }
    }

    //Restart game and refresh score and reset back to level 1
    public void Reset()
    {
        //Reset score
        score = 0;

        //Set the current level to 1 
        currentLevel = 1;

        //Load scence 
        SceneManager.LoadScene("Level" + currentLevel);
    }

    //Go to next level 
    public void IncreaseLevel()
    {
        //Check current level, and if at last level reset to level 1 
        if(currentLevel < highestLevel)
        {
            currentLevel++;
        }
        else
        {
            currentLevel = 1;
        }

        SceneManager.LoadScene("Level" + currentLevel);
    }

}
