using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public string PickaxeTag = "Pickaxe";
    public int HitsBeforeBreaks = 3;
    public GameObject DropPrefab;
    public int DropMoneyValue = 5;
    public bool GiveMoney = true;

    private bool HasHit = false;
    [HideInInspector] public int CurrentHits;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PickaxeTag))
        {
            if (!HasHit)
            {
                CurrentHits += 1;
                Debug.Log("Current Hits: " + CurrentHits);
                HasHit = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PickaxeTag))
        {
            HasHit = false;
        }
    }

    private void Update()
    {
        if (CurrentHits >= HitsBeforeBreaks)
        {
            DropItem();
            Destroy(gameObject);
        }
    }

    private void DropItem()
    {
        if (DropPrefab != null)
        {
            GameObject droppedItem = Instantiate(DropPrefab, transform.position, Quaternion.identity);
            DroppedItem droppedItemScript = droppedItem.GetComponent<DroppedItem>();

            if (droppedItemScript != null)
            {
                droppedItemScript.MoneyValue = DropMoneyValue;
                droppedItemScript.GiveMoney = GiveMoney; 
            }
        }
    }
}
