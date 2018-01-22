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
        ProjectileScript psc = projectile.GetComponent<ProjectileScript>();

        //When clicking the mouse.
        if (Input.GetMouseButtonDown(0))
        {
            //An raycast will be shot from the camera.
            RaycastHit hit;
            Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, raycastLength);
            //Checking if the hit object has a rigidbody.(Better then collider, doesn't depend on (no) parents)
            if (hit.rigidbody != null)
            {
                //Put the Spacecraft script into variable for easier use.
                Spacecraft sc = hit.rigidbody.GetComponent<Spacecraft>();
                //Checking if the hit object has the Spacecraft script on it (instead of tags).
                if (sc != null)
                {
                    //Checking if there is no cooldown going on.(Don't use coroutines for cooldowns, keep it simple and all in a row).
                    if(Time.time > sc.timeTilWeCanShoot)
                    {
                        //Setting the cooldown until we can shoot again.
                        sc.timeTilWeCanShoot = Time.time + sc.shootCooldown;
                        //Instantiating the projectile and adding it to the list.
                        projectileList.Add((GameObject)Instantiate(projectile, projectileSpawner.position, projectileSpawner.rotation));
                        //Determining which projectile in the list this is, -1 because the list starts at 0.
                        currentProjectile = projectileList.Count - 1;
                        //Giving the projectile its velocity.
                        projectileList[currentProjectile].GetComponent<Rigidbody>().velocity = transform.forward * psc.projectileSpeed;
                        //Setting the time until explosion for each individual projectile.
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
