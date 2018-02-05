using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {
    public float reloadModifier;
    public Collider CollidedWith;

    private void OnTriggerEnter(Collider other)
    {
        CollidedWith = other;
    }
}
