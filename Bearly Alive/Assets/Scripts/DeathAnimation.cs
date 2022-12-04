using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
   public Sprite[] sprites;
   private int frame = 0;
   public float framerate = 1f / 6f;

   public SpriteRenderer spriteRenderer;

   private PlayerController controller;

   private void Reset()
   {
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerController>();
   }

   private void OnEnable() 
   {
          if (controller.dead) {
                InvokeRepeating(nameof(Animate), framerate, framerate);
          }
          
   }

   private void Animate()
   {
        frame++;

        if(frame >= 0 && frame < sprites.Length) {
            spriteRenderer.sprite = sprites[frame];
        }
   }
}
