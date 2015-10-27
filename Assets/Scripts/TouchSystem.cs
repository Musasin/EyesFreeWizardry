using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TouchSystem : MonoBehaviour {

	public bool permissionAccel = true;
	public bool permissionFlick = true;
	public bool permissionSideTap = true;

	[SerializeField] GameObject leftSide;
	[SerializeField] GameObject rightSide;
	[SerializeField] GameObject pauseMenu;
	[SerializeField] GameObject player;
	[SerializeField] GameObject mapCamera;

	public void SetPermissionAccel(){
		permissionAccel = GameObject.Find ("accelToggle").GetComponent<Toggle> ().isOn;
	}
	public void SetPermissionFlick(){
		permissionFlick = GameObject.Find ("flickToggle").GetComponent<Toggle> ().isOn;
	}
	public void SetPermissionSideTap(){
		permissionSideTap = GameObject.Find ("sideTapToggle").GetComponent<Toggle> ().isOn;
		leftSide.SetActive (permissionSideTap);
		rightSide.SetActive (permissionSideTap);
	}

	Vector3 cursorPos;
	Vector3 firstPos;
	//Vector3 lastPos;
	Vector3 acceleration;
	List<Vector3> tracksList = new List<Vector3>();
	List<Vector3> accelerationList = new List<Vector3> ();

	Vector3 terminalAcc = Vector3.zero;

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
		PAUSE,
		END
	};
	Mode nowMode = Mode.NOTHING;

	void Start () {
		pauseMenu.SetActive (false);
		playerComponent = player.GetComponent<Player> ();

		mapCamera.SetActive(false);

		Application.targetFrameRate = 50;
	}

	void Update () {
		if (nowMode != Mode.END && nowMode != Mode.PAUSE) {
			time += Time.deltaTime;
			nowMode = modeCheck ();
		}



		if(permissionAccel)
			accelerationCheck ();

		switch (nowMode) {
		case Mode.NOTHING: 	updateNothing();  break;
		case Mode.TOUCH: 	updateTouch();    break;
		case Mode.SWIPE: 	updateSwipe();    break;
		case Mode.SEPARATE: updateSeparate(); break;
		case Mode.PAUSE:    updatePause();    break;
		case Mode.END: 		updateEnd();	  break;
		default: Debug.Assert(false && "Illegal State!"); break;
		}
	}
	
	Mode modeCheck()
	{
		if (Input.touchCount == 3) {
			pauseMenu.SetActive(true);
			return Mode.PAUSE;
		}

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

		if(permissionFlick)
			flickCheck ();

		tapCheck ();

		movement = 0;
		holdFrame = 0;
		tracksList.Clear ();
		accelerationList.Clear ();
		acceleration = new Vector3(0,0,0);
	}

	void accelerationCheck()
	{
		if (playerComponent.isActioning ())
			return;
		if(Input.acceleration.x >= 0.8)
			playerComponent.RightRotate ();
		else if(Input.acceleration.x <= -0.8)
			playerComponent.LeftRotate ();

	}

	void flickCheck()
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
		if (canTapAction())
			playerComponent.MoveFront ();
	}
	public void tapLeftSide()
	{
		if (canTapAction())
			playerComponent.LeftRotate();
	}
	public void tapRightSide()
	{
		if (canTapAction())
			playerComponent.RightRotate();
	}

	bool canTapAction()
	{
		return (time < firstTouchTime + 0.5 && !isMoving (acceleration) && !playerComponent.isActioning());
	}
	
	void updatePause(){
		if (Input.touchCount == 1) {
			nowMode = Mode.NOTHING;
			pauseMenu.SetActive (false);
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
