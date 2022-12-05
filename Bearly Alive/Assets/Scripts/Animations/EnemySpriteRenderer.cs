/*************************************************************** 
*file: EnemySpriteRenderer.cs 
*author: M. Tene 
*class: CS 4700 – Game Development 
*assignment: program 4 
*date last modified: 12/4/2022 
* 
*purpose: this program handles logic for the sprite renderer of the enemies/boss: decision on which sprite/animation
        to select and display. 
* 
****************************************************************/ 





using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteRenderer : MonoBehaviour
{
   public AnimatedSprite moving;
    public Sprite idle;
    public DeathAnimation dying;

    private EnemyController enemyController;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void Disable()
    {
        spriteRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        if (enemyController.isDead) {
            dying.enabled = true;
        } else {
            moving.enabled = true;
        }

    }
}
