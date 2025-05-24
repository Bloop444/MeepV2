using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingFPSBoost : MonoBehaviour
{
    [Header("FPS Boost Settings")]
    [Tooltip("Target frame rate to boost to.")]
    public int targetFrameRate = 60; // Default target frame rate

    [Header("Quality Settings")]
    [Tooltip("Camera that affects quality settings.")]
    public Camera targetCamera; // Camera affecting quality settings

    [Header("Quality Adjustments")]
    [Tooltip("Maximum quality level when FPS boost is lowest.")]
    public int maxQualityLevel = 5; // Maximum quality level (index)
    [Tooltip("Minimum quality level when FPS boost is highest.")]
    public int minQualityLevel = 0; // Minimum quality level (index)

    private int currentQualityLevel;

    private void Start()
    {
        ApplyQualitySettings();
    }

    private void Update()
    {
        AdjustQualityBasedOnFPS();
    }

    private void AdjustQualityBasedOnFPS()
    {
        // Adjust quality level based on target frame rate
        float factor = Mathf.Clamp01((float)targetFrameRate / 60f); // Normalize frame rate to a 0-1 range
        currentQualityLevel = Mathf.RoundToInt(Mathf.Lerp(minQualityLevel, maxQualityLevel, factor));

        // Apply quality settings
        ApplyQualitySettings();
    }

    private void ApplyQualitySettings()
    {
        QualitySettings.SetQualityLevel(currentQualityLevel, true); // Apply current quality level

        // If a target camera is specified, adjust its settings accordingly
        if (targetCamera != null)
        {
            targetCamera.depth = currentQualityLevel; // Example adjustment based on quality level
        }
    }
}
