using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
[RequireComponent(typeof(SteamVR_TrackedObject))]

public class ControllerInput : MonoBehaviour {

	SteamVR_TrackedObject trackedObj;
	SteamVR_Controller.Device device;
	Player player;
	UIController uiController;


	public float edgeThreshold = 0.4f;
	public float upDownThreshold = 0.7f;

	// Use this for initialization
	void Awake () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Player> ();
		uiController = GameObject.Find ("UIController").GetComponent<UIController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		device = SteamVR_Controller.Input ((int)trackedObj.index);
	}

	void Update(){
		if (device.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {
			Vector2 touchpad = (device.GetAxis (Valve.VR.EVRButtonId.k_EButton_Axis0));
			print (touchpad);
			if (player.currentAction == AbilityEnum.NOT_USING_ABILITIES) {
				player.StartMove ();
			} else {
				//Up
				if (touchpad.y > edgeThreshold && touchpad.x > -upDownThreshold && touchpad.x < upDownThreshold) {
					print ("UP");
					player.StartMove();
				//Right
				} else if (touchpad.x > edgeThreshold && touchpad.y < upDownThreshold && touchpad.y > -upDownThreshold) {
					print ("RIGHT");
				//Down
				} else if (touchpad.y < -edgeThreshold && touchpad.x > -upDownThreshold && touchpad.x < upDownThreshold) {
					print ("DOWN");
					player.CancelAction ();
				//Left
				} else if (touchpad.x < -edgeThreshold && touchpad.y < upDownThreshold && touchpad.y > -upDownThreshold) {
					print ("LEFT");
					player.StartFireball ();
				}
			}
		}

		if (device.GetPressDown (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
			player.vrPressDown = true;
		}

		if (device.GetPressUp (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
			player.vrPressUp = true;
		}
		if (device.GetPress (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
			print ("do something");
			//add arrow/behind back check here
			//if () {
			// player.CancelAction()
			// //equip arrow to hand
			// } else {
			switch (player.currentAction) {
			case AbilityEnum.MOVE_PLAYER:
				//draw a line
				//Get the cell the player is looking at
				RaycastHit moveHit;
				if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out moveHit)) {
					//Get the UI hex cell the player is looking at
					UICellObj hitMoveObj = moveHit.transform.gameObject.GetComponent<UICellObj>() as UICellObj;
					if (hitMoveObj != null) {
						player.VRHitObj = hitMoveObj;
					}
				}
				break;
			case AbilityEnum.FIREBALL:
				RaycastHit fireballHit;
				if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out fireballHit)) {
					//Get the UI hex cell the player is looking at
					UICellObj hitFireballObj = fireballHit.transform.gameObject.GetComponent<UICellObj>() as UICellObj;
					if (hitFireballObj != null) {
						player.VRHitObj = hitFireballObj;
					}
				}
				break;
			default:
				break;
			}





		}


	}
}
