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
            if (Input.GetKeyDown(KeyCode.F))
            {
                UpgradeUI.instance.ToggleUpgradeMenu();
                print("Toggle menu");
            }

       

        }

        if (PlayerController.instance.canpickupTechnique)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                TechniqueUIManager.instance.ToggleWeaponMenu();
                print("Toggle Weapon menu");
            }
        }

        
    }
 

    private void Awake()
    {
        //Disable upgrade menu when game first starts 
        //UpgradeUI.instance.upgradeMenu.SetActive(!UpgradeUI.instance.upgradeMenu.activeSelf);

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

        //reset the high score 
        highScore = 0;

        //reset the bullets
        bullets = 15;

        //Set the current level to 1 
        currentLevel = 1;

        //Load scence 
        SceneManager.LoadScene("Home");
    }


    //Allow the player to try again, this keeps the high score and curr level and rests the score 
    public void TryAgain()
    {
        //reset score 
        score = 0;

        //Load scence 
        SceneManager.LoadScene("Level" + currentLevel);
    }


    public void RestartGame()
    {
        SceneManager.LoadScene("GameOver");
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
