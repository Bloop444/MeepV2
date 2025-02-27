using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public int MoneyValue = 5;
    public bool GiveMoney = true; 

    private void Start()
    {
        if (!GiveMoney)
        {
            MoneyValue = 0; 
        }
    }
}
