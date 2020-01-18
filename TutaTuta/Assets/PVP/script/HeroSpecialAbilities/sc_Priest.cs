using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Priest : MonoBehaviour {
	sc_Hero HR;
	sc_PVPGod GM;
	int roadNum;

	void Start () {
		HR = GetComponent<sc_Hero>();
		GM = HR.GM;

		roadNum = HR.roadNumber;

		GM.addShield (roadNum, gameObject.layer);
	}

}
