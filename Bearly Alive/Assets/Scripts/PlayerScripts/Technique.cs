using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Technique : Action
{

    // the ammount of damage this technique's hitbox will do to an enemy
    protected int damage;
    // the duration (milli seconds) untill another technique can be performed. 
    protected float cooldown;

    protected string upgrade;

    public Technique(GameObject actor, int damage, float cooldown) : base(actor) {
        this.damage = damage;
        this.cooldown = cooldown;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public abstract void SetUpgrade(string newUpgrade);

}
