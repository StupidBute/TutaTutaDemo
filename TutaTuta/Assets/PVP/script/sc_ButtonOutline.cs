using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_ButtonOutline : MonoBehaviour {
	public GameObject God;
	public int side;
	public Transform[] Buttons = new Transform[3];
	sc_PVPGod GM;

	void Start () {
		GM = God.GetComponent<sc_PVPGod> ();
	}
	
	// Update is called once per frame
	void Update () {
		int num = sc_RoadCreate.createnum [side];
		if (num == -1)
			transform.position = new Vector2 (-5f, 0f);
		else if (GM.CurrentNum [side, num] == 0)
			transform.position = new Vector2 (-5f, 0f);
		else {
			transform.position = Buttons [num].position;
			transform.localScale = Buttons [num].localScale;
		}
			
	}
}
