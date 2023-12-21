using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public GameObject[] enemies;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ActivateEnemies());
        }
    }
    
    IEnumerator ActivateEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null) 
            {
                enemy.SetActive(true);
                yield return new WaitForSeconds(1f); 
            }
        }
    }
}
