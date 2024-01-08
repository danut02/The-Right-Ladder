using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource source;
    public AudioMixer mixer;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SoundManager soundManager = FindObjectOfType<SoundManager>();
        
        source = soundManager.source;
        mixer.SetFloat("MenuVol", Mathf.Log10(PlayerPrefs.GetFloat("MenuVol")) * 20);
        mixer.SetFloat("MasterVol", Mathf.Log10(PlayerPrefs.GetFloat("MasterVol")) * 20);
        mixer.SetFloat("GameVol", Mathf.Log10(PlayerPrefs.GetFloat("GameVol")) * 20);
    }
}
