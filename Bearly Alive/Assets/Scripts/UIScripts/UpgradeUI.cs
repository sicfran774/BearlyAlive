using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    //Instance created for class 
    public static UpgradeUI instance = null;

    //Ugrade Menu UI labels 
    public Text firstTechniqueLabel;
    public Text secondTechniqueLabel;

    public Text firstTechniqueDescriptionLabel;
    public Text secondTechniqueDescriptionLabel;

    public Text upgradeLabel;
    public Text upgradeDescriptionLabel;
    public Image upgradeImageFirstSlot;
    public Image upgradeImageSecondSlot;

    //upgrade menu object
    public GameObject upgradeMenu;

    //Allows to call events in scripts, used for upgrade menu 
    public delegate void UpgradeMenuCallback(bool active);

    //An instance for call backs  
    public UpgradeMenuCallback onToggleUpgradeMenu;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ToggleUpgradeMenu()
    {

        //Inverse current active state
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);

        //If upgrade menu is active, pass argument in delegate 
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);

        //First Technique label is updated to upgrade menu 
        if (PlayerController.instance.techniques[0] != null)
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
        if (PlayerController.instance.techniques[1] != null)
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
            case "Jello Infusion":
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
}
