using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
/*
        string selectedWeapon = PlayerController.instance.pickedUpgrade.name;
        PlayerController.instance.setUpgrade(slot, selectedUpgrade);

        print(selectedUpgrade + " upgrade applied to slot " + slot);

        //Display upgrade image in UI first slot 
        if (isUpgraded[0] == true)
        {
            Sprite getSprite = SpriteUI.instance.sprite;
            GameManager.instance.upgradeImageFirstSlot = GetComponent<Image>();
            print(getSprite);
            GameManager.instance.upgradeImageFirstSlot.sprite = getSprite;

        }

        //Display upgrade image in UI second slot 
        if (isUpgraded[1] == true)
        {
            Sprite getSprite = SpriteUI.instance.sprite;

            GameManager.instance.upgradeImageSecondSlot = GetComponent<Image>();
            GameManager.instance.upgradeImageSecondSlot.sprite = getSprite;
        }

        //Disable upgrade menu when player selects technique to upgrade
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);

        //If upgrade menu is active, pass argument in delegate 
        GameManager.instance.onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);

        //Destroy upgrade
        Destroy(PlayerController.instance.pickedUpgrade);*/
    }
}
