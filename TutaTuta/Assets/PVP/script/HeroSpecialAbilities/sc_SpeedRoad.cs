using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_SpeedRoad : MonoBehaviour {
	bool Destroyed = false;
	Animator anim;
	ParticleSystem par;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		par = GetComponentInChildren<ParticleSystem> ();
	}

	public void DestroyRoad(){
		if (!Destroyed) {
			StartCoroutine (DestroyCoroutine());
			Destroyed = true;
		}

	}

	IEnumerator DestroyCoroutine(){
		anim.SetTrigger ("end");
		par.Stop (false, ParticleSystemStopBehavior.StopEmitting);
		yield return new WaitForSeconds (2f);
		Destroy (gameObject);
	}
}
