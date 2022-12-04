using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
   public Sprite[] sprites;
   public float framerate = 1f / 6f;

   private SpriteRenderer spriteRenderer;
   private PlayerController controller;
   private int frame;

   private void Awake()
   {
        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerController>();
   }

    //start animating
   private void OnEnable()
   {
        // call animate at specified framerate
        InvokeRepeating(nameof(Animate), framerate, framerate);
   }

   private void OnDisable()
   {
        CancelInvoke();
   }

   private void Animate()
   {
        frame++;

        if(frame >= sprites.Length) {
            frame = 0;
        }

        if(frame >= 0 && frame < sprites.Length) {
            spriteRenderer.sprite = sprites[frame];
        }
   }
}
