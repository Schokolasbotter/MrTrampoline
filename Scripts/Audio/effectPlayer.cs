using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class effectPlayer : MonoBehaviour
{
    #region
    [Header("Audio Clips")]
    [SerializeField] private AudioClip normalBounce;
    [SerializeField] private AudioClip chargedBounce, chargeUp, correctDance1, correctDance2, correctDance3, correctDance4, failDance, scoreDing, floorHit;

    [Header("Audio Sources")]
    [SerializeReference] private AudioSource mrTAudiosource;
    [SerializeReference] private AudioSource trampolineAudiosource;
    [SerializeReference] private AudioSource scoreAudiosource;
    private musicManager musicManager;
    #endregion

    private void Start()
    {
        musicManager = GetComponent<musicManager>();
    }

    //Functions which play the sound once
    public void playBounce()
    {
        mrTAudiosource.PlayOneShot(normalBounce);
    }

    public void playChargedBounce()
    {
      mrTAudiosource.PlayOneShot(chargedBounce);
    }

    public void playFail()
    {
        mrTAudiosource.PlayOneShot(failDance);
    }
    public void playScore()
    {
        mrTAudiosource.PlayOneShot(scoreDing);
    }

    //Trampoline charging sound, with stop and status
    public void playCharging()
    {
        trampolineAudiosource.PlayOneShot(chargeUp);
    }

    public void stopTrampolineAudio()
    {
        trampolineAudiosource.Stop();
    }

    public bool isTrampolinePlaying()
    {
        return trampolineAudiosource.isPlaying;
    }

    //Play a random sound for a correct swipe
    public void playCorrect()
    {
        float rNumber = Random.Range(0f, 1f);
        switch (rNumber)
        {
            case <= 0.25f:
                mrTAudiosource.PlayOneShot(correctDance1);
                break;
            case <= 0.5f:
                mrTAudiosource.PlayOneShot(correctDance2);
                break;
            case <= 0.75f:
                mrTAudiosource.PlayOneShot(correctDance3);
                break;
            case <= 1f:
                mrTAudiosource.PlayOneShot(correctDance4);
                break;
        }
    }
    
    //Coroutine to first let the floorhit sound finish before starting the endscreen music
    public IEnumerator playFloorHit()
    {
        musicManager.stopMusic();
        mrTAudiosource.PlayOneShot(floorHit);
        print(floorHit.length);
        yield return new WaitForSecondsRealtime(floorHit.length);
        musicManager.startEndScreenMusic();
    }

    //Score Roll start and stop functions
    public void playScoreRoll()
    {
        scoreAudiosource.Play();
    }
    public void endScoreRoll()
    {
        scoreAudiosource.Stop();
    }
}
