using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnableGpuInstancing : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Optimization/Enable GPU Instancing")]
    static void Enable()
    {
        SetGpuInstancing(true);
    }

    [MenuItem("Optimization/Disable GPU Instancing")]
    static void Disable()
    {
        SetGpuInstancing(false);
    }

    static void SetGpuInstancing(bool value)
    {
        foreach (var guid in AssetDatabase.FindAssets("t:Material", new[] { "Assets" }))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material != null)
            {
                material.enableInstancing = value;
                EditorUtility.SetDirty(material);
            }
        }
    }
#endif
}
