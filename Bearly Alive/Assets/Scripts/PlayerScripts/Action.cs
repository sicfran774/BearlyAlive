using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    //when you want the actor to preform the action, call this method
    public abstract void Act();
}
