using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    public AudioSource fistaudio;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fistaudio.Play();
            Debug.Log("Hitting");
            Player player = other.gameObject.GetComponent<Player>();
            player.damage(20);
        }
    }
}
