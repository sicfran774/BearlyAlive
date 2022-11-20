using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    void LateUpdate()
    {
        cameraMovement();
    }

    // Follows Player Objects Movement
    void cameraMovement()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
