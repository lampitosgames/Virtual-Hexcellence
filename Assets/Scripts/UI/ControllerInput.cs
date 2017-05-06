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

	public static bool controllerInUse = false;
	public bool thisControllerInUse = false;
	public bool validHex = false;


	public float edgeThreshold = 0.4f;
	public float upDownThreshold = 0.7f;

	// Use this for initialization
	void Awake () {
		if (gameObject.GetComponent<LineRenderer> () != null) {
			Destroy(gameObject.GetComponent<LineRenderer> ());
		}
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Player> ();
		uiController = GameObject.Find ("UIController").GetComponent<UIController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		device = SteamVR_Controller.Input ((int)trackedObj.index);
	}

	void Update(){
		if (!controllerInUse || thisControllerInUse) {
			if (device.GetPressUp (SteamVR_Controller.ButtonMask.Touchpad)) {
				Vector2 touchpad = (device.GetAxis (Valve.VR.EVRButtonId.k_EButton_Axis0));
				controllerInUse = true;
				thisControllerInUse = true;
				if (player.currentAction == AbilityEnum.NOT_USING_ABILITIES) {
					player.StartMove ();
				} else {
					//Up
					if (touchpad.y > edgeThreshold && touchpad.x > -upDownThreshold && touchpad.x < upDownThreshold) {
						player.StartMove ();
						//Right
					} else if (touchpad.x > edgeThreshold && touchpad.y < upDownThreshold && touchpad.y > -upDownThreshold) {
						//Down
					} else if (touchpad.y < -edgeThreshold && touchpad.x > -upDownThreshold && touchpad.x < upDownThreshold) {
						player.CancelAction ();
						if (gameObject.GetComponent<LineRenderer> () != null) {
							Destroy (gameObject.GetComponent<LineRenderer> ());
						}
						controllerInUse = false;
						thisControllerInUse = false;
						//Left
					} else if (touchpad.x < -edgeThreshold && touchpad.y < upDownThreshold && touchpad.y > -upDownThreshold) {
						player.StartFireball ();
					}
				}
			}

			if (device.GetPressUp (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger) && thisControllerInUse && player.validVRHitObj) {
				controllerInUse = false;
				thisControllerInUse = false;
				player.vrPressUp = true;
				player.validVRHitObj = false;
				Debug.Log ("Pressed up");
				if (gameObject.GetComponent<LineRenderer> () != null) {
					Destroy (gameObject.GetComponent<LineRenderer> ());
				}
			}

			if (thisControllerInUse) {
				switch (player.currentAction) {
				case AbilityEnum.MOVE_PLAYER:
					//draw a line
					//Get the cell the player is looking at
					RaycastHit moveHit;
					if (Physics.Raycast (gameObject.transform.position, new Vector3 (0, -1, 0), out moveHit)) {
						//Get the UI hex cell the player is looking at
						UICellObj hitMoveObj = moveHit.transform.gameObject.GetComponent<UICellObj> () as UICellObj;
						if (hitMoveObj != null) {
							player.VRHitObj = hitMoveObj;
							DrawLine (transform.position, hitMoveObj.gameObject.transform.position);
						} else {
							if (gameObject.GetComponent<LineRenderer> () != null) {
								Destroy (gameObject.GetComponent<LineRenderer> ());
							}
						}
					}
					break;
				case AbilityEnum.FIREBALL:
					RaycastHit fireballHit;
					if (Physics.Raycast (gameObject.transform.position, new Vector3 (0, -1, 0), out fireballHit)) {
						//Get the UI hex cell the player is looking at
						UICellObj hitFireballObj = fireballHit.transform.gameObject.GetComponent<UICellObj> () as UICellObj;
						if (hitFireballObj != null) {
							player.VRHitObj = hitFireballObj;
							DrawLine (transform.position, hitFireballObj.gameObject.transform.position);
						} else {
							if (gameObject.GetComponent<LineRenderer> () != null) {
								Destroy (gameObject.GetComponent<LineRenderer> ());
							}
						}
					}
					break;
				default:
					break;
				}
			}
		}
	}

	void DrawLine(Vector3 pos1, Vector3 pos2) {
		if (gameObject.GetComponent<LineRenderer> () == null) {
			gameObject.AddComponent<LineRenderer> ();
		}
		LineRenderer line = gameObject.GetComponent<LineRenderer> ();
		line.SetPosition (0, new Vector3(pos1.x,pos1.y,pos1.z));
		line.SetPosition (1, pos2);
		line.widthMultiplier = 0.01f;
	}
}
