using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager2 : MonoBehaviour
{

    //upgrade menu object
    public GameObject upgradeMenu;
    public bool isFirstUpgraded;
    public bool isSecondUpgraded;

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
        string selectedUpgrade = PlayerController.instance.pickedUpgrade.name;
        //ToDo:
        isFirstUpgraded = true;

        print("Is First upgraded: " + isFirstUpgraded);

        print(selectedUpgrade + " upgrade applied to slot 1");

        PlayerController.instance.setUpgrade(1, selectedUpgrade);

        //Display upgrade image in UI first slot 
        if (isFirstUpgraded == true)
        {
            Sprite getSprite = SpriteUI.instance.sprite;
            GameManager.instance.upgradeImageFirstSlot = GetComponent<Image>();
            print(getSprite);
            GameManager.instance.upgradeImageFirstSlot.sprite = getSprite;

        }

        //Display upgrade image in UI second slot 
        if (isSecondUpgraded == true)
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

    public void setUpgradeinSlot2()
    {
        string selectedUpgrade = PlayerController.instance.pickedUpgrade.name;

        //ToDo:
        isSecondUpgraded = true;
 
        print("Is Second upgraded: " + isSecondUpgraded);

        print(selectedUpgrade + " upgrade applied to slot 2");

        PlayerController.instance.setUpgrade(2, selectedUpgrade);

        //Disable upgrade menu when player selects technique to upgrade 
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);

        //If upgrade menu is active, pass argument in delegate 
        GameManager.instance.onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);

        //Destroy upgrade
        Destroy(PlayerController.instance.pickedUpgrade);
    }



}
