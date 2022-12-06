/***************************************************************
*File: GameManager.cs
*Author: Radical Cadavical
*Class: CS 4700 � Game Development
*Assignment: Program 4
*Date last modified: 12/5/2022
*
*Purpose: This program stores the technique's weapon objects.The 
*program handles Scene transitions, Starting Screen to starting 
*the game at Level 1, displaying Game Over, and Finished Screen.
*Handles Technique and upgrade menu toggles.
****************************************************************/
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

    public GameObject ChiSpitProjectile;

    public GameObject Sword;

    public GameObject Whip;

    public GameObject Boomerang;

  

  
    void Update()
    {
        //User can pick up upgrade, which toggles upgrade menu 
        if (PlayerController.instance.canpickupUpgrade == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //play audio clip
                SoundManager.instance.playPickedUpgradeSound();

                UpgradeUI.instance.ToggleUpgradeMenu();
                print("Toggle Upgrade menu");
            }

       

        }

        if (PlayerController.instance.canpickupTechnique)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //play audio sound
                SoundManager.instance.playPickedTechniqueSound();

                TechniqueUIManager.instance.ToggleWeaponMenu();
                print("Toggle Weapon menu");
            }
        }

        
    }
 


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


    //function: DecreaseAmmo
    //purpose: Decrements Chispit's ammo
    public void DecreaseAmmo(int amount)
    {
        bullets -= amount;
    }

    //function: SetAmmo
    //purpose: Sets the weapon's ammo back to max
    //capacity Used in Reload function
    public void SetAmmo()
    {
        bullets = _MAX_AMMO;
    }

    //function: IncreaseScore
    //purpose: Increases the score
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

    //function Reset()
    //purpose: Restarts game and refresh score and reset back to
    //level 1
    public void Reset()
    {
        //Reset score
        score = 0;

        //reset the high score 
        highScore = 0;

        //reset the bullets
        bullets = 15;

        //Set the current level to 1 
        currentLevel = 1;

        //Load scence 
        SceneManager.LoadScene("Home");
    }


    //function: TryAgain
    //purpose: Allow the player to try again, this keeps the high score
    //and curr level and rests the score 
    public void TryAgain()
    {
        //reset score 
        score = 0;

        //Load scence 
        SceneManager.LoadScene("Level" + currentLevel);
    }


    //function: RestartGame
    //purpose: Triggers to display Game Over
    //screen when player dies
    public void RestartGame()
    {
        SceneManager.LoadScene("GameOver");
    }


    //function: IncreaseLevel
    //purpose: Loads next level in scenemanagement
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
