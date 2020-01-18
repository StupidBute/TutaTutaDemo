using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_Lerp : MonoBehaviour {

	Color startColor, endColor;
	Vector2 targetPos;

	float moveSpeed = 0.3f;
	float fadeSpeed = 0.3f;

	public bool moving = false, fading = false;

	float t0, t1;
	SpriteRenderer spr;

	// Use this for initialization
	void Awake () {
		spr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (moving) {
			if (t0 < 1f) {
				t0 += moveSpeed * Time.deltaTime;
				transform.position = Vector2.Lerp (transform.position, targetPos, t0);
			} else {
				transform.position = targetPos;
				moving = false;
			}
		}

		if (fading) {
			if (t1 < 1f) {
				t1 += fadeSpeed * Time.deltaTime;
				spr.color = Color.Lerp (startColor, endColor, t1);
			} else {
				spr.color = endColor;
				fading = false;
			}
		}

	}

	public Vector2 GetPos(){
		return (Vector2)transform.position;
	}

	public void SetPos(Vector2 Pos){
		transform.position = Pos;
	}

	public void StartMoving(Vector2 target, float speed){
		moving = true;
		targetPos = target;
		moveSpeed = speed;
		t0 = 0f;
	}

	public void StartFading(bool fadeIn, float speed){
		fading = true;
		fadeSpeed = speed;
		Color c_tmp = spr.color;
		if (fadeIn) {
			c_tmp.a = 0;
			startColor = spr.color = c_tmp;
			c_tmp.a = 1;
			endColor = c_tmp;
		} else {
			c_tmp.a = 1;
			startColor = spr.color = c_tmp;
			c_tmp.a = 0;
			endColor = c_tmp;
		}
		t1 = 0f;
	}
}
