using System.Collections;
using UnityEngine;

public class SpawnLimitedObjects : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public Transform[] spawnPoints;
    public int spawnCount = 1; 

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
            for (int i = 0; i < spawnCount; i++)
            {
                
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                
                GameObject objToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

                
                Instantiate(objToSpawn, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}
