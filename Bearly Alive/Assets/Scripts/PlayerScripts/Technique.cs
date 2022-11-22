using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Technique : Action
{
    private string upgrade;

    public Technique(GameObject actor) : base(actor) {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public abstract void SetUpgrade(string newUpgrade);

}
