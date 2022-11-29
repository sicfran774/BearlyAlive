using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    void Start()
    {
        Invoke("DestoryProjectile", 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Destroy(gameObject);
        }

        if (collision.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    void DestoryProjectile()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
