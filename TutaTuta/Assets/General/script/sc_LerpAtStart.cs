using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_LerpAtStart : MonoBehaviour {
	public bool move = false;
	public float moveSpeed;
	public Vector2 targetPos;

	public int fade;
	public float fadeSpeed;

	// Use this for initialization
	void Start () {
		if (move)
			GetComponent<sc_Lerp> ().StartMoving (targetPos, moveSpeed);
		if (fade == 1)
			GetComponent<sc_Lerp> ().StartFading (true, fadeSpeed);
		else if (fade == -1)
			GetComponent<sc_Lerp> ().StartFading (false, fadeSpeed);

		Destroy (GetComponent<sc_LerpAtStart>());
	}

}
