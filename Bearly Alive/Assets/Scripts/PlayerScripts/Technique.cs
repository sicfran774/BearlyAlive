/***************************************************************
*File: Technique.cs
*Author: Radical Cadavical
*Class: CS 4700 – Game Development
*Assignment: Program 4
*Date last modified: 12/5/2022
*
*Purpose: This program creates abstract methods that will be 
*implemented by each technique class to perform its action.
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Any of the player's main moves
// Assertion: this is instantiated as a component of a GameObject
// with a TechHandler
public abstract class Technique : Action
{

    [Header("Technique Values")]
    [SerializeField]
    // the ammount of damage this technique's hitbox will do to an
    // enemy
    protected int damage;
    [SerializeField]
    // the duration (milli seconds) untill another technique can be
    // performed. 
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

    public static bool rotationLock
    {
        get;
        protected set;
    }

    // different values of upgrade cause different actions
    public string upgrade {
        get;
        protected set;
    }

    //function: Initialize
    //purpose: In the abscence of constructors, this initializes the
    //instance's fields.
    public virtual void Initialize(int damage = 0, float cooldown = 0) {
        this.damage = damage;
        this.cooldown = cooldown;
        upgrade = "UpgradeNone";
    }

    //function: SetUpgrade
    //purpose: Installs an upgrade on a technique when
    //player choose technique on UpgradeMenu
    public abstract void SetUpgrade(string newUpgrade);


    //function: startCooling
    //purpose: Handles the technique cooldown
    // to reset based on the specific technique
    protected void startCooling() {
        techsCooling = true;
        Invoke("ResetTechniqueCooldown", cooldown);
    }


    //funciton: ResetTechniqueCooldown
    //purpose: helper method for startCooling
    private void ResetTechniqueCooldown() {
        techsCooling = false;
    }

    //function: OnDestroy (MonoBehaviour)
    //purpose: Reset all cooldowns for technique if level is restarted
    void OnDestroy()
    {
        ResetTechniqueCooldown();
        moveLock = false;
        cursorLock = false;
        rotationLock = false;
    }
}
