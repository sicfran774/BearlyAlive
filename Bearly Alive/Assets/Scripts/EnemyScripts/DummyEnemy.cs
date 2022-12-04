using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Bullet(Clone)")
        {
            Destroy(gameObject);
        }
    }
}
