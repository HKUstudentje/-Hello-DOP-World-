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

    public List<GameObject> projectileList = new List<GameObject>();
    public List<GameObject> explosionList = new List<GameObject>();

    void Start ()
    {

	}
	
	void Update ()
    {
        //Put the ProjectileScript into variable.
        // Notes(gb): Don't do this every tick: the same script is needed each tick, 
        // so scope this higher than the Update function (= as class variable)
        ProjectileScript psc = projectile.GetComponent<ProjectileScript>();

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, raycastLength);
            //Checking if the hit object has a rigidbody.(Better than collider, doesn't depend on (no) parents)
            if (hit.rigidbody != null)
            {
                //Put the Spacecraft script into variable for easier use.
                // Notes(gb): more importantly: you grab the object only once,
                // and from then on use a locally scoped variable, which saves a lot of CPU power.
                Spacecraft sc = hit.rigidbody.GetComponent<Spacecraft>();
                
                if (sc != null)
                {
                    //Checking if there is no cooldown going on.(Don't use coroutines for cooldowns, keep it simple and all in a row).
                    if (Time.time > sc.timeTilWeCanShoot)
                    {
  
                        sc.timeTilWeCanShoot = Time.time + sc.shootCooldown;
                        projectileList.Add((GameObject)Instantiate(projectile, projectileSpawner.position, projectileSpawner.rotation));
                        currentProjectile = projectileList.Count - 1;
                        projectileList[currentProjectile].GetComponent<Rigidbody>().velocity = transform.forward * psc.projectileSpeed;
                        projectileList[currentProjectile].GetComponent<ProjectileScript>().timeWhenExplodes = Time.time + projectileList[currentProjectile].GetComponent<ProjectileScript>().secondsToLive;
                        Debug.Log("Spawned & set time");
                    }
                }
            }
        }
        //For every projectile in the projectileList...
        foreach(GameObject projectile in projectileList)
        {
            //...we check if it is time to explode...
            if(Time.time > projectile.GetComponent<ProjectileScript>().timeWhenExplodes)
            {
                Debug.Log("Boom");
                //and we instantiate an explosion at the projectile location.
                Instantiate(explosion, projectile.transform.position, Quaternion.identity);
                Destroy(projectile.gameObject);
            }
        }
    }
}
