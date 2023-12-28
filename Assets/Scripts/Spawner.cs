using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class Spawner : MonoBehaviour
{

    public GameObject levelLoader;
    
       [System.Serializable]
    public class Wave
    {
        public List<GameObject> enemyPrefabs; 
        public int numberOfEnemies; 
    }

    public List<Wave> waves; 

    public float minX = -33; 
    public float maxX = -10;
    public float minZ = 4;  
    public float maxZ = 17; 
    public float y = -3;    

    public float waveTimeout = 3f; 

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        for (int waveIndex = 0; waveIndex < waves.Count; waveIndex++)
        {
            Wave currentWave = waves[waveIndex];

            // Spawn enemies for the current wave
            for (int i = 0; i < currentWave.numberOfEnemies; i++)
            {
                GameObject enemyPrefab = currentWave.enemyPrefabs[i % currentWave.enemyPrefabs.Count];
                SpawnEnemy(enemyPrefab);
                yield return new WaitForSeconds(2f);
            }

           
            yield return new WaitUntil(() => AllEnemiesDefeated());
            
            yield return new WaitForSeconds(waveTimeout);
        }
        levelLoader.SetActive(true);
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
