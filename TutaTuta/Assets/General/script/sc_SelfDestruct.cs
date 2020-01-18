using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_SelfDestruct : MonoBehaviour {
	public float destructTime;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, destructTime);
	}
}
