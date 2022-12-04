using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechniqueAnimation : MonoBehaviour
{
    private int frame = 0;
    public float framerate = 1f / 6f;
    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;

   private void Reset()
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
