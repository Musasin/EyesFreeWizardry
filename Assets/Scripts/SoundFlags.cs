using UnityEngine;
using System.Collections;

public class SoundFlags : MonoBehaviour {

	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = player.transform.position;
	}
}
