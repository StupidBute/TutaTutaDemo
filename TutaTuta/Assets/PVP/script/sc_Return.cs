using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Return : MonoBehaviour {
	
	public static float timer;
	public GameObject God;
	public GameObject otherReturn;
	public int side;

	SpriteRenderer spr;
	sc_PVPGod GM;
	sc_Return other_scReturn;
	ParticleSystem myPar;

	bool activated = false;
	bool ChangeScene = false;
	bool clicking = false;
	float scale = 0.6f;

	void Awake(){
		timer = -1f;
	}

	void Start () {
		GM = God.GetComponent<sc_PVPGod> ();
		spr = GetComponent<SpriteRenderer> ();
		myPar = transform.GetChild(0).GetComponent<ParticleSystem>();
		myPar.Stop (false, ParticleSystemStopBehavior.StopEmittingAndClear);

		if(GM.Mode == 0)
			other_scReturn = otherReturn.GetComponent<sc_Return> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GM.Mode == 0) {
			if (activated)
				spr.color = new Color (0.7f, 0.7f, 0.7f, 1);
			else
				spr.color = new Color (1, 1, 1, 1);

			if (!ChangeScene) {
				if (timer > -0.01f) {
					timer -= Time.deltaTime;
				} else if(timer > -0.5f){
					timer = -1f;
					StartEmit (false);
					other_scReturn.StartEmit (false);
				}
			}
		}

	}

	public void Touched(bool anim){
		//還沒觸發換scene功能 才進行點擊返回鍵行為
		if(!ChangeScene){
			if (!activated) {																//若此按鈕還沒被沒有被觸發
				activated = true;
				if (GM.Mode == 0) {
					if (timer > 0f) {															//若timer已被另一按鈕觸發，則換scene
						ChangeScene = true;
						StartEmit (false);
						GM.GameState = 3;
						//StartCoroutine (GM.ChangeScene ("Scene_PVPMenu"));
						StartCoroutine (GM.ChangeScene ("Scene_PVP"));
					} else {																	//若timer還沒被觸發，則在此處設為2secs
						timer = 2f;
						other_scReturn.StartEmit (true);
					}
				}
				/*
				else if (GM.Mode == 2) {
					GM.GameState = 3;
					StartCoroutine (GM.ChangeScene ("Scene_PVEMenu"));
				} else {
					GM.GameState = 3;
					StartCoroutine (GM.ChangeScene ("Scene_Start"));
				}*/

			} else {																		//若此按鈕已被觸發，則再將timer設為2secs
				timer = 2f;
			}
		}
		if (anim && !clicking)
			StartCoroutine (ButtonClicking ());
	}

	public void StartEmit(bool _start){
		if (_start) {
			myPar.Play (false);
		}else{
			activated = false;
			myPar.Stop (false, ParticleSystemStopBehavior.StopEmitting);
		}
			
	}

	IEnumerator ButtonClicking(){
		clicking = true;
		transform.localScale = new Vector2 (0.7f, 0.7f);

		float scaleSpeed = 4.5f;
		float speedAcc = -0.8f;

		while (!(scaleSpeed < 0f && scale <= 0.6f)) {
			scale += scaleSpeed * Time.deltaTime;
			transform.localScale = new Vector2 (scale, scale);
			scaleSpeed += speedAcc;
			yield return null;
		}
		transform.localScale = new Vector2 (0.6f, 0.6f);
		clicking = false;
	}
}
