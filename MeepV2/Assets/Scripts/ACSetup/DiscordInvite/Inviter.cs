#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Inviter : EditorWindow
{
    static Inviter()
    {
        if (!EditorPrefs.HasKey("FirstTimeWithAnti"))
        {
            EditorPrefs.SetBool("FirstTimeWithAnti", true);
            EditorApplication.delayCall += ShowWindow;
        }
    }
    [MenuItem("Tools/Anticheat Updates")]
    public static void ShowWindow()
    {
        GetWindow<Inviter>("Anticheat Updates");
    }
    private void OnGUI()
    {
        GUILayout.Label("Hello", EditorStyles.boldLabel);
        GUILayout.Label("Thank you for buying this asset.", EditorStyles.wordWrappedLabel);
        GUILayout.Label("Join the Discord here for updates (Make sure to make a ticket with proof of purchase):", EditorStyles.wordWrappedLabel);

        if (GUILayout.Button("Join Discord", GUILayout.Height(30))) Application.OpenURL("https://discord.gg/W7WGp8tS6k");
        if (GUILayout.Button("Close", GUILayout.Height(20))) Close();

        GUILayout.Label("How to set up:", EditorStyles.wordWrappedLabel);
        GUILayout.Label("Go to the settings, and modify the settings as you please. Then, make sure the cloudscript is in playfab", EditorStyles.wordWrappedLabel);
        GUILayout.Label("Any issues, message thatonegorilla", EditorStyles.wordWrappedLabel);
    }
}
#endif