using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OptimizeForQuest2 : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Optimize for Oculus Quest 2")]
    static void Optimize()
    {
        // Set target framerate
        Application.targetFrameRate = 72;
        Debug.Log("Set target frame rate to 72 FPS");

        // Optimize Quality Settings
        QualitySettings.SetQualityLevel(0, true); // Assuming 0 is the lowest quality level
        Debug.Log("Set Quality Settings to lowest level");

        // Ensure URP is set up
        SetupURP();

        // Set up Oculus-specific settings
        PlayerSettings.colorSpace = ColorSpace.Linear;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

        // Set Android-specific settings
        PlayerSettings.Android.androidTVCompatibility = false;
        PlayerSettings.Android.androidIsGame = true;
        PlayerSettings.Android.blitType = AndroidBlitType.Never;

        // Set VR Settings for Meta XR Interaction SDK
        PlayerSettings.virtualRealitySupported = true;
        SetVirtualRealitySDKs(BuildTargetGroup.Android, new string[] { "Oculus" });
        PlayerSettings.stereoRenderingPath = StereoRenderingPath.SinglePass;

        // Optimize Physics
        Physics.autoSyncTransforms = false;
        Physics.defaultSolverIterations = 6;
        Physics.defaultSolverVelocityIterations = 1;
        Physics.bounceThreshold = 2.0f;

        // Other optimizations
        QualitySettings.vSyncCount = 0; // VSync should be disabled
        Time.fixedDeltaTime = 0.0111f; // 90Hz physics timestep
        Time.maximumDeltaTime = 0.0333f; // 30 FPS

        Debug.Log("Oculus Quest 2 optimization complete!");
    }

    static void SetupURP()
    {
        // Check if URP is set up, if not, set it up
        var pipelineAsset = GraphicsSettings.defaultRenderPipeline as UniversalRenderPipelineAsset;
        if (pipelineAsset == null)
        {
            Debug.LogError("URP is not set up. Please set up URP in your project before running this script.");
            return;
        }

        // Optimize URP settings
        pipelineAsset.renderScale = 0.8f; // Lower render scale for performance
        pipelineAsset.msaaSampleCount = 2; // Use 2x MSAA for better performance
        pipelineAsset.shadowDistance = 50f; // Reduce shadow distance
        pipelineAsset.supportsHDR = false; // Disable HDR
        pipelineAsset.supportsCameraDepthTexture = false; // Disable camera depth texture
        pipelineAsset.supportsCameraOpaqueTexture = false; // Disable camera opaque texture

        Debug.Log("URP settings optimized for Oculus Quest 2");
    }

    static void SetVirtualRealitySDKs(BuildTargetGroup target, string[] sdks)
    {
        // Ensure the virtual reality SDKs are set correctly
        SerializedObject settingsObj = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
        SerializedProperty vrSettings = settingsObj.FindProperty("m_BuildTargetVRSettings");
        
        if (vrSettings != null)
        {
            for (int i = 0; i < vrSettings.arraySize; i++)
            {
                SerializedProperty setting = vrSettings.GetArrayElementAtIndex(i);
                if (setting.FindPropertyRelative("m_BuildTarget").stringValue == target.ToString())
                {
                    SerializedProperty devices = setting.FindPropertyRelative("m_Devices");
                    devices.ClearArray();
                    for (int j = 0; j < sdks.Length; j++)
                    {
                        devices.InsertArrayElementAtIndex(j);
                        devices.GetArrayElementAtIndex(j).stringValue = sdks[j];
                    }
                    settingsObj.ApplyModifiedProperties();
                    Debug.Log("VR SDKs set for " + target.ToString());
                    return;
                }
            }
        }
        Debug.LogError("Failed to set VR SDKs for " + target.ToString());
    }
#endif
}
