using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class SpriteFlip : MonoBehaviour
{
    public Transform player;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    public void Awake()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        this.spriteRenderer.flipX = player.transform.position.x > this.transform.position.x;
    }
}
