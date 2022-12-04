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

    public Image upgradeImage;
    Sprite upgradeSprite;

    public Text appliedUpgradeSlot1Text;
    public Text appliedUpgradeSlot2Text;

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
                    firstTechniqueDescriptionLabel.text = "Toss a slow, wide deadly projectile but leaves you defenseless\n"
                        + "Cooldown: " + Boomerang.defaultCooldown + "\n" + "Damage: " + Boomerang.defaultDamage;
                    break;
                case "ChiSpit":
                    firstTechniqueDescriptionLabel.text = "Rapid fire projectiles channeled by the universe’s candy energy\n"
                        + "Cooldown: " + ChiSpit.defaultCooldown + "\n" + "Damage: " + ChiSpit.defaultDamage;
                    break;
                case "Slash":
                    firstTechniqueDescriptionLabel.text = "Perform a deadly circular slash, dealing AOE Damage\n"
                    + "Cooldown: " + Slash.defaultCooldown + "\n" + "Damage: " + Slash.defaultDamage;
                    break;
                case "Slingshot":
                    firstTechniqueDescriptionLabel.text = "Dash Toward a Direction, damaging Enemies on the way\n"
                    + "Cooldown: " + Slingshot.defaultCooldown + "\n" + "Damage: " + Slingshot.defaultDamage;
                    break;
                case "Whip":
                    firstTechniqueDescriptionLabel.text = "Precise Attack with High Damage, but only hits 1 enemy\n"
                    + "Cooldown: " + Whip.defaultCooldown + "\n" + "Damage: " + Whip.defaultDamage;
                    break;
            }

            appliedUpgradeSlot1Text.text = "Current Upgrade: " + PlayerController.instance.techniques[0].upgrade.Replace("Upgrade", "");


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
                    secondTechniqueDescriptionLabel.text = "Toss a slow, wide deadly projectile but leaves you defenseless\n"
                        + "Cooldown: " + Boomerang.defaultCooldown + "\n" + "Damage: " + Boomerang.defaultDamage;
                    break;
                case "ChiSpit":
                    secondTechniqueDescriptionLabel.text = "Rapid fire projectiles channeled by the universe’s candy energy\n"
                        + "Cooldown: " + ChiSpit.defaultCooldown + "\n" + "Damage: " + ChiSpit.defaultDamage;
                    break;
                case "Slash":
                    secondTechniqueDescriptionLabel.text = "Perform a deadly circular slash, dealing AOE Damage\n"
                    + "Cooldown: " + Slash.defaultCooldown + "\n" + "Damage: " + Slash.defaultDamage;
                    break;
                case "Slingshot":
                    secondTechniqueDescriptionLabel.text = "Dash Toward a Direction, damaging Enemies on the way\n"
                    + "Cooldown: " + Slingshot.defaultCooldown + "\n" + "Damage: " + Slingshot.defaultDamage;
                    break;
                case "Whip":
                    secondTechniqueDescriptionLabel.text = "Precise Attack with High Damage, but only hits 1 enemy\n"
                    + "Cooldown: " + Whip.defaultCooldown + "\n" + "Damage: " + Whip.defaultDamage;
                    break;
            }


            appliedUpgradeSlot2Text.text = "Current Upgrade: " + PlayerController.instance.techniques[1].upgrade.Replace("Upgrade", "");

        }

        //Add upgrade and description to the UI 
        upgradeLabel.text = PlayerController.instance.pickedUpgrade.name;
        //Add sprite of upgrade to UI
        upgradeSprite = PlayerController.instance.pickedUpgrade.GetComponent<SpriteRenderer>().sprite;
        upgradeImage.GetComponent<UnityEngine.UI.Image>().sprite = upgradeSprite;

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

    public void applyUpgradeText(int slot, string selectedUpgrade)
    {


        //Display upgrade image in UI first slot 
        if (slot == 1)
        {

            appliedUpgradeSlot1Text.text = selectedUpgrade;

        }

        //Display upgrade image in UI second slot 
        if (slot == 2)
        {

            appliedUpgradeSlot2Text.text = selectedUpgrade;
        }
    }
}
