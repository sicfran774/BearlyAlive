/***************************************************************
*File: Action.cs
*Author: Radical Cadavical
*Class: CS 4700 – Game Development
*Assignment: Program 4
*Date last modified: 12/5/2022
*
*Purpose: This program creates an abstract method that will be 
*implemented by each technique class to perform its action.
****************************************************************/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Action : MonoBehaviour
{
    //when you want the actor to preform the action, call this method
    public abstract void Act();
}
