﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour {
    public GameObject projectileGO;
    public GameObject explosionGO;
    public Transform projectileSpawner;
    public Transform cameraTransform;

    public int currentProjectile;
    public int projectileAmount;

    public List<ProjectileScript> projectileList = new List<ProjectileScript>();
    //public List<ProjectileScript> disabledProjectileList = new List<ProjectileScript>();
    public List<GameObject> explosionList = new List<GameObject>();

    public ProjectileScript psc;

    void Start ()
    {
        psc = projectileGO.GetComponent<ProjectileScript>();
        for( int i = projectileAmount; i > 0; i--)
        {
            GameObject newProjectile = Instantiate(projectileGO, projectileSpawner.position, projectileSpawner.rotation);
            newProjectile.SetActive(false);
        }
    }

    void Update ()
    {
        //Put the ProjectileScript into variable.
        // Notes(gb): Don't do this every tick: the same script is needed each tick, 
        // so scope this higher than the Update function (= as class variable)
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity);
            // Checking if the hit object has a rigidbody.(Better than collider, doesn't depend on (no) parents)
            if (hit.rigidbody != null)
            {
                // Notes(gb): more importantly: you grab the object only once,
                // and from then on use a locally scoped variable, which saves a lot of CPU power.
                Spacecraft sc = hit.rigidbody.GetComponent<Spacecraft>();
                
                if (sc != null)
                {
                    //Checking if there is no cooldown going on.(Don't use coroutines for cooldowns, keep it simple and all in a row).
                    if (Time.time > sc.timeTilWeCanShoot)
                    {
                        //Setting the cooldown until we can shoot again.
                        sc.timeTilWeCanShoot = Time.time + sc.shootCooldown;
                        projectileList.Add(projectileGO.GetComponent<ProjectileScript>());
                        projectileGO.SetActive(true);
                        projectileGO.GetComponent<Rigidbody>().velocity = transform.forward * psc.projectileSpeed;
                        currentProjectile = projectileList.Count - 1;
                        projectileList[currentProjectile].timeWhenExplodes = Time.time + projectileList[currentProjectile].secondsToLive;
                    }
                }
            }
        }
        // Set int i to the amount of projectiles at the beginning of the loop; for as long as i is bigger then 0 (as long as there are projectiles) the loop plays; i - 1 at the end of the loop;
        for (int i = projectileList.Count; i > 0; i--)
        {
            ProjectileScript projectile = projectileList[i - 1];
            if (Time.time > projectile.timeWhenExplodes)
            {
                Instantiate(explosionGO, projectile.transform.position, Quaternion.identity);
                projectile.gameObject.SetActive(false);
                projectileList.Remove(projectile);
                //Destroy(projectile.gameObject);
            }
        }
    }
}
