using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Any of the player's main moves
// Assertion: this is instantiated as a component of a GameObject with a TechHandler
public abstract class Technique : Action
{

    [Header("Technique Values")]
    [SerializeField]
    // the ammount of damage this technique's hitbox will do to an enemy
    protected int damage;
    [SerializeField]
    // the duration (milli seconds) untill another technique can be performed. 
    protected float cooldown;
    // this variable lets all Techniques coordinate their cooldowns.

    public static bool techsCooling {
        get;
        protected set;
    }

    public static bool moveLock {
        get;
        protected set;
    }

    public static bool cursorLock {
        get;
        protected set;
    }

    // different values of upgrade cause different actions
    protected string upgrade = "none";

    // must be called after AddComponent
    // in the abscence of constructors, this initializes the instance's fields.
    public virtual void Initialize(int damage = 0, float cooldown = 0) {
        this.damage = damage;
        this.cooldown = cooldown;
    }

    // use when player installs an upgrade on a technique
    public abstract void SetUpgrade(string newUpgrade);

    // use in Act to halt other actions
    protected void startCooling() {
        techsCooling = true;
        Invoke("ResetTechniqueCooldown", cooldown);
    }


    // helper method for startCooling
    private void ResetTechniqueCooldown() {
        techsCooling = false;
    }
}
