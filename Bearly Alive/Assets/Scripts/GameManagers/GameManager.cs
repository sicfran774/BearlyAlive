using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    //upgrade menu object
    public GameObject upgradeMenu;

    public GameObject ChiSpitProjectile;

    public GameObject Sword;

    public GameObject Whip;

    public GameObject Boomerang;

    //Allows to call events in scripts, used for upgrade menu 
    public delegate void UpgradeMenuCallback(bool active);

    //An instance 
    public UpgradeMenuCallback onToggleUpgradeMenu;

    //Ugrade Menu UI labels 
    public Text firstTechniqueLabel;
    public Text secondTechniqueLabel;

    public Text firstTechniqueDescriptionLabel;
    public Text secondTechniqueDescriptionLabel;

    public Text upgradeLabel;
    public Text upgradeDescriptionLabel;


    void Update()
    {
        //User can pick up upgrade, which toggles upgrade menu 
        if (PlayerController.instance.canpickup == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameManager.instance.ToggleUpgradeMenu();
                print("Toggle menu");
            }
        }
    }

    public void ToggleUpgradeMenu()
    {
        
        //Inverse current active state
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);

        //If upgrade menu is active, pass argument in delegate 
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);

        //First Technique label is updated to upgrade menu 
        if(PlayerController.instance.techniques[0] != null)
        {
            //Use regular expressions to prase first technique
            string first_technique = PlayerController.instance.techniques[0].ToString();
            string first_technique_re = first_technique.Split('(', ')')[1];

            firstTechniqueLabel.text = first_technique_re;
            switch (first_technique_re)
            {
                case "Boomerang":
                    firstTechniqueDescriptionLabel.text = "Technique can be thrown at enemies far away\n"
                        + "Power: ?\n" + "Range: ? \n" + "Speed: ?\n" + "Ammo: ?";
                    break;
                case "ChiSpit":
                    firstTechniqueDescriptionLabel.text = "Rapid fire projectiles channeled by the universe’s candy energy\n"
                        + "Power: 1\n" + "Range: Short\n" + "Speed: Fast\n" + "Ammo: 15";
                    break;
                case "Slash":
                    firstTechniqueDescriptionLabel.text = "Blunt weapon effective at crushing candy\n"
                        + "Power: 10\n" + "Range: Short\n" + "Speed: Medium\n" + "Ammo: Unlimited";
                    break;
                case "Slingshot":
                    firstTechniqueDescriptionLabel.text = "Projectiles that damage enemies in the way\n"
                        + "Power: 5\n" + "Range: Far\n" + "Speed: Medium\n" + "Ammo: Unlimited";
                    break;
                case "Whip":
                    firstTechniqueDescriptionLabel.text = "Whip through multiple enemies\n"
                        + "Power: 10\n" + "Range: Medium\n" + "Speed: Slow\n" + "Ammo: Unlimited";
                    break;
            }
        }

        //Second Technique label is updated to upgrade menu 
        if (PlayerController.instance.techniques[0] != null)
        {
            //Use regular expressions to prase second technique
            string second_technique = PlayerController.instance.techniques[1].ToString();
            string second_technique_re = second_technique.Split('(', ')')[1];

            secondTechniqueLabel.text = second_technique_re;
            switch (second_technique_re)
            {
                case "Boomerang":
                    secondTechniqueDescriptionLabel.text = "Technique can be thrown at enemies far away\n"
                        + "Power: ?\n" + "Range: ? \n" + "Speed: ?\n" + "Ammo: ?";
                    break;
                case "ChiSpit":
                    secondTechniqueDescriptionLabel.text = "Rapid fire projectiles channeled by the universe’s candy energy\n"
                        + "Power: 1\n" + "Range: Short\n" + "Speed: Fast\n" + "Ammo: 15";
                    break;
                case "Slash":
                    secondTechniqueDescriptionLabel.text = "Blunt weapon effective at crushing candy\n"
                        + "Power: 10\n" + "Range: Short\n" + "Speed: Medium\n" + "Ammo: Unlimited";
                    break;
                case "Slingshot":
                    secondTechniqueDescriptionLabel.text = "Projectiles that damage enemies in the way\n"
                        + "Power: 5\n" + "Range: Far\n" + "Speed: Medium\n" + "Ammo: Unlimited";
                    break;
                case "Whip":
                    secondTechniqueDescriptionLabel.text = "Whip through multiple enemies\n"
                        + "Power: 10\n" + "Range: Medium\n" + "Speed: Slow\n" + "Ammo: Unlimited";
                    break;
            }
        }

        //Add upgrade and description to the UI 
        upgradeLabel.text = PlayerController.instance.pickedUpgrade.name;
        print(PlayerController.instance.pickedUpgrade.name);

        switch (PlayerController.instance.pickedUpgrade.name)
        {
            case "Tajin Rubdown":
                upgradeDescriptionLabel.text = "Upgrade will deal Fire Damage";
                break;
            case "Jelly Infusion":
                upgradeDescriptionLabel.text = "Upgrade will reflect projectiles";
                break;
            case "Malic Acid Dip":
                upgradeDescriptionLabel.text = "Upgrade will deal Poison Damage";
                break;
            case "Pop Rocks":
                upgradeDescriptionLabel.text = "Upgrade will deal Explosive Damage";
                break;
            case "Rock Candy":
                upgradeDescriptionLabel.text = "Upgrade will deal Piercing Damage";
                break;
        }

    }

    private void Awake()
    {
        //Disable upgrade menu when game first starts 
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);

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
