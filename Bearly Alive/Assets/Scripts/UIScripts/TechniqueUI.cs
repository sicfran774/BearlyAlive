using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TechniqueUI : MonoBehaviour
{
    GameObject techniqueDetailsPanel;
    Text techniqueDescription;
    Text techniqueName;
    Text techniqueCooldown;
    Text techniqueDamage;
    Image techniqueImage;

    public Sprite techniqueSprite;

    Rigidbody2D body;
    Collider2D coll;


    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        techniqueDetailsPanel = TechniqueUIManager.instance.TechniqueDetailsUI;
        techniqueDescription = TechniqueUIManager.instance.techniqueDescription;
        techniqueName = TechniqueUIManager.instance.techniqueName;
        techniqueCooldown = TechniqueUIManager.instance.techniqueCooldown;
        techniqueDamage = TechniqueUIManager.instance.techniqueDamage;
        techniqueImage = TechniqueUIManager.instance.techniqueImage;
}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            switch (gameObject.name)
            {
                case "Slingshot":
                    techniqueDescription.text = "Dash Toward a Direction, damaging Enemies on the way";
                    techniqueCooldown.text = "Cooldown: " + Slingshot.defaultCooldown;
                    techniqueDamage.text = "Damage: " + Slingshot.defaultDamage;
                    break;
                case "ChiSpit":
                    techniqueDescription.text = "Fire the universe’s candy energy. Eons of scholars have been unable to understand why it can only come out of your mouth.";
                    techniqueCooldown.text = "Cooldown: " + ChiSpit.defaultCooldown;
                    techniqueDamage.text = "Damage: " + ChiSpit.defaultDamage;
                    break;
                case "Slash":
                    techniqueDescription.text = "Perform a deadly circular slash, dealing AOE Damage";
                    techniqueCooldown.text = "Cooldown: " + Slash.defaultCooldown;
                    techniqueDamage.text = "Damage: " + Slash.defaultDamage;

                    break;
                case "Whip":
                    techniqueDescription.text = "Precise Attack with High Damage, but only hits 1 enemy";
                    techniqueCooldown.text = "Cooldown: " + Whip.defaultCooldown;
                    techniqueDamage.text = "Damage: " + Whip.defaultDamage;
                    break;
                case "Boomerang":
                    techniqueDescription.text = "Toss a slow, wide deadly projectile but leaves you defenseless";
                    techniqueCooldown.text = "Cooldown: " + Boomerang.defaultCooldown;
                    techniqueDamage.text = "Damage: " + Boomerang.defaultDamage;
                    break;
            }


            techniqueImage.GetComponent<UnityEngine.UI.Image>().sprite = techniqueSprite;
            techniqueName.text = gameObject.name + " [TECHNIQUE]";
            print("I AM " + gameObject.name);
            techniqueDetailsPanel.SetActive(true);

        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            techniqueDetailsPanel.SetActive(false);
        }
    }


}

