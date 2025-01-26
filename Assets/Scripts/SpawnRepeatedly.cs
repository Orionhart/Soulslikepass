using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRepeatedly : MonoBehaviour
{
    public GameObject prefabToSpawn; // The prefab to spawn
    public int numberOfPrefabs = 10; // Number of prefabs to spawn
    public float spawnInterval = 1f; // Time interval between spawns

    private void Start()
    {
        // Start the spawning process
        StartCoroutine(SpawnPrefabs());
    }

    private System.Collections.IEnumerator SpawnPrefabs()
    {
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            // Spawn the prefab
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

            // Wait for the specified interval before spawning the next prefab
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}





