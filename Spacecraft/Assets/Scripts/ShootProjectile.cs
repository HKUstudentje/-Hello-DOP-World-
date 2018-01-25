using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour {
    public GameObject projectile;
    public GameObject explosion;
    public Transform projectileSpawner;
    public Transform cameraTransform;
    public float raycastLength;

    public int currentProjectile;

    public List<ProjectileScript> projectileList = new List<ProjectileScript>();
    public List<ProjectileScript> disabledProjectileList = new List<ProjectileScript>();
    public List<GameObject> explosionList = new List<GameObject>();

    public ProjectileScript psc;

    void Start ()
    {
        psc = projectile.GetComponent<ProjectileScript>();
        // Notes(tim): Detect pre-existing projectiles in the scene and move them to the disabledProjectileList?
        // Setting each projectile in the disabledProjectileList on inactive using foreach. (Not yet possible right now, it contains only projectile scripts.)
    }
	
	void Update ()
    {
        //Put the ProjectileScript into variable.
        // Notes(gb): Don't do this every tick: the same script is needed each tick, 
        // so scope this higher than the Update function (= as class variable)
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, raycastLength);
            // Checking if the hit object has a rigidbody.(Better than collider, doesn't depend on (no) parents)
            if (hit.rigidbody != null)
            {
                // Put the Spacecraft script into variable for easier use.
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
                        //Instead of instantiate use setactive projectile and move to spawnpoint and projectile list!
                        GameObject newProjectile = Instantiate(projectile, projectileSpawner.position, projectileSpawner.rotation);
                        projectileList.Add(newProjectile.GetComponent<ProjectileScript>());
                        newProjectile.GetComponent<Rigidbody>().velocity = transform.forward * psc.projectileSpeed;
                        //Determining which projectile in the list this is, -1 because the list starts at 0.
                        currentProjectile = projectileList.Count - 1;
                        projectileList[currentProjectile].timeWhenExplodes = Time.time + projectileList[currentProjectile].secondsToLive;
                    }
                }
            }
        }
        // Set int i to the amount of projectiles at the beginning of the loop; for as long as i is bigger then 0 (as long as there are projectiles) the loop plays; i - 1 at the end of the loop;
        for (int i = projectileList.Count; i > 0; i--)
        {
            if (Time.time > projectileList[i - 1].timeWhenExplodes)
            {
                Instantiate(explosion, projectileList[i - 1].transform.position, Quaternion.identity);
                disabledProjectileList.Add(projectileList[i - 1]);
                projectileList.RemoveAt(i - 1);
                // Disable projectile. But I need the gameObject for that, and the list is made up from scripts.
            }
        }
    }
}
