using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TechniqueUI : MonoBehaviour
{
    Text techniqueDescription;
    Text techniqueName;
    Image techniqueImage;

    public Sprite techniqueSprite;

    Rigidbody2D body;
    Collider2D coll;


    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        techniqueDescription = TechniqueUIManager.instance.techniqueDescription;
        techniqueName = TechniqueUIManager.instance.techniqueName;
        techniqueImage = TechniqueUIManager.instance.techniqueImage;
}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            switch (gameObject.name)
            {
                case "Slingshot":
                    techniqueDescription.text = "Dash Toward a Direction, damaging Enemies on the way\n"
                    + "Cooldown: " + Slingshot.defaultCooldown + "\n" + "Damage: " + Slingshot.defaultDamage;
                    break;
                case "ChiSpit":
                    techniqueDescription.text = "Rapid fire projectiles channeled by the universe’s candy energy\n"
                        + "Cooldown: " + ChiSpit.defaultCooldown + "\n" + "Damage: " + ChiSpit.defaultDamage;
                    break;
                case "Slash":
                    techniqueDescription.text = "Perform a deadly circular slash, dealing AOE Damage\n"
                    + "Cooldown: " + Slash.defaultCooldown + "\n" + "Damage: " + Slash.defaultDamage;
                    break;
                case "Whip":
                    techniqueDescription.text = "Precise Attack with High Damage, but only hits 1 enemy\n"
                    + "Cooldown: " + Whip.defaultCooldown + "\n" + "Damage: " + Whip.defaultDamage;
                    break;
                case "Boomerang":
                    techniqueDescription.text = "Toss a slow, wide deadly projectile but leaves you defenseless\n"
                        + "Cooldown: " + Boomerang.defaultCooldown + "\n" + "Damage: " + Boomerang.defaultDamage;
                    break;
            }

            techniqueImage.GetComponent<Image>().sprite = techniqueSprite;
            techniqueName.text = gameObject.name + " [TECHNIQUE]";
            print("I AM " + gameObject.name);

        }
    }

}

