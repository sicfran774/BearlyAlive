using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    GameObject target;
    public float speed;
    Rigidbody2D bulletRB;
    
    void Start()
    {
        Invoke("DestoryProjectile", 5f);
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
        bulletRB.velocity = new Vector2(moveDir.x, moveDir.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Destroy(gameObject);

        }
        else if (collision.tag == "Player")
        {
            Destroy(gameObject);

        }
        else if (collision.gameObject.name == "SlingshotBox")
        {
            Destroy(gameObject);

        } else if (collision.tag == "UpgradeJello") {
            bulletRB.velocity *= -1;
            bulletRB.tag = "UpgradeNone";
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
