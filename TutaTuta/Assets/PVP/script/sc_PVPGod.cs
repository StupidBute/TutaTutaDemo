using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Heros{
	GameObject[] heros;
	public Heros(GameObject[] newHeros){
		heros = new GameObject[newHeros.Length];
		heros = newHeros;
	}

	public GameObject GetHero(int n){
		return heros [n];
	}
}

public class sc_PVPGod : MonoBehaviour {
	/* 0 = 廢地
	 * 1 = 耶利拉杜姆
	 * 2 = 黑尼度
	 * 3 = 艾達
	 * */

	#region 兵種通用素質
	public float[,] CDTime = new float[4, 3]{{1.6f, 3f, 4f}, {2f, 3.8f, 5.5f}, {1.7f, 3.5f, 4.5f}, {2f, 4f, 5.5f}};
	public int[,] MaxNum = new int[4, 3]{{8, 4, 3}, {8, 4, 4}, {8, 5, 3}, {8, 4, 3}};
	public int[,] CurrentNum = new int[2, 3];
	public int[] CastleMaxHP = new int[]{40, 40};

	//public static sc_PVPGod PVP_GM;
	public static float backSPD = -2f;
	public static float returnAcc = 0.06f;

	#endregion

	#region 一般變數

	public static int[] playerChoose = new int[2]{1, 3};

	public int Mode = 0;	//0 = PVP;	1 = Tutorial;	2 = PVE

	public GameObject[] AllHeros = new GameObject[16];
	public Heros[] GameHeros = new Heros[2];

	public GameObject[] Castle = new GameObject[2];
	public GameObject[] CastleHealthBar = new GameObject[2];
	public GameObject[] StorageSpot = new GameObject[6];
	public GameObject[] IconMagic = new GameObject[4];
	public GameObject[] ButtonObj = new GameObject[2];
	public GameObject[] ArrowHint = new GameObject[2];
	public GameObject Arrow_Multi;
	public GameObject HintObj;
	public GameObject BlackCover;
	public GameObject SoundManager;

	public Sprite[] HealthBarImg = new Sprite[4];
	public Sprite[] ButtonImg = new Sprite[12];
	public Sprite[] MagicIconImg = new Sprite[4];
	public Color[] StorageColor = new Color[4];
	public sc_CastleShake[] scShake = new sc_CastleShake[2];
	public sc_SoundManager GM_Sound;

	public GameObject VictoryObj;
	public GameObject CollideFlash;
	public GameObject DieBlast;
	public GameObject DieBlast2;
	public GameObject Shield;
	public GameObject SpeedRoad;

	public Vector2[] spawnPos = new Vector2[8];
	public int[] CastleHealth = new int[2];
	public int[] roadMonks = new int[4] {0, 0, 0, 0};
	GameObject[] SpawnedSpeedRoads = new GameObject[4];

	SpriteRenderer bCover;
	Image[] healthBar = new Image[2];
	//sc_EnemyAI AI;

	public int winningCamp = -1;
	public int winningSide = -1;

	public int GameState = -1;
	public int StartHintStep, StartButtonStep, StartCastleStep, StartStep;
	public int EndHintStep, EndButtonStep, EndCastleStep, EndVictoryStep, EndStep;

	public Animator HintAnim;
	sc_Lerp[] buttonLerp = new sc_Lerp[2];
	sc_Lerp[] castleLerp = new sc_Lerp[2];


	int StateStep = 0;
	float alpha = 1f;
	#endregion


