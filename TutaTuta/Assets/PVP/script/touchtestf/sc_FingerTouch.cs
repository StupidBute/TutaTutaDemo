using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_FingerTouch : MonoBehaviour {
	public static bool TouchMode = false;
	bool touchReturn = false;
	sc_PVPGod GM;
	sc_SoundManager GM_Sound;
	Camera cam;

	void Start(){
		cam = Camera.main;
		GM = GetComponent<sc_PVPGod> ();
		GM_Sound = GM.GM_Sound;
	}

	void Update () {
		if (GM.GameState == 1 || GM.GameState == 10) {
			if (TouchMode) {
				for (int i = 0; i < Input.touchCount; i++)
					CheckTouch (i);
			} else {
				CheckMouse ();
			}
		} else {
			if (GM.Mode != 0)
				CheckReturn ();
		}

	}

	void CheckMouse(){
		if (Input.GetKeyDown (KeyCode.P))
			Debug.Break ();

		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = TouchedButton (Input.mousePosition);
			if (hit.collider != null) {
				if (hit.collider.tag == "Tag_HeroIcon") {
					GM_Sound.PlaySoundEffect (0, true);
					hit.collider.GetComponent<sc_ButtonClick> ().TouchHeroIcon ();
				}else if (hit.collider.tag == "Tag_Return") {
					GM_Sound.PlaySoundEffect (0, true);
					touchReturn = true;
					hit.collider.GetComponent<sc_Return> ().Touched(true);
				}
			}


		}else if(Input.GetMouseButton(0)){
			if (touchReturn) {
				RaycastHit2D hit = TouchedButton (Input.mousePosition);
				if (hit.collider != null && hit.collider.tag == "Tag_Return")
					hit.collider.GetComponent<sc_Return> ().Touched(false);
			}

		}else if (Input.GetMouseButtonUp (0)) {
			touchReturn = false;
			RaycastHit2D hit = TouchedButton (Input.mousePosition);
			if (hit.collider != null && hit.collider.tag == "Tag_Road") {
				//GM_Sound.PlaySoundEffect (1, true);
				hit.collider.GetComponent<sc_RoadCreate> ().TouchRoad();
			}


		}
	}

	void CheckTouch(int num){
		if (Input.touches [num].phase == TouchPhase.Began) {
			RaycastHit2D hit = TouchedButton (Input.touches [num].position);
			if (hit.collider != null) {
				if (hit.collider.tag == "Tag_HeroIcon") {
					GM_Sound.PlaySoundEffect (0, true);
					hit.collider.GetComponent<sc_ButtonClick> ().TouchHeroIcon ();
				}else if (hit.collider.tag == "Tag_Return") {
					GM_Sound.PlaySoundEffect (0, true);
					touchReturn = true;
					hit.collider.GetComponent<sc_Return> ().Touched(true);
				}
			}

			
		}else if(Input.touches[num].phase == TouchPhase.Stationary || Input.touches[num].phase == TouchPhase.Moved){
			if (touchReturn) {
				RaycastHit2D hit = TouchedButton (Input.touches [num].position);
				if (hit.collider != null && hit.collider.tag == "Tag_Return")
					hit.collider.GetComponent<sc_Return> ().Touched(false);
			}

		}else if (Input.touches [num].phase == TouchPhase.Ended || Input.touches [num].phase == TouchPhase.Canceled) {
			touchReturn = false;
			RaycastHit2D hit = TouchedButton (Input.touches [num].position);
			if (hit.collider != null && hit.collider.tag == "Tag_Road") {
				//GM_Sound.PlaySoundEffect (1, true);
				hit.collider.GetComponent<sc_RoadCreate> ().TouchRoad();
			}
				
			
		}
	}

	void CheckReturn(){
		if (TouchMode) {
			if (Input.touchCount > 0 && Input.touches [0].phase == TouchPhase.Began) {
				RaycastHit2D hit = TouchedButton (Input.touches [0].position);
				if (hit.collider != null && hit.collider.tag == "Tag_Return") {
					GM_Sound.PlaySoundEffect (0, true);
					touchReturn = true;
					hit.collider.GetComponent<sc_Return> ().Touched(true);
				}
			}
		} else {
			if (Input.GetMouseButtonDown(0)) {
				RaycastHit2D hit = TouchedButton (Input.mousePosition);
				if (hit.collider != null && hit.collider.tag == "Tag_Return") {
					GM_Sound.PlaySoundEffect (0, true);
					touchReturn = true;
					hit.collider.GetComponent<sc_Return> ().Touched(true);
				}
			}
		}
	}

	RaycastHit2D TouchedButton(Vector2 Pos){
		Ray ray = cam.ScreenPointToRay (Pos);
		int layerMask = 1 << 10;
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, 10, layerMask);
		return hit;
	}
}
	