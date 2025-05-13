using UnityEngine;

public class DisableOnLoad : MonoBehaviour
{
    [Header("THIS SCRIPT WAS MADE BY FIZZY NOT YOU, PLEASE GIVE CREDIT")]
    [Space()]
    public GameObject[] Maps;
    public GameObject ForestMap;

    private void Start()
    {
        foreach (GameObject Obj in Maps)
        {
            Obj.SetActive(false);
        }

        ForestMap.SetActive(true);
    }
}
