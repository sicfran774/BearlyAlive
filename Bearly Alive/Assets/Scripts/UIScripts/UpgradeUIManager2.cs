using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager2 : MonoBehaviour
{
    //upgrade menu object
    public GameObject upgradeMenu;
    public bool[] isUpgraded = new bool[2];

    public static UpgradeUIManager2 instance = null;

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
        string selectedUpgrade = PlayerController.instance.pickedUpgrade.name;
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
        Destroy(PlayerController.instance.pickedUpgrade);
    }
}
