using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiEnableDisableTimer : MonoBehaviour
{
    public string Tag = "HandTag";
    public List<GameObject> Object;
    public float Delay;
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(Tag))
        {
            StartCoroutine(EnableDisable());
        }
    }

    IEnumerator EnableDisable()
    {
        foreach (GameObject obj in Object)
        {
            obj.SetActive(true);
        }
        yield return new WaitForSeconds(Delay);
        foreach (GameObject obj in Object)
        {
            obj.SetActive(false);
        }
    }
}