using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    public GameObject explosion;
    public float timeWhenExplodes;
    public float projectileSpeed;
    public float secondsToLive;

	// Use this for initialization
	/*void Start ()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;
        timeWhenExplodes = Time.time + secondsToLive;
        //Wat is precies het verschil tussen het gebruiken van velocity en addforce(meer physics based)?
        //Velocity is constant, addforce is meer duwen(verschil in snelheid)
        //StartCoroutine(SpawnExplosion());
    }

    void Update()
    {
        if (Time.time > timeWhenExplodes)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            DestroyImmediate(gameObject);
        }
    }

    IEnumerator SpawnExplosion()
    {
        yield return new WaitForSeconds(timeTilExplode);
        Instantiate(explosion, this.transform.position, Quaternion.identity);
        //Hoe geef ik de explosie zijn eigen vector 3?
        DestroyImmediate(this.gameObject);
        yield return null;
        //Is de yield return null wel nodig als je het gehele gameObject gelijk delete voor de volgende frame?
    }*/
}