	void Awake(){

		GM_Sound = SoundManager.GetComponent<sc_SoundManager> ();
		if (Mode == 0) {
			int[] index = new int[]{ playerChoose [0] * 4, playerChoose [1] * 4 };
			GameHeros [0] = new Heros (new GameObject[]{ AllHeros [index [0]], AllHeros [index [0] + 1], AllHeros [index [0] + 2] });
			GameHeros [1] = new Heros (new GameObject[]{ AllHeros [index [1]], AllHeros [index [1] + 1], AllHeros [index [1] + 2] });
			for (int i = 0; i < 3; i++) {
				CurrentNum [0, i] = MaxNum [playerChoose [0], i];
				CurrentNum [1, i] = MaxNum [playerChoose [1], i];
			}
		} 
		/*
		else if (Mode == 1) {
			AI = GetComponent<sc_EnemyAI> ();
			GameHeros [0] = new Heros (new GameObject[]{ AllHeros [3], AllHeros [4] });
			GameHeros [1] = new Heros (new GameObject[]{ AllHeros [0], AllHeros [1] });
			CurrentNum [0, 0] = CurrentNum [0, 1] = 0;
			CurrentNum [1, 0] = CurrentNum [1, 1] = 999;
			playerChoose [0] = 1;
			playerChoose [1] = 0;
		} else if (Mode == 2) {
			int index = playerChoose [0] * 4;
			GameHeros [0] = new Heros (new GameObject[]{ AllHeros [index], AllHeros [index + 1], AllHeros [index + 2] });
			AI = GetComponent<sc_EnemyAI> ();
			for (int i = 0; i < 3; i++) {
				CurrentNum [0, i] = MaxNum [playerChoose [0], i];
				CurrentNum [1, i] = 999;
			}
			playerChoose [1] = 4;
		}*/

	}

	void Start(){
		bCover = BlackCover.GetComponent<SpriteRenderer> ();
		HintAnim = HintObj.GetComponent<Animator> ();
		scShake [0] = Castle [0].GetComponent<sc_CastleShake> ();
		scShake [1] = Castle [1].GetComponent<sc_CastleShake> ();
		int i;
		if (Mode == 0) {
			for (i = 0; i < 6; i++) {
				StorageSpot [i].GetComponent<SpriteRenderer> ().color = StorageColor [playerChoose [i / 3]];
				if (i < 2) {
					CastleHealth [i] = CastleMaxHP[i];
					healthBar [i] = CastleHealthBar [i].GetComponentInChildren<Image> ();
					healthBar [i].sprite = HealthBarImg [playerChoose [i]];
					buttonLerp [i] = ButtonObj [i].GetComponent<sc_Lerp> ();
					castleLerp [i] = CastleHealthBar [i].GetComponent<sc_Lerp> ();
				}
			}
		}
		/*
		else if (Mode == 1) {
			for (i = 0; i < 2; i++) {
				StorageSpot [i].GetComponent<SpriteRenderer> ().color = StorageColor [0];
				CastleHealth [i] = CastleMaxHP[i];
				healthBar [i] = CastleHealthBar [i].GetComponentInChildren<Image> ();
				castleLerp [i] = CastleHealthBar [i].GetComponent<sc_Lerp> ();
			}
			buttonLerp [0] = ButtonObj [0].GetComponent<sc_Lerp> ();
		} else if (Mode == 2) {
			for (i = 0; i < 3; i++) {
				StorageSpot [i].GetComponent<SpriteRenderer> ().color = StorageColor [playerChoose[0]];
				if (i < 2) {
					CastleHealth [i] = CastleMaxHP[i];
					healthBar [i] = CastleHealthBar [i].GetComponentInChildren<Image> ();
					castleLerp [i] = CastleHealthBar [i].GetComponent<sc_Lerp> ();
				}
			}
			buttonLerp [0] = ButtonObj [0].GetComponent<sc_Lerp> ();
			healthBar [0].sprite = HealthBarImg [playerChoose [0]];
			healthBar [1].sprite = AI.spr_enemyHealth;
		}*/
	}

