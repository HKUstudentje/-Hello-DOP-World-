using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {
    public float reloadModifier;
    public Rigidbody spaceCraftGO;
    public Spacecraft sc;
    public bool powerUpActivate;

    private void Start()
    {
        sc = spaceCraftGO.GetComponent<Spacecraft>();
        powerUpActivate = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(sc != null)
        {
            powerUpActivate = true;
        }
    }
}
