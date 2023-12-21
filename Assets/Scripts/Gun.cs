using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 100;
    public float Velocity = 45;

    public Transform shell;
    public Transform eject;

    private int currentAmmo = 30;
    private bool isReloading = false;

    float nextShotTime;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isReloading)
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

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < 30)
        {
            StartCoroutine(Reload());
        }
    }

    public void Shoot()
    {
        if (Time.time > nextShotTime && !isReloading)
        {
            if (currentAmmo > 0)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile ammo = Instantiate(projectile, muzzle.position, muzzle.rotation);
                ammo.SetSpeed(Velocity);

                Instantiate(shell, eject.position, eject.rotation);

                currentAmmo--;
                Debug.Log("Ammo left: " + currentAmmo);
            }
            else
            {
                Debug.Log("Out of ammo. Reload!");
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(1f);

        currentAmmo = 30;
        isReloading = false;

        Debug.Log("Reloaded! Ammo: " + currentAmmo);
    }
}



