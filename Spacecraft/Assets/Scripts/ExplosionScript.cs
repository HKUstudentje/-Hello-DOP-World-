using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {

    public float timeTilDestroy;

	void Start () {
        StartCoroutine(TimeTilDestroy());
	}

    IEnumerator TimeTilDestroy()
    {
        yield return new WaitForSeconds(timeTilDestroy);
        DestroyImmediate(this.gameObject);
        yield return null;
    }
}
