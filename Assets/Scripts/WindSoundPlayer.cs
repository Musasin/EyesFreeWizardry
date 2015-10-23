using UnityEngine;
using System.Collections;

public class WindSoundPlayer : MonoBehaviour {

	public AudioClip audioClip;
	AudioSource audioSource;
	int hitObjectNum;
	bool frontHit;

	enum Mode{
		FADE_IN,
		MAX,
		FADE_OUT,
		MUTE,
		HALF
	};
	Mode nowMode;

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.clip = audioClip;
		audioSource.Play ();
		hitObjectNum = 0;
		nowMode = Mode.FADE_IN;
	}
	
	// Update is called once per frame
	void Update () {


		switch (nowMode) {
		case Mode.FADE_IN:
			audioSource.volume = audioSource.volume + 0.02f;
			break;
		case Mode.MAX:
			break;
		case Mode.FADE_OUT:
			audioSource.volume = audioSource.volume - 0.02f;
			break;
		case Mode.MUTE:
			break;

		}
		if (frontHit && hitObjectNum <= 0)
			audioSource.volume = 0.5f;

		if (hitObjectNum <= 0)
			nowMode = Mode.FADE_IN;
		else
			nowMode = Mode.FADE_OUT;
	}

	private void OnTriggerEnter(Collider col){
		if (col.name == "frontEmpty")
			frontHit = true;
		else if(col.tag != "hideCube")
			hitObjectNum++;
	}

	private void OnTriggerExit(Collider col){
		if (col.name == "frontEmpty")
			frontHit = false;
		else if(col.tag != "hideCube")
			hitObjectNum--;
	}
}
