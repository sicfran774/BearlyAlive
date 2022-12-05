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

    public void playPickedTechniqueSound()
    {
        audioSource.clip = soundEffects[3];
        audioSource.Play();
    }

    public void playPickedUpgradeSound()
    {
        audioSource.clip = soundEffects[4];
        audioSource.Play();
    }

    public void playPauseSound()
    {
        audioSource.clip = soundEffects[5];
        audioSource.Play();
    }


    public void playUnpauseSound()
    {
        audioSource.clip = soundEffects[6];
        audioSource.Play();
    }

    public void playHitSound()
    {
        audioSource.clip = soundEffects[7];
        audioSource.Play();
    }


}

