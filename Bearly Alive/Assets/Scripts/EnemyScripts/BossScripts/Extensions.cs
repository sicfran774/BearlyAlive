/*************************************************************** 
*file: AnimatedSprite.cs 
*author: M. Tene 
*class: CS 4700 – Game Development 
*assignment: program 4 
*date last modified: 12/4/2022 
* 
*purpose: this program handles sprite collision logic for the boss. 
* 
****************************************************************/ 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    private static LayerMask layerMask = LayerMask.GetMask("Default");
    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        if(rigidbody.isKinematic)
        {
            return false;
        }

        float radius = 0.25f;
        float distance = 0.375f;

       RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask);
       return hit.collider != null && hit.rigidbody != rigidbody;
    }
}