	void Update(){
		if (GameState == -1) {
			bCover.color = new Color (0, 0, 0, alpha);
			if (alpha < 0.01f) {
				if (Mode == 0 || Mode == 2)
					GameState = 0;
				else if (Mode == 1)
					GameState = 10;
				alpha = 0;
				BlackCover.SetActive (false);
			} else {
				alpha -= 0.02f;
			}

		}else if(GameState == 0){
			if (Mode == 0) {
				StateStep++;
				if (StateStep == StartHintStep) {
					StartCoroutine (WaitPlaySound (3, 2.65f));
					HintAnim.SetTrigger ("start");
				} else if (StateStep == StartButtonStep) {
					buttonLerp [0].StartMoving (new Vector2 (0f, -5f), 1f);
					buttonLerp [1].StartMoving (new Vector2 (0f, 5f), 1f);
				} else if (StateStep == StartCastleStep) {
					castleLerp [0].StartMoving (new Vector2 (2.86f, -1.26f), 1f);
					castleLerp [1].StartMoving (new Vector2 (-2.86f, 1.26f), 1f);
				} else if (StateStep >= StartStep) {
					if (HintAnim.GetCurrentAnimatorStateInfo (0).IsName ("anim_Hint_Idle")) {
						StateStep = 0;
						HintAnim.enabled = false;
						ArrowHint [0].SetActive (true);
						ArrowHint [1].SetActive (true);
						GameState = 1;
					}

				}
			} 
			/*
			else if (Mode == 1) {
				StateStep++;
				if (StateStep == StartHintStep) {
					StartCoroutine (WaitPlaySound (3, 2.65f));
					HintAnim.SetTrigger ("start");
				} else if (StateStep >= StartStep) {
					if (HintAnim.GetCurrentAnimatorStateInfo (0).IsName ("anim_Hint_Idle")) {
						StateStep = 0;
						HintAnim.enabled = false;
						GameState = 1;
						AI.StartAI (sc_EnemyAI.AIstate.Tutorial);
						ArrowHint[0] = Instantiate (Arrow_Multi, new Vector2 (0f, -3.8f), Quaternion.identity);
					}
				}
			} else if (Mode == 2) {
				StateStep++;
				if (StateStep == StartHintStep) {
					StartCoroutine (WaitPlaySound (3, 2.65f));
					HintAnim.SetTrigger ("start");
				} else if (StateStep == StartButtonStep) {
					buttonLerp [0].StartMoving (new Vector2 (0f, -5f), 1f);
				} else if (StateStep == StartCastleStep) {
					castleLerp [0].StartMoving (new Vector2 (2.86f, -1.26f), 1f);
					castleLerp [1].StartMoving (new Vector2 (-2.86f, 1.26f), 1f);
				} else if (StateStep >= StartStep) {
					if (HintAnim.GetCurrentAnimatorStateInfo (0).IsName ("anim_Hint_Idle")) {
						StateStep = 0;
						HintAnim.enabled = false;
						ArrowHint [0].SetActive (true);
						GameState = 1;
						AI.StartAI (sc_EnemyAI.AIstate.Normal);
					}

				}
			}*/

		}else if (GameState == 2) {
			StateStep++;
			if (StateStep == EndButtonStep) {
				buttonLerp [0].StartMoving (new Vector2 (0f, -6.3f), 1f);
				if(Mode == 0)
					buttonLerp [1].StartMoving (new Vector2 (0f, 6.3f), 1f);
			}else if (StateStep == EndCastleStep){
				castleLerp[0].StartMoving(new Vector2 (2.86f, -1.26f), 0.5f);
				castleLerp[1].StartMoving(new Vector2 (-2.86f, 1.26f), 0.5f);
			}else if (StateStep == EndHintStep){
				HintAnim.SetTrigger ("end");
			}else if(StateStep == EndVictoryStep){
				if (Mode == 0) {
					GameObject inst = Instantiate (VictoryObj, new Vector2 (0f, 0f), Quaternion.Euler (0f, 0f, 180f * (float)winningSide));
					inst.GetComponent<sc_VictoryObj> ().GM = GetComponent<sc_PVPGod> ();
					StateStep = 0;
					GameState = 3;
				} else {
					if (winningSide == 0) {
						GameObject inst = Instantiate (VictoryObj, new Vector2 (0f, 0f), Quaternion.identity);
						inst.GetComponent<sc_VictoryObj> ().GM = GetComponent<sc_PVPGod> ();
						StateStep = 0;
						GameState = 3;
					}
				}
			}else if (StateStep == EndStep) {
				StateStep = 0;
				GameState = 3;
				if (Mode == 2 && winningSide == 1)					//PVE模式且輸掉掉
					StartCoroutine (ChangeScene ("Scene_PVEMenu"));
			}
				
		}

	}

