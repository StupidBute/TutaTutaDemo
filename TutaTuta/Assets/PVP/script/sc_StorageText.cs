using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_StorageText : MonoBehaviour {
	public GameObject God;
	public SpriteRenderer storageBlack;
	public int side;
	public int num;

	sc_PVPGod GM;
	Text currentNum;

	void Start(){
		
		GM = God.GetComponent<sc_PVPGod> ();
		currentNum = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GM.Mode == 0 || GM.Mode == 2) {
			currentNum.text = GM.CurrentNum [side, num].ToString();

		}else if (GM.Mode == 1) {
			int showNum = GM.CurrentNum [side, num];
			showNum = showNum < 0 ? 0 : showNum;
			currentNum.text = showNum.ToString();

			if (GM.GameState != 1) {
				if (GM.CurrentNum [0, num] == 0 && storageBlack.color.a < 0.1f) {
					storageBlack.color = new Color (0, 0, 0, 0.5f);
				} else if (GM.CurrentNum [0, num] != 0 && storageBlack.color.a > 0.4f) {
					storageBlack.color = new Color (0, 0, 0, 0f);
					StartCoroutine (Bounce());
				}
			}

		}
	}

	IEnumerator Bounce(){
		float scale = 1.5f;
		float speed = 18f, spdAcc = -3f;

		while (scale > 1f) {
			scale += speed*Time.deltaTime;
			speed += spdAcc;
			transform.localScale = new Vector2 (scale, scale);
			yield return null;
		}

		transform.localScale = new Vector2 (1f, 1f);

	}
}
