using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 10;

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }
    

    void Update () {
        transform.Translate (Vector3.forward * Time.deltaTime * speed);
        
      
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
            return;
        }


        if (other.gameObject.tag == "Enemy")
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.damage(15);
            Destroy(gameObject);
        }
        
        if (other.gameObject.tag == "Boss")
        {
            Boss boss = other.gameObject.GetComponent<Boss>();
            boss.damage(2);
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CloseEnemy")
        {
            CloseEnemy enemy = other.gameObject.GetComponent<CloseEnemy>();
            enemy.damage(10);
            Destroy(gameObject);
        }
        
        
    }
}
