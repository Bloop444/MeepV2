using System.Collections;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int CurrentMoney = 0;

    private void Start()
    {
        Load(); 
        StartCoroutine(AutoSave());
    }

    IEnumerator AutoSave()
    {
        while (true)
        {
            Save();
            yield return new WaitForSeconds(15);
        }
    }

    void Save()
    {
        PlayerPrefs.SetInt("CurrentMiningMoney", CurrentMoney);
        PlayerPrefs.Save();
    }

    void Load()
    {
        if (PlayerPrefs.HasKey("CurrentMiningMoney"))
        {
            CurrentMoney = PlayerPrefs.GetInt("CurrentMiningMoney");
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
