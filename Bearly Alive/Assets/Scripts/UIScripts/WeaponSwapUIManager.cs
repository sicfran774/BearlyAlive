using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwapUIManager : MonoBehaviour
{
    //upgrade menu object
    public GameObject WeaponMenu;
    private static bool[] isSwapped = new bool[2];
    //private Sprite[] techniqueSprites = new Sprite[2];

    public void setWeaponSlot1()
    {
        //First Slot is selected to be swapped 
        isSwapped[0] = true;
        setWeaponSlot(1);
    }

    public void setWeaponSlot2()
    {
        //Second Slot is selected to be swapped 
        isSwapped[1] = true;
        setWeaponSlot(2);
    }

    public void setWeaponSlot(int slot)
    {
        //choose correct upgrade tag to pass to setUpgrade(int string)
        string pickupName = PlayerController.instance.pickedTechnique.name;
        string selectedUpgrade = "";

        switch(pickupName) {
            case "Boomerang":
            case "Boomerang(Clone)":
                selectedUpgrade = "UpgradeSpicy";
                PlayerController.instance.LearnTechnique<Boomerang>(slot);
                break;
            case "Slash":
            case "Slash(Clone)":
                selectedUpgrade = "UpgradeJello";
                PlayerController.instance.LearnTechnique<Slash>(slot);
                break;
            case "ChiSpit":
            case "ChiSpit(Clone)":
                selectedUpgrade = "UpgradeSour";
                PlayerController.instance.LearnTechnique<ChiSpit>(slot);
                break;
            case "Whip":
            case "Whip(Clone)":
                selectedUpgrade = "UpgradeKnockback";
                PlayerController.instance.LearnTechnique<Whip>(slot);
                break;
            case "SlingShot":
            case "SlingShot(Clone)":
                selectedUpgrade = "UpgradeRock";
                PlayerController.instance.LearnTechnique<Slingshot>(slot);
                break;
            default:
                break;
        }



        print(selectedUpgrade + " upgrade applied to slot " + slot);

        //Display upgrade image in UI first slot 
        if (isSwapped[0] == true && UpgradeUIManager.instance != null)
        {
            UpgradeUIManager.instance.removeUpgrade(slot);
            //Sprite getSprite = techniqueSprites[0];
            //UpgradeUI.instance.upgradeImageFirstSlot.sprite = getSprite;

        }

        //Display upgrade image in UI second slot 
        if (isSwapped[1] == true && UpgradeUIManager.instance != null)
        {
            UpgradeUIManager.instance.removeUpgrade(slot);

            //Sprite getSprite = techniqueSprites[1];
            //UpgradeUI.instance.upgradeImageSecondSlot.sprite = getSprite;
        }

        //Disable upgrade menu when player selects technique to upgrade
        WeaponMenu.SetActive(!WeaponMenu.activeSelf);

        //If upgrade menu is active, pass argument in delegate 
        TechniqueUIManager.instance.onToggleWeaponMenu.Invoke(WeaponMenu.activeSelf);

        //Destroy upgrade
        Destroy(PlayerController.instance.pickedTechnique);
    }
}
