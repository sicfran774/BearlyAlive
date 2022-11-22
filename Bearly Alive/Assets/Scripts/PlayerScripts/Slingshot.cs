using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : Technique
{

    public Slingshot (GameObject actor, int damage, float cooldown ) : base(actor, damage, cooldown) {

    }

    // Start is called before the first frame update
    public override void Act()
    {
        
    }

    public override void SetUpgrade(string upgrade) {
        base.upgrade = upgrade;// placeholder implementation.
    }

    // When called, the referenced GameObject will stop moving, slide back, then shoot forward with a damaging hitbox.

    // Update is called once per frame
    void Update()
    {
        
    }
}
