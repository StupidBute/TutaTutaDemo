using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sc_VictoryGod : MonoBehaviour {
	public static int WiningCamp = 0;
	public static int WiningSide = 0;

	public GameObject MyCam;
	public GameObject BlackCover;
	public GameObject Logo;
	public GameObject People;
	public GameObject Cover_Left;
	public GameObject Cover_Right;
	public GameObject VicText;
	public GameObject ShiningLight;
	public GameObject Background;
	public GameObject ReturnButton;
	public GameObject SoundManager;

	public Sprite[] imgLogo;
	public Sprite[] imgPeople;
	public Sprite[] imgCover;
	public Sprite[] imgVicText;
	public Sprite[] imgShiningLight;
	public Sprite[] imgBackground;

	sc_SoundManager GM_Sound;

	Image bCover;

	int state = -1;
	int waitStep = 0;
	int lightDir = -1;
	SpriteRenderer logo, sLight;
	SpriteRenderer reButton;
	float alpha = 1f, lightAlpha = 1f, buttonAlpha = 1f, buttonColor = 1f;
	float lerpT = 0;

	bool PressedReturn = false;


	void Start () {
		GM_Sound = SoundManager.GetComponent<sc_SoundManager> ();

		if (WiningSide == 1) {
			MyCam.transform.rotation = Quaternion.Euler (0f, 0f, 180f);
		}

		if (WiningCamp == 1)
			buttonColor = 0.2f;

		bCover = BlackCover.GetComponent<Image> ();

		logo = Logo.GetComponent<SpriteRenderer> ();
		logo.sprite = imgLogo [WiningCamp];
		logo.color = new Color(1, 1, 1, 0f);

		People.GetComponent<SpriteRenderer> ().sprite = imgPeople [WiningCamp];

		Cover_Left.GetComponent<SpriteRenderer> ().sprite = imgCover [WiningCamp];
		Cover_Right.GetComponent<SpriteRenderer> ().sprite = imgCover [WiningCamp];

		VicText.GetComponent<SpriteRenderer> ().sprite = imgVicText [WiningCamp];

		sLight = ShiningLight.GetComponent<SpriteRenderer> ();
		sLight.sprite = imgShiningLight [WiningCamp];
		sLight.color = new Color(1, 1, 1, 0f);

		Background.GetComponent<SpriteRenderer>().sprite = imgBackground [WiningCamp];

		reButton = ReturnButton.GetComponent<SpriteRenderer>();
		reButton.color = new Color(0, 0, 0, 0);
	}

	void Update () {
		switch (state) {
		case -1:
			alpha -= 0.02f;
			bCover.color = new Color (0, 0, 0, alpha);
			if (alpha <= 0.01f) {
				state = 0;
				alpha = 0f;
			}
			break;
		case 0:
			alpha += 0.05f;
			logo.color = new Color (1, 1, 1, alpha);
			if (alpha >= 0.99f) {
				state = 1;
				alpha = 0f;
			}
			break;
		
		case 1:
			People.SetActive (true);
			state = 2;
			break;
		
		case 2:
			if (waitStep == 20)
				state = 3;
			else
				waitStep++;
			break;
		
		case 3:
			lerpT += 0.05f;

			Cover_Right.transform.position = Vector2.Lerp (Cover_Right.transform.position, new Vector2 (3f, -0.1f), lerpT);
			Cover_Left.transform.position = Vector2.Lerp (Cover_Left.transform.position, new Vector2 (-3f, -0.1f), lerpT);

			if (lerpT > 1f) {
				GM_Sound.PlaySoundEffect (1, true);
				ReturnButton.SetActive (true);
				state = 4;
			}
			break;
		
		case 4:
			ShiningLight.transform.Rotate (0, 0, 0.05f);
			alpha += 0.02f;
			sLight.color = new Color (1, 1, 1, alpha);
			reButton.color = new Color (buttonColor, buttonColor, buttonColor, alpha);
			if (alpha >= 0.99f) 
				state = 5;
				
			break;
		case 5:
			ShiningLight.transform.Rotate (0, 0, 0.05f);

			lightAlpha += lightDir * 0.003f;

			if (lightDir == -1 && lightAlpha <= 0.5f)
				lightDir = 1;
			else if (lightDir == 1 && lightAlpha >= 0.99f)
				lightDir = -1;
			sLight.color = new Color (1, 1, 1, lightAlpha);

			buttonAlpha = 0.6f + Mathf.PingPong (Time.time, 0.4f);
			reButton.color = new Color (buttonColor, buttonColor, buttonColor, buttonAlpha);

			if (sc_FingerTouch.TouchMode)
				CheckTouch ();
			else
				CheckMouse ();

			break;

		}

		if (PressedReturn) {
			alpha += 2f * Time.deltaTime;
			bCover.color = new Color (0, 0, 0, alpha);
			if (alpha >= 0.99f) {
				SceneManager.LoadScene("Scene_PVPMenu");
			}
		}

	}

	void CheckTouch(){
		if (Input.touchCount != 0 && Input.touches [0].phase == TouchPhase.Began) {
			RaycastHit2D hit = TouchedButton (Input.touches[0].position);
			if (hit.collider != null)
				ToMenu ();
		}
	}

	void CheckMouse(){
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = TouchedButton (Input.mousePosition);
			if (hit.collider != null)
				ToMenu ();
		}
	}

	public void ToMenu(){
		if (!PressedReturn) {
			GM_Sound.PlaySoundEffect (0, true);
			GM_Sound.StartFadeOut (2f);
			alpha = 0;
			PressedReturn = true;
		}

	}

	RaycastHit2D TouchedButton(Vector2 Pos){
		Ray ray = Camera.main.ScreenPointToRay (Pos);
		int layerMask = 1 << 10;
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, 10, layerMask);
		return hit;
	}
}
