using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_CreateSmoke : MonoBehaviour {
	public GameObject Smoke;
	public int SpawnCycle;

	int spawnStep = 0;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		spawnStep++;
		if (spawnStep == SpawnCycle) {
			spawnStep = 0;
			Instantiate (Smoke, transform.position, Quaternion.identity);
		}
	}
}
