using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Shield : MonoBehaviour {

	public GameObject ShieldBreak;

	// Use this for initialization
	void Start () {
		GameObject firstShield = transform.parent.GetChild (2).gameObject;
		if (firstShield != gameObject)
			Destroy (firstShield);

		if (transform.parent.transform.localScale.x > 0.7f)
			transform.localScale = new Vector2(0.58f, 0.58f);
	}

	public void SelfDestroy(){
		GameObject Inst = Instantiate (ShieldBreak, transform.position, Quaternion.identity);
		if(transform.localScale.x > 0.65f)
			Inst.transform.localScale = new Vector3 (0.27f, 0.27f, 0.27f);
		else
			Inst.transform.localScale = new Vector3 (0.37f, 0.37f, 0.37f);
		Destroy (gameObject);
	}

}
