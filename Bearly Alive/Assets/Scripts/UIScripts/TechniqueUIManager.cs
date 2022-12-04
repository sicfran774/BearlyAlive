using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechniqueUIManager : MonoBehaviour
{
    public static TechniqueUIManager instance = null;

    public GameObject WeaponSwapMenu;

    // Picked UP Technique
    [Header("Picked Up Technique")]
    public Text techniqueDescription;
    public Text techniqueName;
    public Image techniqueImage;


    [Header("Equipped Techniques")]
    //Ugrade Menu UI labels 
    public Text firstTechniqueLabel;
    public Text secondTechniqueLabel;

    public Text firstTechniqueDescriptionLabel;
    public Text secondTechniqueDescriptionLabel;

    public Image upgradeImageFirstSlot;
    public Image upgradeImageSecondSlot;

    public Text appliedUpgradeSlot1Text;
    public Text appliedUpgradeSlot2Text;


    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }



    //Allows to call events in scripts, used for upgrade menu 
    public delegate void WeaponSwapCallback(bool active);

    //An instance for call backs  
    public WeaponSwapCallback onToggleWeaponMenu;

    public void ToggleWeaponMenu()
    {
        //Inverse current active state
        WeaponSwapMenu.SetActive(!WeaponSwapMenu.activeSelf);

        //If upgrade menu is active, pass argument in delegate 
        onToggleWeaponMenu.Invoke(WeaponSwapMenu.activeSelf);

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

            if(UpgradeUIManager.instance != null)
            {
                upgradeImageFirstSlot.sprite = UpgradeUIManager.instance.getSprite(0);

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

            if(UpgradeUIManager.instance != null)
            {
                upgradeImageSecondSlot.sprite = UpgradeUIManager.instance.getSprite(1);

            }

            appliedUpgradeSlot2Text.text = "Current Upgrade: " + PlayerController.instance.techniques[1].upgrade.Replace("Upgrade", "");

        }
    }
}
 
