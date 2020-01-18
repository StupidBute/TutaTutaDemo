using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Monk : MonoBehaviour {
	sc_PVPGod GM;
	sc_Hero HR;
	int roadNum;
	int side;

	void Start () {
		tag = "Tag_Monk";
		HR = GetComponent<sc_Hero>();
		GM = HR.GM;

		roadNum = HR.roadNumber;
		side = HR.side;
		GM.CheckMonks (roadNum, side, transform.rotation, 1);
	}

	public void DieMonk(){
		GM.CheckMonks (roadNum, side, transform.rotation, -1);
	}
}
