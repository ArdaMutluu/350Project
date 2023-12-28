using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float movementSpeed = 12f;
    public Cam cameraObject;
    public static int maxHealth = 130;
    public static int health;
    private const float dashDelay = 0.8f;
    private float currentDashDelay = dashDelay;
    private bool dashing = false;
    private const float dashTime = 0.2f;
    private float currentDashTime = 0f;
    private const float dashSpeed = 40f;
    private Vector3 dashDir = Vector3.zero;
    public float knockbackForce = 150f;
    public float knockbackDuration = 2f;
    private float knockbackTimer = 0f;
    private Vector3 knockbackDir;
    public GameObject dasheffect;
    

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        cameraObject.centerCameraOnPlayer(0);
    }

    void Update()
    {
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

        if (Input.GetMouseButton(1) && currentDashDelay >= dashDelay)
        {
            dash();
        }

        if (dashing)
        {
            if (currentDashTime < dashTime)
            {
                // Use velocity for movement during dashing
                rb.velocity = dashDir * dashSpeed * getSpeedMultiplier();
                currentDashTime += Time.deltaTime;
                dasheffect.SetActive(true);
            }
            else
            {
                dashing = false;
                rb.velocity = Vector3.zero; // Stop the player when dash is complete
                dasheffect.SetActive(false);
            }
        }
        else
        {
            // Only allow movement when not being knocked back
            if (knockbackTimer <= 0)
            {
                HandlePlayerMovement();
            }
        }

        // Handle knockback
        if (knockbackTimer > 0)
        {
            rb.velocity = knockbackDir * knockbackForce;
            knockbackTimer -= Time.deltaTime;
        }

        currentDashDelay += Time.deltaTime;
    }

    void HandlePlayerMovement()
    {
        Vector3 movementVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        
        float smoothInputX = Mathf.Lerp(rb.velocity.x, movementVector.x * movementSpeed * getSpeedMultiplier(), 0.1f);
        float smoothInputZ = Mathf.Lerp(rb.velocity.z, movementVector.z * movementSpeed * getSpeedMultiplier(), 0.1f);
        rb.velocity = new Vector3(smoothInputX, rb.velocity.y, smoothInputZ);
    }

    public void damage(int dmg)
    {
        health -= dmg;
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

    // New method for applying knockback with a specified direction
    public void ApplyKnockback(Vector3 collisionNormal)
    {
        knockbackDir = -collisionNormal;
        knockbackTimer = knockbackDuration;
        rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);
    }

 /*   public void SetPlayerState(PlayerState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;

            // Handle animations based on the state
            switch (newState)
            {
                case PlayerState.Idle:
                    anim.SetBool("isRun", false);
                    anim.SetBool("isIdle", true);
                    break;
                case PlayerState.Walk:
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isRun", true);
                    break;
                case PlayerState.Fire:
                    GetComponent<Animator>().Play("Fire");
                    break;
            }
        }
    }*/

}
