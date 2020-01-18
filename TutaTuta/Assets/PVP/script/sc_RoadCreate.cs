using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_RoadCreate : MonoBehaviour {
	public static int[] createnum = new int[2];
	public int side;
	public int roadNumber;
	public GameObject God;
	public GameObject SpawnEffect;
	public cooldowntime[] cd = new cooldowntime[3];
	int face, playerChoose;
	Vector2 spawnPos, checkPos;
	Vector2 closeSpawnPos;

	GameObject king;
	Quaternion spawnRotate;
	sc_PVPGod GM;

	void Awake(){
		createnum = new int[2]{ -1, -1 };
	}

	void Start () 
	{
		GM = God.GetComponent<sc_PVPGod> ();
		king = null;

		if (GM.Mode == 0) {
			playerChoose = sc_PVPGod.playerChoose [side];
			Initiate ();

		} else if (GM.Mode == 1) {
			if (side == 0)
				playerChoose = 1;
			else
				playerChoose = 0;
			
			Initiate ();

		} else if (GM.Mode == 2) {
			if (side == 0)
				playerChoose = sc_PVPGod.playerChoose [0];
			else
				playerChoose = 4;
			Initiate ();
		}

		closeSpawnPos = spawnPos;

	}

	public void TouchRoad()
	{
		if (createnum [side] != -1) {
			king = GM.GameHeros [side].GetHero (createnum [side]);

			if (GM.Mode == 0) {
				if (king.tag == "Tag_Rocket")
					CreateRocket (true);
				else
					CheckSpawn (true);

			} else if (GM.Mode == 1) {
				if (side == 0)
					CheckSpawn (true);
				else
					CheckSpawn (false);
			} else if (GM.Mode == 2) {
				if (side == 0) {
					if (king.tag == "Tag_Rocket")
						CreateRocket (true);
					else
						CheckSpawn (true);
				} else {
					if (king.tag == "Tag_Rocket")
						CreateRocket (false);
					else
						CheckSpawn (false);
				}
			}
		}


	}

	void CreateRocket(bool limited){
		if (limited) {
			if (GM.CurrentNum [side, createnum [side]] > 0) {
				GM.CurrentNum [side, createnum [side]]--;
				GameObject Inst = Instantiate (king, spawnPos, spawnRotate);

				sc_Bullet rocket = Inst.GetComponent<sc_Bullet> ();
				rocket.face = face;
				rocket.GM = GM;
				if (cd [createnum [side]] != null)
					StartCoroutine (cd [createnum [side]].Flash ());
			}
		} else {
			GameObject Inst = Instantiate (king, spawnPos, spawnRotate);
			sc_Bullet rocket = Inst.GetComponent<sc_Bullet> ();
			rocket.face = face;
			rocket.GM = GM;
		}

	}

	void CheckSpawn(bool limited){
		if (limited && GM.CurrentNum [side, createnum [side]] == 0)
			return;
		
		int layerMask = 3 << 8;
		RaycastHit2D other = Physics2D.Raycast (checkPos, spawnRotate * Vector2.up, 0.5f, layerMask);

		if (other.collider == null){												//far enough
			CreateHero (limited);

		}else{																		//close
			if (other.distance < 0.15f) {
				return;
			} else {
				if (other.collider.gameObject.layer != side + 8 && other.distance < 0.3f)
					return;
				float myLength = king.GetComponent<sc_Hero>().ConnectLength;
				float dy = -face * (0.01f + myLength);
				closeSpawnPos = new Vector2 (transform.position.x, other.point.y + dy);
				CreateHero (limited, closeSpawnPos);
			}

		}
	}

	public float Distance(int type){
		RaycastHit2D other = Detect (type);
		if (other.collider != null)
			return other.distance;
		else
			return 8f;
		
	}

	public RaycastHit2D Detect(int type){
		int layerMask;
		if (type == 0)
			layerMask = 1 << 8;
		else if (type == 1)
			layerMask = 1 << 9;
		else
			layerMask = 3 << 8;

		return Physics2D.Raycast (checkPos, spawnRotate * Vector2.up, 8f, layerMask);

	}

	public int CountRoad(int type){
		int layerMask = type == 0? 1 << 8 : 1 << 9;
		return Physics2D.RaycastAll (checkPos, spawnRotate * Vector2.up, 8f, layerMask).Length;
	}

	public void Initiate(){
		if (side == 0) {
			spawnRotate = Quaternion.Euler (0, 0, 0);
			face = 1;
			spawnPos =  new Vector3(transform.position.x ,transform.position.y-1.86f);
			checkPos =  new Vector2(transform.position.x ,transform.position.y-1.95f);
		} else {
			spawnRotate = Quaternion.Euler (0, 0, 180);
			face = -1;
			spawnPos =  new Vector3(transform.position.x ,transform.position.y+1.86f);
			checkPos =  new Vector2(transform.position.x ,transform.position.y+1.95f);
		}
	}

	void CreateHero(bool limited){
		Instantiate (SpawnEffect, new Vector2 (transform.position.x, -face * 3.86f), spawnRotate);
		if (limited) {
			GM.CurrentNum [side, createnum [side]]--;
			if (cd [createnum [side]] != null)
				StartCoroutine (cd [createnum [side]].Flash ());
		}
			

		GameObject Inst = Instantiate (king, spawnPos, spawnRotate);

		sc_Hero instHero = Inst.GetComponent<sc_Hero> ();
		instHero.GM = GM;
		instHero.roadNumber = roadNumber;
		instHero.camp = playerChoose;
		instHero.side = side;
		instHero.face = face;
		instHero.num = createnum [side];
	}

	void CreateHero(bool limited, Vector2 newPos){
		
		Instantiate (SpawnEffect, new Vector2 (transform.position.x, -face * 3.86f), spawnRotate);
		if (limited) {
			GM.CurrentNum [side, createnum [side]]--;
			if (cd [createnum [side]] != null)
				StartCoroutine (cd [createnum [side]].Flash ());
		}

		GameObject Inst = Instantiate (king, newPos, spawnRotate);

		sc_Hero instHero = Inst.GetComponent<sc_Hero> ();
		instHero.GM = GM;
		instHero.roadNumber = roadNumber;
		instHero.camp = playerChoose;
		instHero.side = side;
		instHero.face = face;
		instHero.num = createnum [side];
	}
}
