using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
   public Sprite dying;
   public Sprite dead;

   public SpriteRenderer spriteRenderer;

   private void Reset()
   {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.enabled = false;
   }

   private void OnEnable() 
   {
        UpdateSprite();
        DisablePhysics();
        StartCoroutine(Animate());
        
   }

   private void UpdateSprite()
   {
          spriteRenderer.enabled = true;
          spriteRenderer.sortingOrder = 10;
          if (dying != null) {
              spriteRenderer.sprite = dying; 
          }
          
   }

   private void DisablePhysics()
   {
          Collider2D[] colliders = GetComponents<Collider2D>();
          foreach (Collider2D collider in colliders) {
               collider.enabled = false;
          }

          GetComponent<Rigidbody2D>().isKinematic = true;
   }

   private IEnumerator Animate()
   {
          float elapsed = 0f;
          float duration = 1f;

          while (elapsed < duration) {
               spriteRenderer.sprite = dying;
               elapsed += Time.deltaTime;
               yield return null;
          }

          spriteRenderer.sprite = dead;
   }


}
