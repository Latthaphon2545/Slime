using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    // FX
    public AudioSource dropItemSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource pickupSound;
    public AudioSource walkSound;
    public AudioSource jumpSound;


    //Music
    public AudioSource startingZoneBGMusic;
    public AudioSource natureBGMusic;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    public void playSound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }

}
