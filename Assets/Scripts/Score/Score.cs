using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public Transform playerTransform;
    public TMP_Text scoreText;
    private float initialYPosition;
    private float highestYPosition;
    private int score = 0;

    void Start()
    {
        initialYPosition = playerTransform.position.y;
        highestYPosition = initialYPosition;
    }

    void Update()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        float currentYPosition = playerTransform.position.y;
        if (currentYPosition > highestYPosition)
        {
            highestYPosition = currentYPosition;
            score = Mathf.RoundToInt(highestYPosition - initialYPosition);
        }
        scoreText.text = $"Score: {score:N0}";
    }
}