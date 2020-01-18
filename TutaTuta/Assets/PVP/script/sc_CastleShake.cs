using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_CastleShake : MonoBehaviour {
	Vector2 Origin, ShakePoint;
	Vector2[] LoseShake = new Vector2[2];
	bool Shake = false;
	public bool Lose = false;
	float t = 0f;
	int ShakeStep = 0;
	// Use this for initialization
	void Start () {
		Origin = transform.position;
		ShakePoint = transform.position;
		LoseShake[0] = new Vector2 (transform.position.x - 0.02f, transform.position.y);
		LoseShake[1] = new Vector2 (transform.position.x + 0.02f, transform.position.y);

	}
	
	// Update is called once per frame
	void Update () {
		if (Lose) {
			ShakeStep++;
			if (ShakeStep == 4) {
				transform.position = LoseShake [0];
			} else if (ShakeStep == 8) {
				ShakeStep = 0;
				transform.position = LoseShake [1];
			}
		}else if (Shake) {
			t += 0.2f;
			transform.position = Vector2.Lerp (ShakePoint, Origin, t);
			if (t > 0.99f) {
				t = 0;
				Shake = false;
			}
		}
	}

	public void CastleShake(){
		int i = Random.Range (1, 3);
		int dir = i == 1 ? 1 : -1;
		transform.Translate ((float)(dir * Random.Range (2, 5)) * 0.02f, 0, 0);
		ShakePoint = transform.position;
		Shake = true;
	}
}
