/*************************************************************** 
*file: AnimatedSprite.cs 
*author: M. Tene 
*class: CS 4700 – Game Development 
*assignment: program 4 
*date last modified: 12/5/2022 
* 
*purpose: this program handles sprite animation logic for the boss. 
* 
****************************************************************/ 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpriteRenderer : MonoBehaviour
{
    public DeathAnimation death;
    public Sprite idle;
    public DeathAnimation shoot;
    public AnimatedSprite moving;
    public Sprite shadow;

    private BossController boss;
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        boss = GetComponent<BossController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        moving.enabled = boss.isMoving;
        death.enabled = boss.isDead;
        shoot.enabled = boss.isShooting;
        if (boss.isJumping) {
            spriteRenderer.enabled = false;
        }
    }

}
