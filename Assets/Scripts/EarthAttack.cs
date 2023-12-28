using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthAttack : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hitting ea ");
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.ApplyKnockback(other.contacts[0].normal);
                player.damage(15);
                Debug.Log("Applied Knockback");
            }
        }
    }
}