	//廢地 加速道
	public void CheckMonks(int roadNum, int side, Quaternion spRotation, int add){
		if (roadMonks [roadNum] == 0) {
			roadMonks [roadNum] += add;
			SpawnedSpeedRoads[roadNum] = Instantiate (SpeedRoad, spawnPos[4*side+roadNum], spRotation);
		} else {
			roadMonks [roadNum] += add;
			if (roadMonks [roadNum] == 0) {
				SpawnedSpeedRoads [roadNum].GetComponent<sc_SpeedRoad> ().DestroyRoad ();
				SpawnedSpeedRoads [roadNum] = null;
			}
		}
	}


	//耶利拉杜姆 生盾道
	public void addShield(int roadNum, int myLayer){
		int layerMask = 1 << myLayer;
		RaycastHit2D[] Hits = Physics2D.RaycastAll (spawnPos[roadNum], Vector2.up, 10f, layerMask);
		foreach (RaycastHit2D hit in Hits) {
			Instantiate(Shield, hit.collider.transform.position, hit.collider.transform.rotation, hit.collider.transform);
			hit.collider.GetComponent<sc_Hero> ().shield = 1;
		}
	}

	public void DamageCastle(int side){
		if (GameState == 1) {
			CastleHealth [side]--;
			scShake[side].CastleShake ();
			healthBar [side].fillAmount = (float)CastleHealth [side] / (float)CastleMaxHP[side];
			//if (Mode == 2 && side == 1)
				//AI.Goal ();
			
			if (CastleHealth [side] == 0) {
				HintAnim.enabled = true;
				scShake [side].Lose = true;
				Castle [side].transform.GetChild (0).gameObject.SetActive (true);
				sc_RoadCreate.createnum [0] = sc_RoadCreate.createnum [1] = -1;
				winningSide = side == 0 ? 1 : 0;
				winningCamp = playerChoose [winningSide];
				StartCoroutine (WaitPlaySound (4, 0.4f));

				//if (AI != null)
					//AI.StopAI ();
				
				if (ArrowHint [side] != null)
					Destroy (ArrowHint [side].gameObject);

				if (Mode != 1 || winningSide == 0) {
					GameState = 2;
					if (Mode == 2 && winningSide == 1) {
						GM_Sound.StartLerpVolume (2f, 0.3f);
						HintAnim.SetBool ("win", false);
						StartCoroutine (WaitPlaySound (2, 2.4f));
					} else {
						GM_Sound.StartLerpVolume (2f, 0.5f);
						HintAnim.SetBool ("win", true);
					}
					
				} else {								//教學模式且輸掉掉
					GM_Sound.StartLerpVolume (2f, 0.5f);
					GameState = 11;
					CurrentNum [0, 0] = CurrentNum [0, 1] = -1;
					HintAnim.SetBool ("win", false);
					HintAnim.SetTrigger ("end");
					StartCoroutine (WaitPlaySound (2, 2.4f));
				}

			}
		} else if (GameState == 10) {
			CastleHealth [side]--;
			scShake[side].CastleShake ();
		}
	}

	public IEnumerator ChangeScene(string sceneName){
		if (Mode == 1) {
			PlayerPrefs.SetString ("Tutorial", "pass");
			PlayerPrefs.Save ();
		}
		BlackCover.SetActive (true);
		alpha = 0f;
		GM_Sound.StartFadeOut (1f);
		while (alpha < 1f) {
			alpha += Time.deltaTime;
			if (alpha > 1f)
				alpha = 1f;
			bCover.color = new Color (0, 0, 0, alpha);
			yield return null;
		}
		SceneManager.LoadScene (sceneName);
	}

	IEnumerator WaitPlaySound(int _num, float _t){
		yield return new WaitForSeconds (_t);
		GM_Sound.PlaySoundEffect (_num, true);
	}

}
