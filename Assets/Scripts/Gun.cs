using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 100;
    public float Velocity = 45;
    public GameObject muzzleflash;
    public AudioSource gunSound;
    public AudioClip gunfiresound;
    public AudioSource reloadSound;
    public Transform shell;
    public Transform eject;
    private int currentAmmo = 30;
    private bool isReloading = false;
    float nextShotTime;

    public void Start()
    {
        GameObject gunObject = GameObject.Find("Fire1");
        
    }


    private void Update()
    {
        if (Input.GetButton("Fire1") && !isReloading)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(Reload());
            }
        }
        else
        {
            muzzleflash.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < 30)
        {
            StartCoroutine(Reload());
        }
    }

    public void Shoot()
    {
        Debug.Log("Shoot");
        if (Time.time > nextShotTime && !isReloading)
        {
            muzzleflash.SetActive(true);
            gunSound.PlayOneShot(gunfiresound);
            nextShotTime = Time.time + msBetweenShots / 1000;
            Projectile ammo = Instantiate(projectile, muzzle.position, muzzle.rotation);
            ammo.SetSpeed(Velocity);
            Instantiate(shell, eject.position, eject.rotation);

            currentAmmo--;
            Debug.Log("Ammo left: " + currentAmmo);
        }
    }
    
    
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        reloadSound.Play();
        yield return new WaitForSeconds(1f);

        currentAmmo = 30;
        isReloading = false;

        Debug.Log("Reloaded! Ammo: " + currentAmmo);
    }
}



