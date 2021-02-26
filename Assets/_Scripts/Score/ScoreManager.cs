﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    public int m_score = 0;
    public int m_addScore = 0;
    int highScore = 0;

    bool changeScoreStanby = false;

    [SerializeField]float stanbyTime = 1.0f;
    float stanbyNowTime = 0.0f;

    [SerializeField] float animationTime = 1.0f;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] PlayerData playerData;

    private void Update()
    {
        scoreText.text = "Score:" + m_score;
        if (m_score > highScore)
        {
            highScore = m_score;
            highScoreText.text = "HighScore:" + highScore;
        }
        
        if (changeScoreStanby)
        {
            StanbyCountUp();
        }
    }

    void StanbyCountUp()
    {
        stanbyNowTime += Time.deltaTime;
        if (stanbyNowTime > stanbyTime)
        {
            ResetStanbyCount();
            changeScoreStanby = false;
            ChangeScreenText();
        }
    }

    public void AddScore(int addScore)
    {
        m_addScore += addScore;
        ResetStanbyCount();
        changeScoreStanby = true;
        Debug.Log("addScore:" + addScore);
    }

    void ResetStanbyCount()
    {
        stanbyNowTime = 0;
    }

    public void ChangeScreenText()
    {
        DOTween.To(
            () => m_score,
            num => m_score = num,
            m_addScore,
            animationTime);
    }

    public void ResetScore()
    {
        m_addScore = 0;
        m_score = 0;
    }
}
