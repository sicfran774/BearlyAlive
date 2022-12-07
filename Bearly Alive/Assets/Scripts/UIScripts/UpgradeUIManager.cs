using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    //upgrade menu object
    public GameObject upgradeMenu;
    private bool[] isUpgraded = new bool[2];
    public static Sprite[] upgradeSprites = new Sprite[2];

    public Sprite emptySprite;


    public static UpgradeUIManager instance = null;
  

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        print(upgradeSprites[0]);
    }


    public Sprite getSprite(int slot)
    {
        if(upgradeSprites[slot] == null)
        {

            return emptySprite;
        }
        return upgradeSprites[slot];
    }

    //Apply upgrade based on what technique player chooses 
    public void setUpgradeinSlot1()
    {
        //First Slot is selected to be upgraded 
        isUpgraded[0] = true;
        upgradeSprites[0] = UpgradeUI.instance.upgradeImage.sprite;
        setUpgradeinSlot(1);
    }

    public void setUpgradeinSlot2()
    {
        //Second Slot is selected to be upgraded 
        isUpgraded[1] = true;
        upgradeSprites[1] = UpgradeUI.instance.upgradeImage.sprite;
        setUpgradeinSlot(2);
    }

    public void removeUpgrade(int slot)
    {
        if(upgradeSprites[slot - 1] != null)
        {
            upgradeSprites[slot - 1] = null;
            if (slot == 1)
            {
                UpgradeUI.instance.upgradeImageFirstSlot.sprite = null;

            }
            else
            {
                UpgradeUI.instance.upgradeImageSecondSlot.sprite = null;

            }

        }
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
            case "Tajin Rubdown(Clone)":
                selectedUpgrade = "UpgradeSpicy";
                break;
            case "Jello Infusion":
            case "Jello Infusion(Clone)":
                selectedUpgrade = "UpgradeJello";
                break;
            case "Malic Acid Dip":
            case "Malic Acid Dip(Clone)":
                selectedUpgrade = "UpgradeSour";
                break;
            case "Pop Rocks":
            case "Pop Rocks(Clone)":
                selectedUpgrade = "UpgradeKnockback";
                break;
            case "Rock Candy":
            case "Rock Candy(Clone)":
                selectedUpgrade = "UpgradeRock";
                break;
            default:
                break;
        }

        print("SelectedUpgrade" + selectedUpgrade);
        if(!PlayerController.instance.setUpgrade(slot, selectedUpgrade)) //If the technique is null go into if
        {
            return;
        }

        print(selectedUpgrade + " upgrade applied to slot " + slot);

        //Display upgrade image in UI first slot 
        if (isUpgraded[0] == true)
        {
            Sprite getSprite = upgradeSprites[0];
            print(getSprite);
            UpgradeUI.instance.upgradeImageFirstSlot.sprite = getSprite;

        }

        //Display upgrade image in UI second slot 
        if (isUpgraded[1] == true)
        {
            Sprite getSprite = upgradeSprites[1];
            print(getSprite);
            UpgradeUI.instance.upgradeImageSecondSlot.sprite = getSprite;
        }

        //Disable upgrade menu when player selects technique to upgrade
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);

        //If upgrade menu is active, pass argument in delegate 
        UpgradeUI.instance.onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);

        //Destroy upgrade
        Destroy(PlayerController.instance.pickedUpgrade);
    }
}
