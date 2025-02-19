using System.Collections;
using UnityEngine;

public class SpawnOnTrigger : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public Transform[] spawnPoints; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag")) 
        {
            StartCoroutine(SpawnObjects());
        }
    }

    private IEnumerator SpawnObjects()
    {
        yield return new WaitForSeconds(1);

        if (objectsToSpawn.Length > 0 && spawnPoints.Length > 0)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
 
                GameObject objToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
                Instantiate(objToSpawn, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}
