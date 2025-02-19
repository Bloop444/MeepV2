using System.Collections;
using UnityEngine;

public class DeleteFromHierarchy : MonoBehaviour
{
    public GameObject objectToDelete; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag")) 
        {
            StartCoroutine(DeleteAfterDelay());
        }
    }

    private IEnumerator DeleteAfterDelay()
    {
        yield return new WaitForSeconds(1); 
        if (objectToDelete != null)
        {
            Destroy(objectToDelete); 
        }
    }
}
