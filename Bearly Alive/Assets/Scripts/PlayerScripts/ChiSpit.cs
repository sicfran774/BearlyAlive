using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiSpit : Technique
{

    // Bullet Game Object
    public GameObject projectile;

    // Bullet Firing Sound
    public AudioSource bulletSource;

    // Hud to display ammo count
    public HudManager hudManager;

    // the ammount of shots before reloading
    public int _MAX_AMMO = 15;

    
    int rounds;
 
 
    // to be manipulated by designer
    public const int defaultDamage = 5;
    public const float defaultCooldown = 2f;

    // ALWAYS call after add component
    public override void Initialize (int damage = defaultDamage, float cooldown = defaultCooldown) {
        base.Initialize(damage, cooldown);
        
    }

    // call when the actor should perform chiSpit
    public override void Act() {
            FireWeapon();
            startCooling();
    }

    // Act will have different behavior depending on which upgrade is set.
    public override void SetUpgrade(string newUpgrade) {

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
            bulletSource.Play();
            GameManager.instance.DecreaseAmmo(1);
            hudManager.refresh();
            Instantiate(projectile, transform.position, transform.rotation);
            print("Bullets Remaining: " + rounds);
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
