using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Slash : MonoBehaviour
{

    public float slashDuration = 0.5f;
    float speed;
    Rigidbody2D sword;
    Collider2D coll;
    public GameObject rotatePoint;

    //This method is called when the gameobject must preform


    // Makes the sword hidden when it is first initializes when the game starts
    private void Awake()
    {
        gameObject.SetActive(false);
    }


    // When the player activates the Slash Technique, sword object is re-enabled
    // When sword is re-enabled, function is called setting up the rigidbody and collider
    // and performing the circular slash
    private void OnEnable()
    {
        //speed = slashSpeed;
        sword = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        StartCoroutine(Rotate(slashDuration));

    }

    // When the circular slash is done, the sword object is disabled
    // This function is called, removing its rigidbody and collider
    private void OnDisable()
    {
        sword = null;
        coll = null;
    }


    // Coroutine that handles the circular slash technique.
    // The amount of time it takes to complete the action is 
    // based on the argument passed.
    IEnumerator Rotate(float duration)
    {
        float startRotation = rotatePoint.transform.eulerAngles.z + 45f;
        float endRotation = startRotation - 405.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 405.0f;
            rotatePoint.transform.eulerAngles = new Vector3(rotatePoint.transform.eulerAngles.x, rotatePoint.transform.eulerAngles.y, zRotation);
            yield return null;
        }
        gameObject.SetActive(false);
    }

}
