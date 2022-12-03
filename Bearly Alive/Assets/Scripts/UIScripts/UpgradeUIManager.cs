using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    //upgrade menu object
    public GameObject upgradeMenu;
    private bool[] isUpgraded = new bool[2];

    public static UpgradeUIManager instance = null;
  

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //Apply upgrade based on what technique player chooses 
    public void setUpgradeinSlot1()
    {   
        //First Slot is selected to be upgraded 
        isUpgraded[0] = true;
        setUpgradeinSlot(1);
    }

    public void setUpgradeinSlot2()
    {
        //Second Slot is selected to be upgraded 
        isUpgraded[1] = true;
        setUpgradeinSlot(2);
    }


    // to be called by setUpgradeinSlot2() and ...1()
    public void setUpgradeinSlot(int slot)
    {
        //choose correct upgrade tag to pass to setUpgrade(int string)
        string pickupName = PlayerController.instance.pickedUpgrade.name;
        string selectedUpgrade = "";
      
        switch (pickupName)
        {
            case "Tajin Rubdown":
                selectedUpgrade = "UpgradeSpicy";
                break;
            case "Jello Infusion":
                selectedUpgrade = "UpgradeJello";
                break;
            case "Malic Acid Dip":
                selectedUpgrade = "UpgradeSour";
                break;
            case "Pop Rocks":
                selectedUpgrade = "UpgradeKnockback";
                break;
            case "Rock Candy":
                selectedUpgrade = "UpgradeRock";
                break;
            default:
                break;
        }

        PlayerController.instance.setUpgrade(slot, selectedUpgrade);

        print(selectedUpgrade + " upgrade applied to slot " + slot);

        //Display upgrade image in UI first slot 
        if (slot == 1)
        {

            UpgradeUI.instance.applyUpgradeText(1, pickupName);

        }

        //Display upgrade image in UI second slot 
        if (slot == 2)
        {
            UpgradeUI.instance.applyUpgradeText(2, pickupName);
        }

        //Disable upgrade menu when player selects technique to upgrade
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);

        //If upgrade menu is active, pass argument in delegate 
        UpgradeUI.instance.onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);

        //Destroy upgrade
        Destroy(PlayerController.instance.pickedUpgrade);
    }
}
