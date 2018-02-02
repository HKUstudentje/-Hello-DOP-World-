using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {
    public float reloadModifier;
    public Collider powerUpCollision;
    public bool powerUpActivate;

    private void Start()
    {
        powerUpActivate = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        powerUpCollision = other;
        powerUpActivate = true;
    }
}
