using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_ArrowHint : MonoBehaviour {
	public sc_PVPGod GM;
	public int type = 0;	//0:Side0		1:Side1
	ParticleSystem par;
	Ray[] CheckRays = new Ray[2];
	bool activated = true;

	void Start () {
		par = transform.GetChild (0).GetComponent<ParticleSystem> ();
		CheckRays [0] = new Ray (new Vector2 (-3f, -3.65f), Vector2.right);
		CheckRays [1] = new Ray (new Vector2 (-3f, 3.65f), Vector2.right);
	}

	void Update () {
		if (activated)
			CheckSpawn();
	}

	void CheckSpawn(){
		int layerMask;
		layerMask = (type == 0)? (1 << 8) : (1 << 9);
		RaycastHit2D hit = Physics2D.Raycast (CheckRays[type].origin, CheckRays[type].direction, 6f, layerMask);
		if (hit.collider != null) {
			StartCoroutine (StopHint ());
			activated = false;
		} else if (sc_PVPGod.playerChoose [type] == 2) {					//飛彈
			if (GM.CurrentNum [type, 2] < GM.MaxNum [2, 2]) {
				StartCoroutine (StopHint ());
				activated = false;
			}
		}
	}

	IEnumerator StopHint(){
		par.Stop (true, ParticleSystemStopBehavior.StopEmitting);
		yield return new WaitForSeconds (0.8f);
		Destroy (gameObject);
	}
}
