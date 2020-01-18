using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_DestroyBeam : MonoBehaviour {
	public GameObject MyBeam;
	public GameObject BeamBlast;
	public float ScaleRatio = 1.6f;
	public float ATK;
	public int HitCycle = 5;


	RaycastHit2D Hit;
	Collider2D PreHit = null;
	float xScale = 0.05f, yScale = 0f;
	float ScaleSpeed = 0.03f;
	sc_Hero hero;
	//bool HitTarget = false;
	int HitStep;

	void Start(){
		MyBeam.transform.localScale = new Vector2 (xScale, yScale);
		hero = GetComponent<sc_Hero> ();
		HitStep = 0;
	}

	void Update(){
		
		if (xScale < 0.65f) {
			xScale += ScaleSpeed;
			ScaleSpeed += 0.01f;
		}

		int layerMask = 3 << 8;
		Hit = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y + hero.face * hero.ConnectLength), hero.face * Vector2.up, 8, layerMask);
		if (Hit.collider != null) {
			if (Hit.collider != PreHit) {
				PreHit = Hit.collider;
				HitStep = 0;
			}

			yScale = Hit.distance * ScaleRatio;
			BeamBlast.transform.position = new Vector2 (transform.position.x, MyBeam.transform.position.y + hero.face * Hit.distance);

		} else {
			PreHit = null;
			if(yScale < 14f)
				yScale += 0.8f;
			BeamBlast.transform.position = new Vector2 (-10f, 0f);
		}

		MyBeam.transform.localScale = new Vector2 (xScale, yScale);




		if (PreHit != null) {
			if (hero.GM.GameState == 1) {
				if (HitStep == HitCycle) {
					if (Hit.collider.tag == "Tag_Beamer" || Hit.collider.tag == "Tag_SuperBeamer")
						Hit.collider.GetComponent<sc_SuperBeamer> ().HitByBeam ();
					else
						Hit.collider.GetComponent<sc_Hero> ().Damaged (ATK);

					HitStep = 0;

				} else {
					HitStep++;

				}
			}

		}
	}
}
