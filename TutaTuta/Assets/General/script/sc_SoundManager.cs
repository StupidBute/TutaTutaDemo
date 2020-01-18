using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_SoundManager : MonoBehaviour {
	public AudioSource[] sdEffect = new AudioSource[9];
	public AudioSource sdBGM;
	public float Origin_BGMVolume = 0.3f;
	bool fadedOut = false;
	float volume = 1f;

	public void StartFadeOut(float speed){
		if (!fadedOut) {
			fadedOut = true;
			StartCoroutine (FadeOut (speed));
		}
	}

	public void StartLerpVolume(float _spd, float _v){
		StartCoroutine (LerpVolume (_spd, _v));
	}

	public void PlaySoundEffect(int num, bool play){
		if (play)
			sdEffect [num].Play ();
		else
			sdEffect [num].Stop ();
	}

	IEnumerator LerpVolume(float _spd, float _v){
		float t = 0f;
		float _v0 = volume;
		while (t < 1f) {
			t += Time.deltaTime * _spd;
			volume = Mathf.Lerp(_v0, _v, t);
			sdBGM.volume = volume * Origin_BGMVolume;
			yield return null;
		}
		volume = _v;
		sdBGM.volume = volume * Origin_BGMVolume;
	}

	IEnumerator FadeOut(float spd){
		while (volume > 0f) {
			volume -= spd * Time.deltaTime;
			if (volume < 0f)
				volume = 0f;
			sdBGM.volume = volume * Origin_BGMVolume;
			yield return null;
		}
	}
}
