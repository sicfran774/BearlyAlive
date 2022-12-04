using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwap : MonoBehaviour
{
    //upgrade menu object
    public GameObject upgradeMenu;
    public bool[] isSwapped = new bool[2];

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
        string pickupName = PlayerController.instance.pickedUpgrade.name;
        string selectedUpgrade = "";

        switch(pickupName) {
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
      /*  if (isSwapped[0] == true)
        {
            Sprite getSprite = SpriteUI.instance.sprite;
            UpgradeUI.instance.upgradeImageFirstSlot = GetComponent<Image>();
            print(getSprite);
            UpgradeUI.instance.upgradeImageFirstSlot.sprite = getSprite;

        }

        //Display upgrade image in UI second slot 
        if (isSwapped[1] == true)
        {
            Sprite getSprite = SpriteUI.instance.sprite;

            UpgradeUI.instance.upgradeImageSecondSlot = GetComponent<Image>();
            UpgradeUI.instance.upgradeImageSecondSlot.sprite = getSprite;
        }
*/
        //Disable upgrade menu when player selects technique to upgrade
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);

        //If upgrade menu is active, pass argument in delegate 
        UpgradeUI.instance.onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);

        //Destroy upgrade
        Destroy(PlayerController.instance.pickedUpgrade);
    }
}
