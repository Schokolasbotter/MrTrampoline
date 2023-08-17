using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class mainMenuAudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [Header("AudioClip")]
    public AudioClip mainThemeLoop;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        //Audio starts with intro sound which is attached to audiosource at start
        //Once that is finished, we change to the loopable part and play it
        if (!audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.clip = mainThemeLoop;
            audioSource.Play();
        }
    }
}
