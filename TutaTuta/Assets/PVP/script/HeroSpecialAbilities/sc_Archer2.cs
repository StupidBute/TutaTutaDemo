using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Archer2 : MonoBehaviour {
	public GameObject MyBullet;
	public int shootCycle;
	public float shootATK;
	public float bulletSpeed;
	public float bulletOffset;

	sc_Hero hero;
	Animator anim;
	//Collider2D PreHit;
	int ShootStep = 5;
	int face;
	int enemyLayer;
	// Use this for initialization
	void Start () {
		hero = GetComponent<sc_Hero> ();
		anim = GetComponent<Animator> ();
		face = hero.face;
		enemyLayer = hero.enemyLayer;
	}


	void Update () {
		int enemyState = CheckEnemy ();

		switch (enemyState) {
		case 0:								//no enemy ahead
			anim.SetBool ("near", false);
			break;

		case 1:									//enemy at far or have allies ahead
			anim.SetBool ("near", false);
			if (ShootStep == 0) {
				Shoot ();
				ShootStep = shootCycle;
			} else {
				ShootStep--;
			}
			
			break;
		case 2:									//enemy close
			anim.SetBool("near", true);
			break;
		}
	}

	int CheckEnemy(){
		int layerMask = 1 << enemyLayer;
		RaycastHit2D enemy = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + hero.face*hero.ConnectLength), hero.face * Vector2.up, 8f, layerMask);
		if (enemy.collider == null)
			return 0;
		else if (enemy.distance >= 0.8f)
			return 1;
		else {
			int layerMask2 = 1 << gameObject.layer;
			RaycastHit2D allies = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + hero.face*hero.ConnectLength), hero.face * Vector2.up, 0.8f, layerMask2);
			if (allies.collider != null)
				return 1;
			else
				return 2;
		}
	}

	void Shoot(){
		anim.SetTrigger ("shoot");
		GameObject Inst = Instantiate (MyBullet, new Vector2 (transform.position.x, transform.position.y + face * bulletOffset), transform.rotation);
		sc_Bullet bullet = Inst.GetComponent<sc_Bullet> ();
		bullet.speed = bulletSpeed;
		bullet.ATK = shootATK;
		bullet.face = face;
	}



}