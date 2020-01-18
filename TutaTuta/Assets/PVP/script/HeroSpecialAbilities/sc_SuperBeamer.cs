using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_SuperBeamer : MonoBehaviour {
	public int Shield;
	public float MaxSPD;
	public float Weight;
	public float ATK;
	public int SuperStep;
	public float returnAcc;
	public GameObject TransEffect;

	Animator Anim;
	int HitCount = 0;
	sc_Hero hero;

	// Use this for initialization
	void Start () {
		hero = GetComponent<sc_Hero> ();
		Anim = GetComponent<Animator> ();
		if(hero.GM.Mode == 2 && hero.side == 0)		//AI對戰模式且為玩家邊
			SuperStep -= 1;
	}

	public void HitByBeam(){
		if (tag == "Tag_SuperBeamer")
			return;
		
		HitCount++;
		if (HitCount == SuperStep) {
			Instantiate (TransEffect, transform.position, transform.rotation, transform);
			hero.shield = Shield;
			hero.MaxHP = Shield;
			hero.hp = Shield;
			hero.MaxSPD = MaxSPD;
			hero.returnAcc = returnAcc;
			hero.Weight = Weight;
			hero.totalWeight = Weight;
			hero.ATK = ATK;
			tag = "Tag_SuperBeamer";
			Anim.SetBool ("Super", true);
			hero.MyHealthBar.SetActive (true);
			hero.MyHealthBar.GetComponent<sc_HealthBar_Beamer> ().superMode = 1;
		}
	}
}
