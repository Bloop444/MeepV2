using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerTabs : MonoBehaviour
{
    public List<GameObject> DisablingTabs = new List<GameObject>();
    public GameObject EnablingTab;
    
    public void OnTriggerEnter()
    {

        foreach (var Dtab in DisablingTabs)
        {
            Dtab.SetActive(false);
        }
        
        EnablingTab.SetActive(true);


    }
}
