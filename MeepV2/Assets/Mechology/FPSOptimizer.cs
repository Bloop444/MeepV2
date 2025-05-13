using UnityEngine;
using System;

public class FPSOptimizer : MonoBehaviour
{
    void Start()
    {
        
        if (!Application.isPlaying)
        { return; }

        FindObjectOfType<FPSOptimizer>();

        OptimizeQualitySettings();
        OptimizePhysicsSettings();
        OptimizeRendering();
    }

    void OptimizeQualitySettings()
    {
        QualitySettings.SetQualityLevel(0, true); 
    }

    void OptimizePhysicsSettings()
    {
        Time.fixedDeltaTime = 0.02f;
        Physics.sleepThreshold = 0.005f; 
    }

    void OptimizeRendering()
    {
        Camera.main.farClipPlane = 1000;
        Camera.main.depthTextureMode = DepthTextureMode.None;
        QualitySettings.antiAliasing = 0; 
        QualitySettings.shadowCascades = 0; 
        QualitySettings.shadowDistance = 0; 
    }

    void EnableOcclusionCulling()
    {
        Camera.main.useOcclusionCulling = true;
    }

    void EnableLOD()
    {
        
    }
}

