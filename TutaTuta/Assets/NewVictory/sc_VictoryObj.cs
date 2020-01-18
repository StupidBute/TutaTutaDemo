using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_VictoryObj : MonoBehaviour {
	public sc_PVPGod GM;
	public Sprite[] CampLogo = new Sprite[4];
	int canTouch = 0;
	float t = 0f;
	string sceneName;

	void Start () {
		transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().sprite = CampLogo [GM.winningCamp];
		/*
		if (GM.Mode == 0)
			sceneName = "Scene_PVPMenu";
		else if (GM.Mode == 1)
			sceneName = "Scene_Start";
		else
			sceneName = "Scene_PVEMenu";*/
		
		sceneName = "Scene_PVP";	//for demo
		StartCoroutine (WinSound(0.3f));
	}


	void Update () {
		if(canTouch == 0){
			t += Time.deltaTime;
			if (t > 1.5f)
				canTouch = 1;
		}else if (canTouch == 1) {
			if (sc_FingerTouch.TouchMode) {
				if (Input.touchCount != 0 && Input.touches [0].phase == TouchPhase.Began) {
					StartCoroutine (GM.ChangeScene (sceneName));
					canTouch = 2;
				}
					
			} else {
				if (Input.GetMouseButtonDown (0)) {
					StartCoroutine (GM.ChangeScene (sceneName));
					canTouch = 2;
				}
					
			}
		}
	}

	IEnumerator WinSound(float _t){
		yield return new WaitForSeconds (_t);
		GM.GM_Sound.PlaySoundEffect (1, true);
	}
}
