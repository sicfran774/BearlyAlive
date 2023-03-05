using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public RoomManager roomManager;
    public float cameraTransitionSpeed = 1.0f;

    //This code below will move camera to each room
    public IEnumerator cameraMovement(Vector3 newPosition)
    {
        float t = 0.0f;
        Vector3 startingPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        while(t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / cameraTransitionSpeed);
            transform.position = Vector3.Lerp(startingPos, newPosition, t);
            yield return 0;
        }

        //Old code
        //transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
