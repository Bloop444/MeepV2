using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestTeleport : MonoBehaviour
{
    [Header("Credits: TheCoder")]

    [Header("Gorilla Player")]
    public GameObject gorillaPlayer;

    [Header("Location")]
    public GameObject teleportLocation;

    [Header("Hand Tag(s)")]
    public string rHand = "HandTag";
    public string lHand = "HandTag";

    private Collider[] objectsWithColliders;

    void Start()
    {
        objectsWithColliders = FindObjectsOfType<Collider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(rHand) || other.CompareTag(lHand))
        {
            StartCoroutine(Teleport());
        }
    }

    IEnumerator Teleport()
    {
        gorillaPlayer.GetComponent<Rigidbody>().isKinematic = true;
        foreach (Collider collider in objectsWithColliders)
        {
            collider.enabled = false;
        }
        gorillaPlayer.transform.position = teleportLocation.transform.position;
        yield return new WaitForSeconds(1.0f);
        foreach (Collider collider in objectsWithColliders)
        {
            collider.enabled = true;
        }
        gorillaPlayer.GetComponent<Rigidbody>().isKinematic = false;
    }
}
