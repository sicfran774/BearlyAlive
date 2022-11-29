using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechniqueInteractionController : MonoBehaviour
{

    Rigidbody2D technique;
    Collider2D coll;
    // Start is called before the first frame update
    void Start()
    {
        technique = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            print("SLASH TECHNIQUE ACQUIRED");
            Destroy(gameObject);
        }
    }


}
