using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAudio : MonoBehaviour
{
    private AudioSource[] allAudioSources;

    void Awake()
    {
        allAudioSources = FindObjectsOfType<AudioSource>();
    }

    public void PauseAllSounds()
    {
        foreach (var audioSrc in allAudioSources)
        {
            if (audioSrc.isPlaying)
                audioSrc.Pause();
        }
    }

    public void ResumeAllSounds()
    {
        foreach (var audioSrc in allAudioSources)
        {
            audioSrc.UnPause();
        }
    }
}

