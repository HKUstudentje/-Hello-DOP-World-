using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviour {
    public GameObject projectileGO;
    public GameObject explosionGO;
    public GameObject spaceCraftGO;
    public GameObject powerUpGO;

    public Transform projectileSpawner;
    public Transform cameraTransform;

    public int currentProjectile;
    public int projectileAmount;

    public List<ProjectileScript> projectileList = new List<ProjectileScript>();
    public List<GameObject> explosionList = new List<GameObject>();
    public List<PowerUpScript> powerUpList = new List<PowerUpScript>();

    public ProjectileScript projectileScript;
    public Spacecraft spaceCraftScript;
    public PowerUpScript powerUpScript;

    public Rigidbody spaceCraftRB;

    void Start ()
    {
        spaceCraftScript = spaceCraftGO.GetComponent<Spacecraft>();
        spaceCraftRB = spaceCraftGO.GetComponent<Rigidbody>();

        powerUpScript = powerUpGO.GetComponent<PowerUpScript>();

        powerUpList.AddRange(FindObjectsOfType<PowerUpScript>());

        for ( int i = projectileAmount; i > 0; i--)
        {
            GameObject newProjectile = Instantiate(projectileGO, projectileSpawner.position, projectileSpawner.rotation);
            newProjectile.SetActive(false);
        }
    }

    void Update ()
    {
        // Notes(gb): Make sure to take deltaTime into account, because of time-independent movement.
        // look a little into what deltaTime is, and why it's used.
        float dt = Time.deltaTime;

        if(Input.GetKey(KeyCode.W))
        {
            Vector3 force = spaceCraftGO.transform.forward * spaceCraftScript.spaceCraftSpeed * dt;
            spaceCraftRB.AddForceAtPosition(force, transform.position, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Vector3 force = spaceCraftGO.transform.forward * -spaceCraftScript.spaceCraftSpeed * dt;
            spaceCraftRB.AddForceAtPosition(force, transform.position, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Vector3 rotation = new Vector3( 0, 1, 0 ) * spaceCraftScript.spaceCraftRotateSpeed * dt;
            spaceCraftGO.transform.Rotate(rotation);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Vector3 rotation = new Vector3( 0, -1, 0 ) * spaceCraftScript.spaceCraftRotateSpeed * dt;
            spaceCraftGO.transform.Rotate(rotation);
        }

        for (int i = powerUpList.Count; i > 0; i--)
        {
            PowerUpScript powerUp = powerUpList[i - 1];
            if (powerUp.powerUpActivate)
            {
                Debug.Log("1");
                if (powerUp.powerUpCollision.gameObject.GetComponent<Spacecraft>() != null)
                {
                    Debug.Log("2");
                    spaceCraftScript.shootCooldown *= powerUp.reloadModifier;
                    powerUp.powerUpActivate = false;
                    powerUp.gameObject.SetActive(false);
                }    
            }
        }


            // Note(Tim): Shooting is op dit moment nog 'broken', het omzetten naar object pooling is nog niet helemaal gelukt.

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
                        projectileGO.GetComponent<Rigidbody>().velocity = transform.forward * projectileScript.projectileSpeed;
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
