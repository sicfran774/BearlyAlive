using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{

    GameObject upgradePanel;
    Text upgradeName;
    Image upgradeImage;
    Text upgradeDescription;

    public Sprite sprite;

    Rigidbody2D body;
    Collider2D coll;


    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        upgradePanel = UpgradeUIManager.instance.UpgradeDetailsUI;
        upgradeName = UpgradeUIManager.instance.upgradeName;
        upgradeImage = UpgradeUIManager.instance.upgradeImage;
        upgradeDescription = UpgradeUIManager.instance.upgradeDescription;
        upgradePanel.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            switch (gameObject.name)
            {
                case "Tajin Rubdown":
                    upgradeDescription.text = "Technique will deal Fire Damage";
                    break;
                case "Jelly Infusion":
                    upgradeDescription.text = "Technique will reflect projectiles";
                    break;
                case "Malic Acid Dip":
                    upgradeDescription.text = "Technique will deal Poison Damage";
                    break;
                case "Pop Rocks":
                    upgradeDescription.text = "Technique will deal Explosive Damage";
                    break;
                case "Rock Candy":
                    upgradeDescription.text = "Technique will deal Piercing Damage";
                    break;
            }

            upgradeImage.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
            upgradeName.text = "[UPGRADE] " + gameObject.name;
            print("I AM " + gameObject.name);
            upgradePanel.SetActive(true);

        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            upgradePanel.SetActive(false);
        }
    }


}
