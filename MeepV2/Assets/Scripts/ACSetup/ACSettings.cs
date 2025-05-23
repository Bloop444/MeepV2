using System;
using System.Collections.Generic;
using UnityEngine;

// ================================
// Anti-Cheat
// ================================
// Hello! Thank you for buying the anticheat. Please do not share this asset.
// ================================

[CreateAssetMenu(fileName = "ACSettings", menuName = "Settings/ACSettings")]
public class ACSettings : ScriptableObject
{
    [Header("Player Name Restrictions")]
    [Tooltip("Maximum allowed username length.")]
    [SerializeField] public int MaxUsernameLength = 16;

    [Tooltip("List of characters that are not allowed in player names.")]
    [SerializeField] public List<string> BadCharacters = new List<string>();

    [Tooltip("Kick the player if their name contains a restricted character.")]
    [SerializeField] public bool KickOnBadName = true;

    [Tooltip("Exit the game if a player's name is invalid.")]
    [SerializeField] public bool ExitGameOnBadName = true;

    [Header("Detection Timing")]
    [Tooltip("Time interval (in seconds) between cheat detection scans.")]
    [SerializeField] public int DetectionInterval = 16;

    [Header("Cheat Detection")]
    [Tooltip("Kick players if a cheat is detected.")]
    [SerializeField] public bool KickOnDetection = true;

    [Tooltip("List of suspicious libraries to check for in the APK.")]
    [SerializeField] public string[] CheckedLibraries = { "bootstrap", "funchook", "dobby", "capstone" };

    [Tooltip("List of DLL files that should not be present.")]
    [SerializeField] public List<string> CheckedDLLs = new List<string> { "mscorlib.dll-resources.dat" };

    [Tooltip("List of DEX files that should not be present.")]
    [SerializeField] public List<string> CheckedDEXs = new List<string>();

    [Tooltip("Custom files or folders to check for cheat detection.")]
    [SerializeField] public List<string> CustomChecks = new List<string>();

    [Header("Oculus Auth")]
    [Tooltip("Use oculus auth.")]
    [SerializeField] public bool OculusAuth = true;

    [Tooltip("Kick the player if their name contains a restricted character.")]
    [SerializeField] public bool KickFromServerOnFailedAuth = true;

    [Tooltip("Exit the game if a player's name is invalid.")]
    [SerializeField] public bool ExitGamOnFailedAuth = true;

    [Header("Banning (RISKY)")]
    [Header("If auth fails for an unknown reason, then it can cause the player to be banned, even though they may have not caused this by cheating. This can be enabled, just use caution.")]
    [Tooltip("How long to wait for playfab to login to ban the player. 0 seconds is to just skip.")]
    [SerializeField] public float WaitTime = 5000;
    [Tooltip("ban details if they failed auth.")]
    [SerializeField] public BanDetails BanDetailsOnFailedAuth;
    [Tooltip("ban details if they have been detected for mods.")]
    [SerializeField] public BanDetails BanDetailsOnFailedModCheck;
    [Tooltip("Ban details for invalid username.")]
    [SerializeField] public BanDetails BanDetailsOnInvalidUsername;

    [Header("Experimental")]
    [Tooltip("Prevents injection.")]
    [SerializeField] public bool AntiDLLInjection;

    [Serializable]
    public class BanDetails
    {
        [Tooltip("Should send details to cloudscript at all.")]
        [SerializeField] public bool ShouldSend;
        [Tooltip("Should ban the player")]
        [SerializeField] public bool ShouldBan;
        [Tooltip("How long the ban should be")]
        [SerializeField] public int BanTime;
        [Tooltip("Ban reason")]
        [SerializeField] public string BanReason;
        [Tooltip("Should send a webhook to alert you on what has happened.")]
        [SerializeField] public bool SendWebhook = true;
    }
}
