using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_ShieldMan : MonoBehaviour {
	public float ShieldWeight;
	public GameObject TransformEffect0;
	public GameObject TransformEffect1;
	public bool canSpawnEffect = true;

	int moveStep = 0, spawnStep = 0;
	float originSpd;
	float originWeight;
	float originY;
	sc_Hero hero;

	Animator anim;

	void Start(){
		tag = "Tag_ShieldMan";
		hero = GetComponent<sc_Hero>();
		originSpd = hero.MaxSPD;
		originWeight = hero.Weight;
		originY = transform.position.y;
		anim = GetComponent<Animator> ();
	}

	void Update(){
		if (tag == "Untagged" && Stop ())
			CheckPassClear ();

		if (canSpawnEffect) {
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("hero21_Transform0")) {
				canSpawnEffect = false;
				Instantiate (TransformEffect0, transform.position, transform.rotation);
			}
		} else {
			spawnStep++;
			if (spawnStep >= 50) {
				spawnStep = 0;
				canSpawnEffect = true;
			}
		}
	}

	public void ShieldManHit(){
		float distance = transform.position.y - originY;
		if (hero.side == 0 && distance > 1f || hero.side == 1 && distance < -1f) {
			tag = "Untagged";
			hero.MaxSPD = 0;
			hero.Weight = ShieldWeight;
			hero.totalWeight = ShieldWeight;
			moveStep = 0;
		}
	}

	bool Stop(){
		if (hero.spd > -0.01f) {
			return true;
		} else {
			ShieldManMove ();
			return false;
		}
	}

	void CheckPassClear (){
		int layerMask = 1 << hero.enemyLayer;
		RaycastHit2D hit = Physics2D.Raycast (transform.position, hero.face * Vector2.up, 1.5f, layerMask);
		if (hit.collider == null) {
			if (moveStep >= 10)
				ShieldManMove ();
			else
				moveStep++;
		}else{
			moveStep = 0;
		}
	}

	void ShieldManMove(){
		tag = "Tag_ShieldMan";
		anim.SetTrigger("clear");
		if (hero.GM.GameState == 1)
			hero.MaxSPD = originSpd;
		else
			hero.MaxSPD = originSpd * 0.5f;
		hero.Weight = originWeight;
		Instantiate (TransformEffect1, transform.position, transform.rotation);

		if (hero.BackConnect != null)
			hero.totalWeight = originWeight + hero.BackConnect.GetComponent<sc_Hero> ().totalWeight;
		else
			hero.totalWeight = originWeight;
	}
}
