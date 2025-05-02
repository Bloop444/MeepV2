using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSwitching : MonoBehaviour
{
    public GameObject[] Tabs;

    public int CurrentIndex = 0;

    public string Tag = "FC";

    private bool CanPress = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag))
        {
            Tabs[CurrentIndex].SetActive(false);

            CurrentIndex = (CurrentIndex + 1) % Tabs.Length;

            if(CurrentIndex == Tabs.Length)
            {
                CurrentIndex = 0;
            }

            Tabs[CurrentIndex].SetActive(true);

            CanPress = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(Tag))
        {
            CanPress = true;
        }
    }
}
