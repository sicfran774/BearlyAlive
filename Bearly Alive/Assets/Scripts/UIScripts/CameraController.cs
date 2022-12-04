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
        transform.position = new Vector3(roomManager.playerCurrentRoom.transform.position.x, roomManager.playerCurrentRoom.transform.position.y, transform.position.z);
    }
}
