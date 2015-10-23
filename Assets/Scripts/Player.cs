using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public AudioClip runClip;
	public AudioClip rotationClip;
	AudioSource audioSource;

	Vector3 newAngle;
	float frame;
	float angleDifference;
	int moveCount;

	const int MAX_MOVECOUNT = 25;
	const float MOVEMENT = 0.04f;

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();

		frame = 0;
		moveCount = MAX_MOVECOUNT;
		newAngle = new Vector3 (0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		frame++;

		if (isRunning ()) {
			gameObject.transform.Translate (new Vector3 (0f, 0f, MOVEMENT));
			moveCount++;
		}

		angleDifference = Mathf.DeltaAngle (transform.eulerAngles.y, newAngle.y);
		if(angleDifference < -0.1f){
			transform.Rotate(new Vector3 (0f, -5f, 0f));
		}else if(angleDifference > 0.1f){
			transform.Rotate(new Vector3 (0f, 5f, 0f));
		}

		if(!isActioning())
			audioSource.Stop ();
	}


	public void MoveFront()
	{
		moveCount = 0;
		audioSource.clip = runClip;
		audioSource.Play ();
	}
	
	public void RightRotate()
	{
		newAngle = new Vector3 (newAngle.y, newAngle.y + 90, 0f);
		angleDifference = Mathf.DeltaAngle (transform.eulerAngles.y, newAngle.y);
		audioSource.clip = rotationClip;
		audioSource.Play ();
	}
	public void LeftRotate()
	{
		newAngle = new Vector3 (newAngle.y, newAngle.y - 90, 0f);
		angleDifference = Mathf.DeltaAngle (transform.eulerAngles.y, newAngle.y);
		audioSource.clip = rotationClip;
		audioSource.Play ();
	}

	public bool isActioning()
	{
		return isRunning() || isRolling();
	}
	public bool isRolling()
	{
		return angleDifference < -0.1f || angleDifference > 0.1f;
	}
	public bool isRunning()
	{
		return moveCount < MAX_MOVECOUNT;
	}

	void OnCollisionEnter(Collision col){
		Debug.Log ("collision");
	}
}
