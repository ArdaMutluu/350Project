using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessSpawner : MonoBehaviour
{
    public GameObject player; 

    public List<GameObject> enemyPrefabs; 

    public float minX = -19; 
    public float maxX = 14; 
    public float minZ = -4;   
    public float maxZ =1;  
    public float y = 4;   

    public float waveTimeout = 3f;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        int startingEnemyCount = 5;
        int currentEnemyCount = startingEnemyCount;

        while (player != null) 
        {
            // Spawn enemies for the current wave
            for (int i = 0; i < currentEnemyCount; i++)
            {
                GameObject enemyPrefab = GetRandomEnemyPrefab();
                SpawnEnemy(enemyPrefab);
                yield return new WaitForSeconds(1.5f);
            }

            // Wait until all enemies in the current wave are defeated
            yield return new WaitUntil(() => AllEnemiesDefeated());

            // Wait for the specified timeout between waves
            yield return new WaitForSeconds(waveTimeout);

            // Increase the enemy count for the next wave by 2
            currentEnemyCount += 2;
        }
        
    }

    GameObject GetRandomEnemyPrefab()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), y, Random.Range(minZ, maxZ));
        Quaternion spawnRotation = Quaternion.identity;
        Instantiate(enemyPrefab, spawnPosition, spawnRotation);
    }

    bool AllEnemiesDefeated()
    {
        GameObject[] closeEnemies = GameObject.FindGameObjectsWithTag("CloseEnemy");
        GameObject[] regularEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        return closeEnemies.Length == 0 && regularEnemies.Length == 0;
    }
}
