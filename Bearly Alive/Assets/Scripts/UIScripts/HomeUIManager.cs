using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUIManager : MonoBehaviour
{
    //Start the game 
    public void StartGame()
    {
        //Enable gameplay music and load level1 scene
        if(GameManager.instance != null)
        {
            GameManager.instance.GetComponent<AudioSource>().Play();
        }

        SceneManager.LoadSceneAsync("Level1");
    }
}
