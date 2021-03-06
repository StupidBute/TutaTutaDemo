using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cooldowntime : MonoBehaviour {
	public GameObject God;
	sc_PVPGod GM;
	int playerChoose;

	public int side;
	public int num;
	public int MaxNum;
	public float counter;

	bool activate = true;
	float i = 0;
	Image btn, cir;
	Color origin = new Color(0f, 0f, 0f, 0.384f);

	void Start () {
		btn = transform.GetChild(0).GetComponent<Image> ();
		btn.fillAmount = 0;
		cir = transform.GetChild(1).GetComponent<Image>();
		cir.fillAmount = 1;

		GM = God.GetComponent<sc_PVPGod> ();
		playerChoose = sc_PVPGod.playerChoose [side];

		MaxNum = GM.MaxNum [playerChoose, num];
		counter = GM.CDTime [playerChoose, num];
	}
	
	// Update is called once per frame
	void Update () {
		if (GM.GameState == 1) {
			if (GM.CurrentNum [side, num] < MaxNum && activate)
				RunCounter ();
			else
				StopRunning ();
				
		} else {
			if (GM.CurrentNum [side, num] < 0) {
				StopRunning ();
				GM.CurrentNum [side, num] = 0;
			}
		}

	}

	void RunCounter(){
		if (i < counter){
			float amount = i / counter;
			if (GM.CurrentNum [side, num] == 0) {
				btn.fillAmount = 1f - amount;
			}
			cir.fillAmount = amount;

			i += Time.deltaTime;
		} else if (i >= counter){
			i = 0;
			GM.CurrentNum[side, num] ++;
		}

	}

	void StopRunning(){
		i = 0;
		btn.fillAmount = 0f;
		cir.fillAmount = 1f;
	}

	public void ActivateCD(bool _activate){
		activate = _activate;
	}

	public IEnumerator Flash(){
		btn.fillAmount = 1f;
		float t = 0f;
		while (t < 1f) {
			if (GM.CurrentNum [side, num] == 0) {
				btn.color = origin;
				yield break;
			}
			t += Time.deltaTime * 3f;
			btn.color = Color.Lerp (origin, Color.clear, t);
			yield return null;
		}
		btn.color = Color.clear;
	}

}
