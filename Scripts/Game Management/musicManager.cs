using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicManager : MonoBehaviour
{
    private AudioSource[] audioSources;
    private bool endScreenOn = false;
    public AudioClip endScreenLoopClip;
    // Start is called before the first frame update
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
    }

    private void Update()
    {
        if (endScreenOn && !audioSources[2].isPlaying)
        {
            audioSources[2].clip = endScreenLoopClip;
            audioSources[2].loop = true;
            audioSources[2].Play();
        }
    }

    public void startCircusMusic()
    {
        audioSources[0].mute = true;
        audioSources[1].mute = false;
    }
    
    public void startDanceMusic()
    {
        audioSources[0].mute = false;
        audioSources[1].mute = true;
    }

    public void stopMusic()
    {
        audioSources[0].mute = true;
        audioSources[1].mute = true;
    }
    public void startEndScreenMusic()
    {
        endScreenOn = true;
        audioSources[2].Play();
    }
}
