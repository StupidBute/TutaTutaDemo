using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_CreateMagic : MonoBehaviour {
	public GameObject God;
	public int side;
	// Use this for initialization
	void Start () {
		sc_PVPGod GM = God.GetComponent<sc_PVPGod> ();
		int cmp = sc_PVPGod.playerChoose [side];
		GetComponent<SpriteRenderer>().sprite = GM.MagicIconImg[cmp];
		Instantiate (GM.IconMagic [cmp], transform.position, transform.rotation, transform);
	}
}
