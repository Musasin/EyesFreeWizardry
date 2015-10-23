using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TouchSystem : MonoBehaviour {

	Vector3 cursorPos;
	Vector3 firstPos;
	//Vector3 lastPos;
	Vector3 acceleration;
	List<Vector3> tracksList = new List<Vector3>();
	List<Vector3> accelerationList = new List<Vector3> ();

	GameObject player;
	Player playerComponent;

	double movement;
	double time = 0;
	int holdFrame = 0;
	int waitFrame;

	double firstTouchTime;

	enum Mode { 
		NOTHING,
		TOUCH,
		SWIPE,
		SEPARATE,
		END
	};
	Mode nowMode = Mode.NOTHING;

	void Start () {
		player = GameObject.Find ("Player");
		playerComponent = player.GetComponent<Player> ();
		GameObject.Find ("MapCamera").SetActive(false);

		Application.targetFrameRate = 50;
	}

	void Update () {
		if(nowMode != Mode.END)
			time += Time.deltaTime;
		//Debug.Log (1 / Time.deltaTime);
	}

	void FixedUpdate () {
		
		if(nowMode != Mode.END)
			nowMode = modeCheck ();

		switch (nowMode) {
		case Mode.NOTHING: 	updateNothing();  break;
		case Mode.TOUCH: 	updateTouch();    break;
		case Mode.SWIPE: 	updateSwipe();    break;
		case Mode.SEPARATE: updateSeparate(); break;
		case Mode.END: 		updateEnd();	  break;
		default: Debug.Assert(false && "Illegal State!"); break;
		}
	}
	
	Mode modeCheck()
	{
		if (isTouching ()) {
			if (holdFrame == 0)	return Mode.TOUCH;
			else				return Mode.SWIPE;
		} else {
			if (holdFrame == 0)	return Mode.NOTHING;
			else				return Mode.SEPARATE;
		}
	}
	
	bool isTouching(){
		return (Input.GetMouseButton (0) || Input.touchCount > 0);
	}

	void updateNothing(){
		waitFrame++;
	}

	void updateTouch(){
		holdFrame++;
		addTrack();

		waitFrame = 0;
		firstPos = cursorPos;
		firstTouchTime = time;
	}
	void updateSwipe(){
		holdFrame++;
		addTrack();

		acceleration = tracksList[tracksList.Count - 1] - tracksList[tracksList.Count - 2];
		accelerationList.Add (acceleration);
		movement += Math.Abs(acceleration.x) + Math.Abs(acceleration.y);
	}

	void updateSeparate(){


		swipeCheck ();

		tapCheck ();

		movement = 0;
		holdFrame = 0;
		tracksList.Clear ();
		accelerationList.Clear ();
		acceleration = new Vector3(0,0,0);
	}

	void swipeCheck()
	{
		double averageAccel = 0;
		if (accelerationList.Count < 3)
			return;

		for (int i = accelerationList.Count - 2; i < accelerationList.Count; i++) {
			averageAccel += accelerationList[i].x;
		}

		Debug.Log (averageAccel);

		if (!playerComponent.isActioning ()) {
			if (averageAccel < -5)
				playerComponent.RightRotate ();
			if (averageAccel > 5)
				playerComponent.LeftRotate ();
		}

	}

	void tapCheck()
	{
		if (time < firstTouchTime + 0.5 && !isMoving (acceleration)) {
			if(!playerComponent.isActioning())
				playerComponent.MoveFront ();
		}
	}

	void updateEnd(){
	}

	void addTrack(){
		cursorPos = updateNowPos ();
		tracksList.Add (cursorPos);
	}
	Vector3 updateNowPos()
	{
		//UpdatePos (Mouse)
		if(Input.GetMouseButton (0))
			return Input.mousePosition;
		
		//UpdatePos (Touch)
		if(Input.touchCount > 0)
			return Input.GetTouch(0).position;
		
		return new Vector3(0,0,0);
	}

	
	bool isMoving(Vector3 acceleration)
	{
		return System.Math.Abs (acceleration.x + acceleration.y) > 1;
	}
}
