using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float movementSpeed = 10f;

    public Cam cameraObject;
    private int maxHealth = 100;
    public int health;
    private const float dashDelay = 0.8f;
    private float currentDashDelay = dashDelay;
    private bool dashing = false;
    private const float dashTime = 0.185f;
    private float currentDashTime = 0f;
    private const float dashSpeed = 38f;
    private Vector3 dashDir = Vector3.zero;
    GunController gunController;
    

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        cameraObject.centerCameraOnPlayer(0);
        gunController = GetComponent<GunController>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            gunController.Shoot();
        }
        
        
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDistance = 0.0f;
        if (playerPlane.Raycast(ray, out hitDistance))
        {
            Vector3 targetPoint = ray.GetPoint(hitDistance);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);
        }

        Vector3 oldPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (Input.GetMouseButton(1) && currentDashDelay >= dashDelay)
        {
            dash();
        }

        if (dashing)
        {
            if (currentDashTime < dashTime)
            {
                float slices = 6;
                for (int i = 0; i < slices; i++)
                {
                    float movMultiplier = dashSpeed * getSpeedMultiplier() * Time.deltaTime / slices;
                    Vector3 destination = transform.position;
                    destination.x += dashDir.x * movMultiplier;
                    destination.z += dashDir.z * movMultiplier;
                    bool intersection = Physics.Linecast(transform.position, destination);
                    if (intersection)
                    {
                        currentDashTime = dashTime;
                        break;
                    };
                    transform.position = destination;
                }
                currentDashTime += Time.deltaTime;

            }
            else
            {
                dashing = false;
            }
        }
        else
        {
            Vector3 position = transform.position;
            Vector3 movementVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            position.z += movementVector.z * movementSpeed * getSpeedMultiplier() * Time.deltaTime;
            position.x += movementVector.x * movementSpeed * getSpeedMultiplier() * Time.deltaTime;
            transform.position = position;
            
        }
        
        currentDashDelay += Time.deltaTime;
    }

    public void damage(int dmg)
    {
       
        health-=dmg;
        cameraObject.shake();
        if (health <= 0)
        {
            die();
        }

    }


    void die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void dash()
    {
        float zMovement = Input.GetAxisRaw("Vertical");
        float xMovement = Input.GetAxisRaw("Horizontal");
        if (zMovement == 0 && xMovement == 0) return;
        dashDir = new Vector3(xMovement, 0, zMovement).normalized;
        dashing = true;
        currentDashDelay = 0;
        currentDashTime = 0;
    }
    

   

    float getSpeedMultiplier()
    {
         return 1;
    }

    public bool isDead()
    {
        return health <= 0;
    }
}
