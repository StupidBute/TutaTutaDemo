using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_ButtonAnim : MonoBehaviour {

	SpriteRenderer spr;
	Color bright, dark;

	public float originScale = 1f;
	public float alpha;
	float scale = 1f;
	float scaleSpeed = 1f, colorSpeed = 1f, speedAcc = 0.1f;
	float t = 0f;
	int state = 0;
	bool fadeIn = false;
	bool anim0 = false, anim1 = false, anim2 = false, anim3 = false;
	//bool locking = false, IE_break = false;
	//0.pop up		1.bounce		2.press & out		3.disappear

	void Awake () {
		spr = GetComponent<SpriteRenderer> ();
		bright = dark = spr.color;
		bright.a = alpha;
		dark.a = 0f;

	}

	void Update () {
		//test
		/*
		if (Input.GetKeyDown (KeyCode.Q))
			PopUp (12f, -1f);
		if (Input.GetKeyDown (KeyCode.W))
			Bounce (4f, -0.8f, 1.1f);
		if (Input.GetKeyDown (KeyCode.E))
			PressOut (4f, -0.5f);
		if (Input.GetKeyDown (KeyCode.R))
			Fade(false, 5f);
		*/

		if (anim0)
			ButtonAnim0 ();

		if (anim1)
			ButtonAnim1 ();

		if (anim2)
			ButtonAnim2 ();

		if (anim3)
			ButtonAnim3 ();
		
	}

#region 進行函式
	void ButtonAnim0(){
		scale += scaleSpeed * 0.02f;
		if (scaleSpeed < 0f && scale <= 1f) {
			scale = 1f;
			anim0 = false;
		} else {
			scaleSpeed += speedAcc;
		}
		float finalScale = scale * originScale;
		transform.localScale = new Vector2 (finalScale, finalScale);
	}

	void ButtonAnim1(){
		scale += scaleSpeed * 0.02f;
		if (state == 0) {
			if (scale < 1) {
				scaleSpeed *= 0.6f;
				speedAcc *= 1.1f;
				state = 1;
			}else
				scaleSpeed += speedAcc;
		} else {
			if (scale >= 1) {
				scale = 1;
				anim1 = false;
			}else
				scaleSpeed -= speedAcc;
		}

		float finalScale = scale * originScale;
		transform.localScale = new Vector2 (finalScale, finalScale);
	}

	void ButtonAnim2(){
		scale += scaleSpeed * 0.02f;
		if (scale <= 0) {
			scale = 0;
			anim2 = false;
		} else {
			scaleSpeed += speedAcc;
		}
		float finalScale = scale * originScale;
		transform.localScale = new Vector2 (finalScale, finalScale);
	}

	void ButtonAnim3(){
		t += colorSpeed * 0.02f;
		if (fadeIn) {
			if (t >= 1f) {
				spr.color = bright;
				anim3 = false;
			}else
				spr.color = Color.Lerp (dark, bright, t);
		} else {
			if (t >= 1f) {
				spr.color = dark;
				anim3 = false;
			}else
				spr.color = Color.Lerp (bright, dark, t);
		}

	}
#endregion

#region 初始函式
	public void PopUp(float spd, float spdAcc){
		anim0 = true;
		spr.color = bright;
		scaleSpeed = spd;
		speedAcc = spdAcc;
		scale = 0f;
		transform.localScale = Vector2.zero;
	}

	public void Bounce(float spd, float spdAcc, float firstScale){
		anim1 = true;
		scaleSpeed = spd;
		speedAcc = spdAcc;
		scale = firstScale;
		state = 0;
	}

	public void PressOut(float spd, float spdAcc){
		anim2 = true;
		scaleSpeed = spd;
		speedAcc = spdAcc;
		scale = 1.05f;
	}

	public void Fade(bool isFadeIn, float spd){
		scale = 1f;
		transform.localScale = Vector2.one * originScale;
		anim3 = true;
		t = 0f;
		fadeIn = isFadeIn;
		colorSpeed = spd;
		if (fadeIn)
			spr.color = dark;
		else
			spr.color = bright;
	}

	public void PressBtn(){
		StartCoroutine (IE_PressBtn ());
	}

#endregion

	IEnumerator IE_PressBtn(){
		float pressScale = 0.8f;							//按下時的大小比例
		Vector2 _origin = new Vector2(originScale, originScale);
		Vector2 targetScale = pressScale * _origin;			//按下後的大小

		float _t = 0f;

		//階段1: 按下
		while (_t < 1f) {
			_t += Time.deltaTime * 20f;
			transform.localScale = Vector2.Lerp (_origin, targetScale, _t);
			spr.color = Color.Lerp (Color.white, new Color (pressScale, pressScale, pressScale, 1f), _t);
			yield return null;
		}

		//階段2:彈出
		float _scale = pressScale, _spd = 4.5f, _spdAcc = -0.8f;
		while (_spd > 0 || _scale > 1f) {
			_scale += _spd * Time.deltaTime;
			_spd += _spdAcc;
			float _realScale = originScale * _scale;
			float _rgb = Mathf.Clamp01 (_scale);
			transform.localScale = new Vector2 (_realScale, _realScale);
			spr.color = new Color (_rgb, _rgb, _rgb, 1f);
			yield return null;
		}
		transform.localScale = _origin;
		spr.color = Color.white;
	}

}
