using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_ButtonClick : MonoBehaviour {
	public GameObject God;
	public int side;
	public int buttonNum;

	bool clicking = false;
	float scale = 0.72f;
	SpriteRenderer spr, sprMagic;
	sc_PVPGod GM;

	void Start () {
		spr = GetComponent<SpriteRenderer> ();
		GM = God.GetComponent<sc_PVPGod> ();

		if (GM.Mode != 1)
			spr.sprite = GM.ButtonImg [3 * sc_PVPGod.playerChoose [side] + buttonNum];
		
		if (buttonNum == 2)
			sprMagic = transform.GetChild (2).gameObject.GetComponent<SpriteRenderer> ();
	}

	void Update(){
		#region 電腦操作
		if (buttonNum == 0 && Input.GetKeyDown (KeyCode.RightControl)) {
			int _r = Random.Range(0, 16);
			int _num = 0;
			if(_r < 7)
				_num = 0;
			else if(_r < 12)
				_num = 1;
			else
				_num = 2;
			sc_RoadCreate.createnum [side] = _num;
		}
		#endregion

		if (!clicking) {
			if (GM.CurrentNum [side, buttonNum] == 0) {
				ChangeButtonColor (new Color (0.6f, 0.6f, 0.6f, 1));

				if (scale > 0.6701f){
					scale -= Time.deltaTime;
					if (scale < 0.67f)
						scale = 0.67f;
				}

			}else if(sc_RoadCreate.createnum[side] == buttonNum){
				ChangeButtonColor (new Color (1, 1, 1, 1));

				if (scale < 0.7499f){
					scale += Time.deltaTime;
					if (scale > 0.75f)
						scale = 0.75f;
				}
			}else{
				ChangeButtonColor (new Color (1, 1, 1, 1));

				if (scale < 0.7199f) {
					scale += Time.deltaTime;
					if (scale > 0.72f)
						scale = 0.72f;
				} else {
					scale = 0.72f;
				}
			}

			transform.localScale = new Vector2 (scale, scale);
		}
	}

	void ChangeButtonColor(Color _c){
		spr.color = _c;
		if (buttonNum == 2)
			sprMagic.color = _c;
	}

	public void TouchHeroIcon(){
		if (GM.Mode == 0 || GM.Mode == 2) {
			sc_RoadCreate.createnum [side] = buttonNum;
			if (!clicking && GM.CurrentNum [side, buttonNum] > 0)
				StartCoroutine (ButtonClicking ());
		} else if (GM.Mode == 1) {
			if (GM.GameState == 1 || GM.GameState == 10 && GM.CurrentNum [0, buttonNum] != 0) {
				sc_RoadCreate.createnum [side] = buttonNum;
				if (!clicking && GM.CurrentNum [side, buttonNum] > 0)
					StartCoroutine (ButtonClicking ());
			}
		}

	}

	IEnumerator ButtonClicking(){
		clicking = true;
		transform.localScale = new Vector2 (0.77f, 0.77f);

		float scaleSpeed = 4f;
		float speedAcc = -0.8f;

		while (!(scaleSpeed < 0f && scale <= 0.75f)) {
			scale += scaleSpeed * Time.deltaTime;
			transform.localScale = new Vector2 (scale, scale);
			scaleSpeed += speedAcc;
			yield return null;
		}
		transform.localScale = new Vector2 (0.75f, 0.75f);
		clicking = false;
	}

}
