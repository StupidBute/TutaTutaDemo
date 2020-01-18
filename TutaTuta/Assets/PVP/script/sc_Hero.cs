using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Hero : MonoBehaviour {

#region 變數
	public sc_PVPGod GM;
	//素質
	public float MaxHP;
	public float MaxSPD;
	public float ATK;
	public float Weight;
	public int shield;
	public float totalWeight;
	public float ConnectLength;		//偵測友軍連結距離，從中心點算起



	//身分
	public int camp;				//陣營編號
	public int roadNumber;			//路線編號
	public int side;				//player1 or player2
	public int face;				//面對方向 1 or -1
	public int enemyLayer;
	public int num;					//for PVE

	public float returnAcc;
	public float backSPD;
	public float hp;
	public float spd;

	public GameObject MyHealthBar;
	bool colliding = false;


	public sc_Hero FrontConnect = null;
	public sc_Hero BackConnect = null;
	public Animator Anim;

	int WinState = 0;
	Renderer ren;

#endregion

#region 初始化Start

	void Start () {
		hp = MaxHP;
		totalWeight = Weight;
		returnAcc = sc_PVPGod.returnAcc;
		backSPD = sc_PVPGod.backSPD;
		spd = MaxSPD;


		Anim = GetComponent<Animator> ();
		ren = GetComponent<Renderer> ();

		if (side == 0) {
			gameObject.layer = 8;
			enemyLayer = 9;
		} else {
			gameObject.layer = 9;
			enemyLayer = 8;
		}

		//if(ConnectLength < 0.41f)
			//transform.Translate (0, 0.41f - ConnectLength, 0);

		MyHealthBar.SetActive (false);
		if (!SpawnConnect ()) {
			if (face == 1)
				CheckCollide();
		}

	}

#endregion

#region 重複執行Update

	void Update () {

		if (GM.GameState == 2 || GM.GameState == 3 || GM.GameState == 11) {
			if (WinState == 0) {
				if (Anim != null)
					Anim.speed *= 0.5f;
				Destroy (transform.GetChild (0).gameObject);
				if (GM.winningSide == side) {
					if (MaxSPD > 2f)
						MaxSPD = 1f;
					else
						MaxSPD *= 0.5f;
					WinState = 1;
				} else {
					MaxSPD = -1.4f;
					WinState = -1;
				}
			} else if (WinState <= -1) {
				WinState--;
				if (WinState <= -50)
					SelfDestruct ();
			} else if (WinState >= 1 && GM.GameState == 11) {
				WinState++;
				if (WinState >= 50)
					SelfDestruct ();
			}

		}

		if (transform.position.y < -4.5f || transform.position.y > 4.5f || hp < 0.01f)
			SelfDestruct();
		
		CheckConnect ();
		transform.Translate (0, spd * Time.deltaTime, 0);
		AddSpeed ();

		if (face == 1)
			CheckCollide();

		ren.sortingOrder = -face * (int)(transform.position.y * 100f);

	}

#endregion

#region 自訂函式

	#region 給予整行速度

	public void ConnectSpeed(float connectSpd, bool checkBack){
		spd = connectSpd;
		if (checkBack) {
			if (BackConnect != null)
				BackConnect.ConnectSpeed (connectSpd, true);
				
		} else {
			if (FrontConnect != null)
				FrontConnect.ConnectSpeed (connectSpd, false);
		}

	}

	#endregion

	#region 受攻擊(扣血或減護盾)
	public void Damaged(float dmg){
		if (dmg >= 99f) {						//火箭
			hp = 0f;
			return;
		}else if (dmg < 0.01f)					//碰到無傷英雄
			return;
		

		if (shield > 0) {						//有護盾
			shield--;
			if (camp == 1 && shield == 0)
				transform.GetChild (2).GetComponent<sc_Shield> ().SelfDestroy ();
			else if (tag == "Tag_SuperBeamer")
				hp = shield;
		} else {								//無護盾
			hp -= dmg;
			if(MyHealthBar != null)
				MyHealthBar.SetActive (true);
		}
		return;
	}
	#endregion

	#region 偵測友軍連結
	bool SpawnConnect(){							//start & forward
		int layerMask = 1 << gameObject.layer;
		RaycastHit2D other = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + face*ConnectLength), face * Vector2.up, 0.08f, layerMask);
		if (other.collider != null) {
			FrontConnect = other.collider.GetComponent<sc_Hero>();
			FrontConnect.BackConnect = GetComponent<sc_Hero>();
			FrontConnect.totalWeight = Weight + FrontConnect.Weight;
			transform.Translate (0f, other.distance-0.01f, 0f);
			AddSpeed ();
			return true;
		}
		return false;
	}

	void CheckConnect(){							//update & backward
		int layerMask = 1 << gameObject.layer;
		RaycastHit2D other = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - face*ConnectLength), face * Vector2.down, 0.08f, layerMask);
		if (other.collider != null) {
			if (other.distance <= 0.02f) {
				if (BackConnect == null) {
					BackConnect = other.collider.GetComponent<sc_Hero>();
					BackConnect.FrontConnect = GetComponent<sc_Hero>();
				}
				totalWeight = Weight + BackConnect.totalWeight;

			}
		}else{
			if (BackConnect != null) {
				BackConnect.FrontConnect = null;
				BackConnect = null;
			}
			totalWeight = Weight;
		}
	}
	#endregion

	#region 增速
	void AddSpeed(){
		float SpeedLimit, Acc;
		if (camp == 0 && GM.roadMonks [roadNumber] != 0 && GM.GameState == 1) {
			SpeedLimit = MaxSPD+1.2f;
			Acc = returnAcc + 0.04f;
		}else{
			SpeedLimit = MaxSPD;
			Acc = returnAcc;
		}
			
		if (FrontConnect != null) {										//have front connect
			float frontSpd = FrontConnect.spd;
			if (SpeedLimit > frontSpd)
				SpeedLimit = frontSpd;
		}

		if(spd < SpeedLimit)
			spd += Acc;
		
		if (spd > SpeedLimit)
			spd = SpeedLimit;

		if (Anim != null) {
			if(spd < -0.01f)
				Anim.SetBool ("hit", true);
			else
				Anim.SetBool ("hit", false);
		}
	}
	#endregion

	#region 偵測敵軍碰撞
	void CheckCollide(){
		int layerMask = 1 << enemyLayer;
		RaycastHit2D other = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + ConnectLength), Vector2.up, 0.01f, layerMask);
		if (other.collider != null) {
			if (!colliding) {
				colliding = true;
				Instantiate (GM.CollideFlash, other.point, Quaternion.identity);
				sc_Hero enemy = other.collider.GetComponent<sc_Hero> ();

				enemy.Damaged (ATK);
				Damaged (enemy.ATK);

				if (tag == "Tag_ShieldMan")
					GetComponent<sc_ShieldMan> ().ShieldManHit ();
				else if (other.collider.tag == "Tag_ShieldMan")
					other.collider.GetComponent<sc_ShieldMan> ().ShieldManHit ();

				float otherWeight = enemy.totalWeight;
				if (totalWeight > otherWeight + 0.01f) {
					enemy.ConnectSpeed(backSPD, true);
					spd = 0f;
					if (Anim != null && tag != "Tag_ShieldMan")
						Anim.SetTrigger ("attack");
						
				} else if (totalWeight < otherWeight - 0.01f) {
					enemy.ConnectSpeed(0f, true);
					spd = backSPD;
					if (enemy.Anim != null && enemy.tag != "Tag_ShieldMan")
						enemy.Anim.SetTrigger ("attack");
						
						
				} else {
					enemy.ConnectSpeed(backSPD, true);
					spd = backSPD;
				}
			}	
		} else {
			if (colliding)
				colliding = false;
		}
	}
	#endregion

	#region 死亡
	void SelfDestruct(){
		if (BackConnect != null)
			BackConnect.FrontConnect = null;
		if (FrontConnect != null)
			FrontConnect.BackConnect = null;

		if (tag == "Tag_Monk")
			GetComponent<sc_Monk> ().DieMonk();

		if (side == 0) {
			if (transform.position.y > 4.5f) {
				GM.DamageCastle (1);
			} else if (transform.position.y < -4.5) {
				Instantiate (GM.DieBlast2, new Vector2 (transform.position.x, -4f), transform.rotation);
			} else {
				GameObject Inst = Instantiate (GM.DieBlast, transform.position, transform.rotation);
				if (ConnectLength > 0.4)
					Inst.transform.localScale = new Vector2 (0.5f, 0.5f);
			}
		} else {
			if (transform.position.y < -4.5f)
				GM.DamageCastle(0);
			else if (transform.position.y > 4.5)
				Instantiate (GM.DieBlast2, new Vector2 (transform.position.x, 4f), transform.rotation);
			else {
				GameObject Inst = Instantiate (GM.DieBlast, transform.position, transform.rotation);
				if (ConnectLength > 0.4)
					Inst.transform.localScale = new Vector2 (0.5f, 0.5f);
			}
		}
		
		Destroy (gameObject);
	}
	#endregion


#endregion

}
