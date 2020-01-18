using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_StopBySpeed : MonoBehaviour {
	int stopStep = 0;
	sc_Hero hero;
	Animator anim;
	// Use this for initialization
	void Start () {
		hero = GetComponent<sc_Hero> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (hero.spd == 0f) {
			if (stopStep == 3)
				anim.SetBool ("stop", true);
			else
				stopStep++;
		} else {
			anim.SetBool ("stop", false);
			stopStep = 0;
		}
			
	}
}
