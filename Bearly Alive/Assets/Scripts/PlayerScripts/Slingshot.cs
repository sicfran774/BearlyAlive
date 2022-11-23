using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : Technique
{
    // to be manipulated by designer
    public const int defaultDamage = 5;
    public const float defaultCooldown = 2f;
    public float slingSpeed = 10f;

    // for applying movement to actor
    private Rigidbody2D actorBody;

    public Slingshot (GameObject actor, int damage = defaultDamage, float cooldown = defaultCooldown) : base(actor, damage, cooldown) {
        actorBody = actor.GetComponent<Rigidbody2D>();
    }

    // When called, the referenced GameObject will stop moving, slide back, then shoot forward with a damaging hitbox.
    public override void Act()
    {

        print("slingshotting");
    }

    public override void SetUpgrade(string upgrade) {
        base.upgrade = upgrade;// placeholder implementation.
    }
    
    // Update is called once per frame
    void Update()
    {
        print("slingshot update");
        move();
    }

    private void move() {
        Vector2 currPosition = actor.transform.position;
        Vector2 displacement = actor.transform.forward * Time.deltaTime * slingSpeed;
        currPosition += displacement;
        actorBody.MovePosition(currPosition);
    
    }
}
