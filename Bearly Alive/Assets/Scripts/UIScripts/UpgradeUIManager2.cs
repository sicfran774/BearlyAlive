using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUIManager2 : MonoBehaviour
{

    //upgrade menu object
    public GameObject upgradeMenu;

    //ADDED TO APPLY TO UPGRADE UI THAT I CREATED
    //Apply upgrade based on what technique player chooses 
    public void setUpgradeinSlot1()
    {
        string selectedUpgrade = PlayerController.instance.pickedUpgrade.name;

        print(selectedUpgrade + " upgrade applied to slot 1");

        PlayerController.instance.setUpgrade(1, selectedUpgrade);

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
