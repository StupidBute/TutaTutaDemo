using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_RocketSmoke : MonoBehaviour {
	SpriteRenderer spr;
	float alpha = 1f;
	float scale = 0.2f;
	// Use this for initialization
	void Start () {
		transform.Translate ((float)Random.Range (-10, 10) / 120, 0, 0);
		spr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(alpha > 0.01f)
			alpha -= 0.02f;
		scale += 0.01f;
		spr.color = new Color (1, 1, 1, alpha);
		transform.localScale = new Vector2 (scale, scale);
	}
}
