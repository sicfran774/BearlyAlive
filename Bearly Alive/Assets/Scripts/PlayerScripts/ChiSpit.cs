using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiSpit : Technique
{

    // Hud to display ammo count
    HudManager hudManager;

    // the ammount of shots before reloading
    public int _MAX_AMMO = 15;

    int rounds = 15;


    // to be manipulated by designer
    public const int defaultDamage = 1;
    public const float defaultCooldown = 0.4f;

    // ALWAYS call after add component
    public override void Initialize (int damage = defaultDamage, float cooldown = defaultCooldown) {
        base.Initialize(defaultDamage, defaultCooldown);
        hudManager = gameObject.GetComponent<PlayerController>().hudManager;
    }

    // call when the actor should perform chiSpit
    public override void Act() {
        if (!techsCooling)
        {
            startCooling();
            FireWeapon();
        }
    }

    // Act will have different behavior depending on which upgrade is set.
    public override void SetUpgrade(string newUpgrade) {
        upgrade = newUpgrade;
    }

    // Handles Player's firing, Projectile Sound, reloading and Weapon's Ammo UI.
    // If the weapon has no ammo left, Reload Method is invoke after 2 second delay.
    // Use Left Crtl or Mouse Click to fire weapon
    void FireWeapon()
    {
        if (rounds > 0)
        {
            // Fire Bullet
            rounds--;
            SoundManager.instance.playBulletSound();
            GameManager.instance.DecreaseAmmo(1);
            hudManager.refresh();

            // spawn bullet, then set its upgrade tag
            GameObject latestBullet = Instantiate(GameManager.instance.ChiSpitProjectile, transform.position, transform.rotation);
            latestBullet.tag = upgrade;

            // debug print("Bullets Remaining: " + rounds);
        }

        if(rounds == 0)
        {
            print("Reloading");
            //Reload, wait 2 seconds
            Invoke("Reload", 2f);        
        }

    }


    // Handles Weapon Reload for Basic Weapon
    void Reload()
    {
        rounds = GameManager.instance._MAX_AMMO;
        GameManager.instance.SetAmmo();
        hudManager.refresh();
        CancelInvoke("Reload");
        print("Bullet's Remaining: " + rounds);
    }


}
