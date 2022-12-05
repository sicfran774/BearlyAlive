using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public RoomManager roomManager;

    void LateUpdate()
    {
        cameraMovement();
    }

    // Follows Player Objects Movement
    void cameraMovement()
    {
        //This code below will snap camera to each room
        if (roomManager.playerCurrentRoom != null)
        {
            transform.position = new Vector3(roomManager.playerCurrentRoom.transform.position.x, roomManager.playerCurrentRoom.transform.position.y, transform.position.z);
        }

        //Old code
        //transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
