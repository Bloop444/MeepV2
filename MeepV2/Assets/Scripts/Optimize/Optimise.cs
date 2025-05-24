using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Optimise : MonoBehaviour
{
    public enum AntiAliasingLevel
    {
        None = 0,
        Low = 2,
        Medium = 4,
        High = 8
    }
    [Header("Your Chosen FPS")]
    [Space]
    [Range(60,120)]
    public int targetFPS = 120;
    [Space]
    [Header("Your Chosen Anti Alias setting")]
    [Space]
    public AntiAliasingLevel antiAliasingLevel = AntiAliasingLevel.Low;

    void Start()
    {
        QualitySettings.antiAliasing = (int)antiAliasingLevel;
        Application.targetFrameRate = targetFPS;
    }
}