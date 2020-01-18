using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_HealthBar : MonoBehaviour {

	public GameObject UpperHealthBar;
	Image upperHB;
	sc_Hero hero;

	void Start(){
		upperHB = UpperHealthBar.GetComponent<Image> ();
		hero = GetComponentInParent<sc_Hero> ();
	}

	// Update is called once per frame
	void Update () {
		float targetAmount = hero.hp / hero.MaxHP;
		float difference = upperHB.fillAmount - targetAmount;
		if (difference > 0) {
			upperHB.fillAmount -= 0.01f + difference * 0.1f;
		}
		if (upperHB.fillAmount < targetAmount)
			upperHB.fillAmount = targetAmount;
	}
}
