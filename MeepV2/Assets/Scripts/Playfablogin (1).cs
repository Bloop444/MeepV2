using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using TMPro;

public class Playfablogin : MonoBehaviour
{
    public static Playfablogin instance;
    public string MyPlayFabID;
    public string CatalogName;
    public List<GameObject> specialitems;
    public List<GameObject> disableitems;

    [System.Serializable]
    public class Currency
    {
        public string CurrencyCode;
        public string CurrencyName;
        public int Balance;
        public TextMeshPro CurrencyText;
    }

    public List<Currency> Currencies = new List<Currency>();

    public string bannedscenename;
    public TextMeshPro MOTDText;
    public TextMeshPro UserName;
    public string StartingUsername;
    public string name;
    [SerializeField] public bool UpdateName;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        login();
    }

    public void login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
                GetUserVirtualCurrency = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    public void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Logging in");
        GetAccountInfoRequest InfoRequest = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(InfoRequest, AccountInfoSuccess, OnError);
        GetVirtualCurrencies();
        GetMOTD();
    }

    public void AccountInfoSuccess(GetAccountInfoResult result)
    {
        MyPlayFabID = result.AccountInfo.PlayFabId;
    }

    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
    }

    void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        foreach (var currency in Currencies)
        {
            if (result.VirtualCurrency.ContainsKey(currency.CurrencyCode))
            {
                currency.Balance = result.VirtualCurrency[currency.CurrencyCode];
                currency.CurrencyText.text = "You have " + currency.Balance.ToString() + " " + currency.CurrencyName;
            }
        }
    }

    private void OnError(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.AccountBanned)
        {
            SceneManager.LoadScene(bannedscenename);
        }
    }

    public void GetMOTD()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), MOTDGot, OnError);
    }

    public void MOTDGot(GetTitleDataResult result)
    {
        if (result.Data == null || !result.Data.ContainsKey("MOTD"))
        {
            Debug.Log("No MOTD");
            return;
        }
        MOTDText.text = result.Data["MOTD"];
    }
}
