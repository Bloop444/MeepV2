using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class ConvertMoneyToPlayfab : MonoBehaviour
{
    public MoneyManager _MoneyManager;
    public TMP_Text CurrentMoney;
    public TMP_Text MoneyUWillGet;

    public string HandTag = "HandTag";
    public string CurrencyCode = "DB";

    public List<GameObject> objectsToEnable;
    public float ConvertRadius = 5f;

    private int PreviewMoney;
    private int savedMoney;

    private void Start()
    {
        LoadMoney();
        GetPlayFabCurrency();
    }

    private void Update()
    {
        CurrentMoney.text = "Current Money: " + _MoneyManager.CurrentMoney.ToString();
        PreviewMoney = Mathf.FloorToInt(_MoneyManager.CurrentMoney * 0.5f);
        MoneyUWillGet.text = "Converted Money: " + PreviewMoney.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(HandTag))
        {
            ConvertItemsInRadius();
        }
    }

    private void ConvertItemsInRadius()
    {
        Collider[] itemsToConvert = Physics.OverlapSphere(transform.position, ConvertRadius);
        int totalMoney = 0;

        foreach (Collider col in itemsToConvert)
        {
            DroppedItem droppedItem = col.GetComponent<DroppedItem>();
            if (droppedItem != null)
            {
                totalMoney += droppedItem.MoneyValue;
                Destroy(droppedItem.gameObject);
            }
        }

        if (_MoneyManager != null && totalMoney > 0)
        {
            _MoneyManager.CurrentMoney += totalMoney;
            ConvertAndSaveMoney(_MoneyManager.CurrentMoney);
        }
    }

    void ConvertAndSaveMoney(int amount)
    {
        if (amount <= 0) return;

        PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = CurrencyCode,
            Amount = amount
        },
        result =>
        {
            _MoneyManager.CurrentMoney = 0;
            savedMoney += amount;
            SaveMoney();
            EnableGameObjects();
        },
        error => Debug.LogError("Error: " + error.GenerateErrorReport()));
    }

    void GetPlayFabCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        result =>
        {
            if (result.VirtualCurrency.ContainsKey(CurrencyCode))
            {
                savedMoney = result.VirtualCurrency[CurrencyCode];
            }
        },
        error => Debug.LogError("Error retrieving PlayFab money: " + error.GenerateErrorReport()));
    }

    void SaveMoney()
    {
        PlayerPrefs.SetInt("SavedMoney", _MoneyManager.CurrentMoney);
        PlayerPrefs.Save();
    }

    void LoadMoney()
    {
        if (PlayerPrefs.HasKey("SavedMoney"))
        {
            _MoneyManager.CurrentMoney = PlayerPrefs.GetInt("SavedMoney");
        }
    }

    void EnableGameObjects()
    {
        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
