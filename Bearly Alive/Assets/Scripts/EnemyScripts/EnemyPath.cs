using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPath : MonoBehaviour
{
    public AIPath aIPath;

    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        faceVelocity();
    }

    void faceVelocity() {
        direction = aIPath.desiredVelocity;

        transform.right = direction;
    }

}
