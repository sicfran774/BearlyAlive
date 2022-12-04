using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    public AnimatedSprite moving;
    public Sprite idle;
    public DeathAnimation dying;
    public TechniqueAnimation[] techniques;

    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
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
        moving.enabled = playerController.isMoving;

        dying.enabled = playerController.isDead;

        if (!playerController.isMoving) {
            spriteRenderer.sprite = idle;
        }
    }
}
