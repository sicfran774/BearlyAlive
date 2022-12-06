/***************************************************************
*File: ChiSpit.cs
*Author: Radical Cadavical
*Class: CS 4700 – Game Development
*Assignment: Program 4
*Date last modified: 12/5/2022
*
*Purpose: This program implements the ChiSpit Technique by
*implementing the functions inherited from Technique Class.
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiSpit : Technique
{

    // Hud to display ammo count
    HudManager hudManager;

    // the ammount of shots before reloading
    public int _MAX_AMMO = 15;

    // grab HelperMethods for angling the bullets
    public HelperMethods helpers;

    // Max ammo 
    int rounds = 15;


    // to be manipulated by designer
    public const int defaultDamage = 1;
    public const float defaultCooldown = 0.4f;

    // ALWAYS call after add component
    //function: Initialize
    //purpose: Sets the default damage and cooldown values when technique
    //has been learned/equipped by the player
    public override void Initialize (int damage = defaultDamage, float cooldown = defaultCooldown) {
        base.Initialize(defaultDamage, defaultCooldown);
        helpers = GetComponent<HelperMethods>();
        hudManager = gameObject.GetComponent<PlayerController>().hudManager;
    }



    //function: Act
    //purpose: Overrides Action's Act method to perform
    //Chispit firing and start technique's cooldown
    public override void Act() {
        if (!techsCooling)
        {
            startCooling();
            FireWeapon();
        }
    }

    //function: SetUpgrade
    //purpose: Sets the pickedUp upgrade to the technique
    public override void SetUpgrade(string newUpgrade) {
        upgrade = newUpgrade;
    }

    //function: FireWeapon
    //purpose: Handles Player's firing, Projectile Sound, reloading and Weapon's Ammo UI.
    // If the weapon has no ammo left, Reload Method is invoke after 2 second delay.
    // Use Left Crtl or Mouse Click to fire weapon
    void FireWeapon()
    {
        if (rounds > 0)
        {
            // Start Animation
            //GetComponent<TechniqueAnimation>().enabled = true; // this bugged the firing
            // Fire Bullet
            rounds--;
            SoundManager.instance.playBulletSound();
            GameManager.instance.DecreaseAmmo(1);
            hudManager.refresh();

            // spawn bullet, then set its upgrade tag
            GameObject latestBullet = Instantiate(GameManager.instance.ChiSpitProjectile, transform.position, helpers.CursorAngle());
            latestBullet.tag = upgrade;

        }

        if(rounds == 0)
        {
            //Reload, wait 2 seconds
            Invoke("Reload", 2f);        
        }

    }


    //function: Reload
    //purpose: Handles Weapon Reload for Chispit Technique
    void Reload()
    {
        rounds = GameManager.instance._MAX_AMMO;
        GameManager.instance.SetAmmo();
        hudManager.refresh();
        CancelInvoke("Reload");
        print("Bullet's Remaining: " + rounds);
    }


}
