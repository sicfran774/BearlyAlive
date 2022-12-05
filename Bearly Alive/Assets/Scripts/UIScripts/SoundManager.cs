using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance = null;

    [SerializeField]
    AudioClip[] soundEffects;
    AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void playBulletSound()
    {
        audioSource.clip = soundEffects[0];
        audioSource.Play();
    }

    public void playSlashSound()
    {
        audioSource.clip = soundEffects[1];
        audioSource.Play();
    }

    public void playWalkSound()
    {
        audioSource.clip = soundEffects[2];
        audioSource.Play();
    }


}

