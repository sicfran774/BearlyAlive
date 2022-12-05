/*************************************************************** 
*file: DeathAnimation.cs 
*author: M. Tene 
*class: CS 4700 – Game Development 
*assignment: program 4 
*date last modified: 12/3/2022 
* 
*purpose: this program handles the death animation for player and enemies. 
* 
****************************************************************/ 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
   public Sprite[] sprites;
   private int frame = 0;
   public float framerate = 1f / 6f;

   public SpriteRenderer spriteRenderer;

   private void Awake()
   {
        spriteRenderer = GetComponent<SpriteRenderer>();
   }

   private void OnEnable() 
   {
     InvokeRepeating(nameof(Animate), framerate, framerate);     
   }

   private void Animate()
   {
        frame++;

        if(frame >= 0 && frame < sprites.Length) {
            spriteRenderer.sprite = sprites[frame];
        }
   }
}
