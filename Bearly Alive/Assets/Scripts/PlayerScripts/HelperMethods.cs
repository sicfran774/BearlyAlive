/***************************************************************
*File: HelperMethods.cs
*Author: Radical Cadavical
*Class: CS 4700 – Game Development
*Assignment: Program 4
*Date last modified: 12/5/2022
*
*Purpose: This program implements methods to handle player's 
*rotation and technique implementation.
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperMethods : MonoBehaviour
{
    //returns a normalized vector pointing from the player to the mouse cursor
    public Vector2 CursorVector()
    {
        Vector3 CursorVector3 = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        return (new Vector2(CursorVector3.x, CursorVector3.y));
    }

    //returns a quaternion pointing from the player to the mouse cursor
    public Quaternion CursorAngle() 
    {
		//Get the Screen position of the mouse
		Vector3 mouseOnScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		Vector3 rotation = (mouseOnScreen - transform.position).normalized;

		//Get the angle between the points
		float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

		return Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
}
