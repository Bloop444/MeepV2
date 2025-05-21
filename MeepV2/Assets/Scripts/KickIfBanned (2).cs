using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;

public class KickIfBanned : MonoBehaviour
{
    public float checkInterval = 5f;

    private void Start()
    {
        InvokeRepeating("CheckBanStatus", 0f, checkInterval);
    }

    private void CheckBanStatus()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountInfoSuccess, OnGetAccountInfoError);
    }

    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {

    }

    private void OnGetAccountInfoError(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.AccountBanned)
        {
            PhotonNetwork.Disconnect();
            Application.Quit();
        }
    }
}
