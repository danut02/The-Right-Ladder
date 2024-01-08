using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Monologue : MonoBehaviour
{
    public static Monologue instance;
    public TMP_Text displayedText;

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

    public void DisplayText(string text_received, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayTextRoutine(text_received, duration));
    }

    private IEnumerator DisplayTextRoutine(string text, float duration)
    {
        displayedText.text = text;
        displayedText.enabled = true;
        yield return new WaitForSeconds(duration);
        displayedText.enabled = false;
    }

    public void Start()
    {
        displayedText.enabled = false;
    }
    
}