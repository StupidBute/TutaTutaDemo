using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_HealthBar_Beamer : MonoBehaviour {
	public GameObject BottomHealthBar;
	public GameObject UpperHealthBar;
	public Sprite SuperHealthBar;
	public Color SuperHealthColor;
	public int superMode = 0;
	/* 0 = normal
	 * 1 = shrink
	 * 2 = grow
	 * 3 = set
	 * */

	float MaxScale, xScale;
	Image upperHB;
	sc_Hero hero;
	float fillSpeed = 0.01f;

	void Start(){
		upperHB = UpperHealthBar.GetComponent<Image> ();
		hero = GetComponentInParent<sc_Hero> ();
		MaxScale = transform.localScale.x;
		xScale = transform.localScale.x;
	}

	// Update is called once per frame
	void Update () {
		if (superMode != 1) {
			float targetAmount = hero.hp / hero.MaxHP;

			float difference = upperHB.fillAmount - targetAmount;
			if (difference > 0) {
				upperHB.fillAmount -= fillSpeed + difference* 0.1f;
			}
			if (upperHB.fillAmount < targetAmount)
				upperHB.fillAmount = targetAmount;
		}

		if (superMode == 1) {
			xScale -= 0.1f;
			if (xScale <= 0f) {
				xScale = 0f;
				superMode = 2;
				upperHB.sprite = SuperHealthBar;
				upperHB.color = SuperHealthColor;
				BottomHealthBar.GetComponent<Image>().sprite = SuperHealthBar;
				fillSpeed = 0.05f;
			}
		} else if (superMode == 2) {
			xScale += 0.1f;
			if (xScale >= MaxScale) {
				xScale = MaxScale;
				superMode = 3;
			}
		}

		transform.localScale = new Vector2(xScale, transform.localScale.y);
	}
}
