﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MagicLightProbes
{
    [CustomEditor(typeof(MLPCombinedVolume))]
    public class MLPCombinedVolumeEditor : Editor
    {
        private bool sceneGUIEnabled = false;

        public override void OnInspectorGUI()
        {
            if (!sceneGUIEnabled)
            {
                Enable();
            }

            MLPCombinedVolume combinedVolume = (MLPCombinedVolume)target;
            GeneralMethods.MLPCombinedVolumeEditor(combinedVolume);
            EditorUtility.SetDirty(combinedVolume);
        }

        [MenuItem("Tools/Magic Light Probes/Scene GUI/Enable")]
        public static void Enable()
        {
            SceneView.onSceneGUIDelegate -= OnScene;
            SceneView.onSceneGUIDelegate += OnScene;
        }

        [MenuItem("Tools/Magic Light Probes/Scene GUI/Disable")]
        public static void Disable()
        {
            SceneView.onSceneGUIDelegate -= OnScene;
        }

        static void OnScene(SceneView scene)
        {           
            Handles.BeginGUI();

            GUILayout.Window(2, 
                new Rect(Screen.width / EditorGUIUtility.pixelsPerPoint - 140, 
                Screen.height / EditorGUIUtility.pixelsPerPoint - 90, 100, 50), (id) =>
            {
                if (GUILayout.Button("Mark As Bottom"))
                {
                    MagicLightProbes[] sceneVolumes = FindObjectsOfType<MagicLightProbes>(); 
                    
                    if (sceneVolumes.Length == 0)
                    {
                        Disable();
                        return;
                    }

                    for (int i = 0; i < sceneVolumes.Length; i++)
                    {
                        if (Selection.objects.Length > 1)
                        {
                            for (int j = 0; j < Selection.objects.Length; j++)
                            {
                                if (!sceneVolumes[i].groundAndFloorKeywords.Contains(Selection.objects[j].name))
                                {
                                    sceneVolumes[i].groundAndFloorKeywords.Add(Selection.objects[j].name);
                                }
                            }
                        }
                        else
                        {
                            if (!sceneVolumes[i].groundAndFloorKeywords.Contains(Selection.activeGameObject.name))
                            {
                                if (Selection.activeGameObject.GetComponent<MeshRenderer>() != null || Selection.activeGameObject.GetComponent<Collider>() != null)
                                {
                                    string errorMessage = "";
                                    bool isStatic = MLPUtilites.CheckIfStatic(Selection.activeGameObject, out errorMessage);

                                    if (isStatic)
                                    {
                                    sceneVolumes[i].groundAndFloorKeywords.Add(Selection.activeGameObject.name);
                                        Debug.LogFormat("<color=yellow>MLP:</color> An object named " +
                                            "\"" + Selection.activeGameObject.name + "\" has been added to the list.");
                                    }
                                    else
                                    {
                                        if (EditorUtility.DisplayDialog("Magic Light Probes", errorMessage, "OK"))
                                        {
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EditorUtility.DisplayDialog("Magic Light Probes", "The object you have selected does not have a Collider or Mesh Renderer " +
                                        "and cannot be taken into account by the system. Add a Collider or Mesh Renderer (with mesh filter) to the object or " +
                                        "select another object.", "OK"))
                                    {
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (EditorUtility.DisplayDialog("Magic Light Probes", "This object has already " +
                                    "been added to the list.", "OK"))
                                {
                                    return;
                                }
                            }
                        }
                    }
                }

                if (GUILayout.Button("Unmark As Bottom"))
                {
                    MagicLightProbes[] sceneVolumes = FindObjectsOfType<MagicLightProbes>();

                    for (int i = 0; i < sceneVolumes.Length; i++)
                    {
                        if (Selection.objects.Length > 1)
                        {
                            for (int j = 0; j < Selection.objects.Length; j++)
                            {
                                if (sceneVolumes[i].groundAndFloorKeywords.Contains(Selection.objects[j].name))
                                {
                                    sceneVolumes[i].groundAndFloorKeywords.Remove(Selection.objects[j].name);
                                }
                            }
                        }
                        else
                        {
                            if (sceneVolumes[i].groundAndFloorKeywords.Contains(Selection.activeGameObject.name))
                            {
                                sceneVolumes[i].groundAndFloorKeywords.Remove(Selection.activeGameObject.name);
                                Debug.LogFormat("<color=yellow>MLP:</color> An object named \"" + Selection.activeGameObject.name + "\" " +
                                    "has been removed from the list.");
                            }
                            else
                            {
                                if (EditorUtility.DisplayDialog("Magic Light Probes", "This object is not listed.", "OK"))
                                {
                                    return;
                                }
                            }
                        }
                    }
                }

            }, "MLP Helper");

            

            Handles.EndGUI();
        }
    }
}
