using Photon.Pun;
using Oculus.Platform;
using Oculus.Platform.Models;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;
using System.Threading.Tasks;

public static class ACRunner
{
    private static ACSettings Settings;
    private static bool ChecksPassed = true;
    private static bool LibraryCheckPassed = true;
    private static bool LogFileCheckPassed = true;
    private static bool HarmonyCheckPassed = true;
    private static bool DLLCheckPassed = true;
    private static bool DEXCheckPassed = true;
    private static bool CustomCheckPassed = true;
    private static bool OculusAuthPassed = false;
    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        Settings = Resources.Load<ACSettings>("Settings");
        if (Settings == null)
        {
            Debug.LogError("ACSettings not found! Ensure it's in a Resources folder.");
            return;
        }

        if (Settings.OculusAuth)
            Authenticate();

        if (Settings.AntiDLLInjection)
            Begin();

        MonitorStatus();
        DetectLibraries();
        CheckForDEXs();
        RunCustomChecks();
        CheckForFile("funchook.log", ref LogFileCheckPassed);
        CheckForFile("0Harmony.dll", ref HarmonyCheckPassed);
        CheckForDLLs();
    }
    private static void Begin()
    {
        if (UnityEngine.Application.platform == RuntimePlatform.Android)
        {
            string Path = $"/storage/emulated/0/Android/data/{UnityEngine.Application.identifier}/files";
            string[] AllDlls = Directory.GetFiles(Path, "*.dll", SearchOption.AllDirectories);

            foreach (string Dll in AllDlls)
            {
                if (!Dll.Contains("/il2cpp/") && !Dll.Contains("/cache/"))
                {
                    File.Delete(Dll);
                    Utils.ForceCrash(ForcedCrashCategory.Abort);
                    UnityEngine.Application.Quit();
                }
            }
        }
    }
    private static void Authenticate()
    {
        Core.AsyncInitialize().OnComplete(c =>
        {
            Entitlements.IsUserEntitledToApplication().OnComplete(callback =>
            {
                if (callback.IsError)
                {
                    Debug.LogError("Meta Platform entitlement error: " + callback.GetError());
                    FailedOculusAuth(Settings.BanDetailsOnFailedAuth);
                    return;
                }
                Users.GetLoggedInUser().OnComplete(m =>
                {
                    if (!m.IsError && m.Type == Message.MessageType.User_GetLoggedInUser)
                    {
                        Users.GetUserProof().OnComplete(r =>
                        {
                            if (r.IsError)
                            {
                                Debug.LogError("User proof retrieval error.");
                                FailedOculusAuth(Settings.BanDetailsOnFailedAuth);
                                return;
                            }

                            Debug.Log("User authenticated successfully in PlayFab.");
                            OculusAuthPassed = true;
                        });
                    }
                    else
                    {
                        Debug.LogError("Failed to retrieve Oculus user data.");
                        FailedOculusAuth(Settings.BanDetailsOnFailedAuth);
                    }
                });
            });
        });
    }
    private async static void FailedOculusAuth(ACSettings.BanDetails banDetails)
    {
        if (banDetails.ShouldSend)
        {
            int interval = 500;
            int Counter = 0;

            while (!PlayFabClientAPI.IsClientLoggedIn() && Counter < Settings.WaitTime)
            {
                await Task.Delay(500);
                Counter += 500;
            }

            if (!PlayFabClientAPI.IsClientLoggedIn())
            {
                if (Settings.KickFromServerOnFailedAuth)
                    PhotonNetwork.LeaveRoom();

                if (Settings.ExitGamOnFailedAuth)
                    ForceExit();
                return;
            }

            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
            {
                FunctionName = "banPlayer",
                FunctionParameter = new
                {
                    playerId = PlayFabSettings.staticPlayer.PlayFabId,
                    duration = banDetails.BanTime,
                    reason = banDetails.BanReason,
                    sendwhook = banDetails.SendWebhook,
                    banuser = banDetails.ShouldBan
                },
                GeneratePlayStreamEvent = true
            },
            result =>
            {
                if (Settings.KickFromServerOnFailedAuth)
                    PhotonNetwork.LeaveRoom();

                if (Settings.ExitGamOnFailedAuth)
                    ForceExit();
            },
            error =>
            {
                if (Settings.KickFromServerOnFailedAuth)
                    PhotonNetwork.LeaveRoom();

                if (Settings.ExitGamOnFailedAuth)
                    ForceExit();
            });
        }
        else
        {
            if (Settings.KickFromServerOnFailedAuth)
                PhotonNetwork.LeaveRoom();

            if (Settings.ExitGamOnFailedAuth)
                ForceExit();
        }
    }
    private static void DetectLibraries()
    {
        if (Settings == null) return;
        var librarySet = new HashSet<string>(Settings.CheckedLibraries);
        string[] files = Directory.GetFiles(UnityEngine.Application.persistentDataPath, "*", SearchOption.AllDirectories);

        if (files.Any(file => librarySet.Any(lib => Path.GetFileName(file).Contains(lib))))
        {
            ChecksPassed = false;
            LibraryCheckPassed = false;
            TriggerDetection(Settings.BanDetailsOnFailedModCheck);
        }
        else
        {
            LibraryCheckPassed = true;
        }
    }
    private static void CheckForFile(string fileName, ref bool checkPassed)
    {
        if (Settings == null) return;

        string[] files = Directory.GetFiles(UnityEngine.Application.persistentDataPath, fileName, SearchOption.AllDirectories);
        checkPassed = files.Length == 0;

        if (!checkPassed)
        {
            ChecksPassed = false;
            TriggerDetection(Settings.BanDetailsOnFailedModCheck);
        }
    }
    private static void CheckForDLLs()
    {
        if (Settings == null) return;

        var bypassSet = new HashSet<string>(Settings.CheckedDLLs);
        var files = Directory.EnumerateFiles(UnityEngine.Application.persistentDataPath, "*.dll", SearchOption.AllDirectories);

        if (files.Any(file => !bypassSet.Contains(Path.GetFileName(file))))
        {
            DLLCheckPassed = false;
            ChecksPassed = false;
            TriggerDetection(Settings.BanDetailsOnFailedModCheck);
        }
        else
        {
            DLLCheckPassed = true;
        }
    }
    private static void CheckForDEXs()
    {
        if (Settings == null) return;

        var bypassSet = new HashSet<string>(Settings.CheckedDEXs ?? new List<string>());
        string[] files = Directory.GetFiles(UnityEngine.Application.persistentDataPath, "*.dex", SearchOption.AllDirectories);

        if (files.Any(file => !bypassSet.Contains(Path.GetFileName(file))))
        {
            DEXCheckPassed = false;
            ChecksPassed = false;
            TriggerDetection(Settings.BanDetailsOnFailedModCheck);
        }
        else
        {
            DEXCheckPassed = true;
        }
    }
    private static void RunCustomChecks()
    {
        if (Settings == null || Settings.CustomChecks == null) return;

        var checkSet = new HashSet<string>(Settings.CustomChecks);
        string[] files = Directory.GetFiles(UnityEngine.Application.persistentDataPath, "*", SearchOption.AllDirectories);
        string[] folders = Directory.GetDirectories(UnityEngine.Application.persistentDataPath, "*", SearchOption.AllDirectories);

        if (files.Concat(folders).Any(item => checkSet.Contains(Path.GetFileName(item))))
        {
            CustomCheckPassed = false;
            ChecksPassed = false;
            TriggerDetection(Settings.BanDetailsOnFailedModCheck);
        }
        else
        {
            CustomCheckPassed = true;
        }
    }
    public static IEnumerator MonitorStatus()
    {
        while (true)
        {
            if (Settings == null)
            {
                Debug.LogError("Settings not initialized!");
                yield break;
            }

            yield return new WaitForSeconds(Settings.DetectionInterval);

            if (CustomCheckPassed && DEXCheckPassed && DLLCheckPassed && HarmonyCheckPassed && LibraryCheckPassed && !LogFileCheckPassed && OculusAuthPassed)
                if (!ChecksPassed)
                    TriggerDetection(Settings.BanDetailsOnFailedModCheck);

            foreach (var badChar in Settings.BadCharacters)
            {
                if (PhotonNetwork.LocalPlayer.NickName.Contains(badChar) ||
                    PhotonNetwork.LocalPlayer.NickName.Length > Settings.MaxUsernameLength)
                {
                    TriggerDetection(Settings.BanDetailsOnInvalidUsername);

                    yield break;
                }
            }
        }
    }
    private static void TriggerDetection(ACSettings.BanDetails banDetails)
    {
        if (banDetails.ShouldSend)
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
            {
                FunctionName = "banPlayer",
                FunctionParameter = new
                {
                    playerId = PlayFabSettings.staticPlayer.PlayFabId,
                    duration = banDetails.BanTime,
                    reason = banDetails.BanReason,
                    sendwhook = banDetails.SendWebhook,
                    banuser = banDetails.ShouldBan
                },
                GeneratePlayStreamEvent = true
            },
            result =>
            {
                if (Settings.KickOnDetection)
                    PhotonNetwork.LeaveRoom();

                if (Settings.ExitGameOnBadName)
                    ForceExit();
            },
            error =>
            {
                if (Settings.KickOnDetection)
                    PhotonNetwork.LeaveRoom();

                if (Settings.ExitGameOnBadName)
                    ForceExit();
            });
        }
        else
        {
            if (Settings.KickOnDetection)
                PhotonNetwork.LeaveRoom();

            if (Settings.ExitGameOnBadName)
                ForceExit();
        }
    }
    private static void ForceExit()
    {
        Utils.ForceCrash(ForcedCrashCategory.Abort);
        UnityEngine.Application.Quit();
    }
}