using UnityEngine;
using System.Collections;

public class WaterDrop : MonoBehaviour {
	
	public AudioClip audioClip;
	AudioSource audioSource;

	double time = 0;
	double elapsedTime = 0;

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.clip = audioClip;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		elapsedTime += Time.deltaTime;;

		if (/*!audioSource.isPlaying &&*/ elapsedTime > 1) {
			audioSource.Play ();
			elapsedTime = 0;
		}

	}
}
