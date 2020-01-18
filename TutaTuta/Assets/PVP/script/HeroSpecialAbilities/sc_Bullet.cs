using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Bullet : MonoBehaviour {
	public sc_PVPGod GM;
	public float speed;
	public float ATK;
	public int face;
	public int enemyLayer;
	public GameObject LaunchEffect;
	public GameObject DestroyEffect;

	bool hittingTarget = false;
	int layerMask;
	RaycastHit2D Hit;
	// Update is called once per frame
	void Start(){
		enemyLayer = face == 1 ? 9 : 8;
		layerMask = 1 << enemyLayer;

		if (ATK > 50f)
			SpawnLaunchEffect ();
	}

	void Update () {

		if (hittingTarget)
			DestroyBullet ();
		else
			transform.Translate (0, speed * Time.deltaTime, 0);
		
		if (transform.position.y < -4.5f || transform.position.y > 4.5f) {
			if (ATK > 50f) {
				//is rocket
				if (face == 1)
					GM.DamageCastle (1);
				else
					GM.DamageCastle (0);
			}

			Destroy (gameObject);
		}
			
		CheckHit ();

	}

	void SpawnLaunchEffect (){
		Instantiate (LaunchEffect, new Vector2(transform.position.x, -4*face), transform.rotation);
	}

	void CheckHit(){
		Hit = Physics2D.Raycast (transform.position, face * Vector2.up, speed * Time.deltaTime, layerMask);
		if (Hit.collider != null)
			hittingTarget = true;
	}

	void DestroyBullet(){
		transform.position = Hit.point;
		sc_Hero hit = Hit.collider.GetComponent<sc_Hero> ();
		if (hit != null) {
			hit.Damaged (ATK);
			if(ATK > 50f)
				Instantiate (DestroyEffect, Hit.point, transform.rotation);
			else
				Instantiate (DestroyEffect, Hit.collider.transform.position, Hit.collider.transform.rotation, Hit.collider.transform);
		}
		Destroy (gameObject);
	}
}
