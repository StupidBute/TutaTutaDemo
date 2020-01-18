using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_BulletHole : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.Translate (-0.04f * (float)Random.Range (-2, 8), 0.04f * (float)Random.Range (-2, 8), 0f);
	}

}
