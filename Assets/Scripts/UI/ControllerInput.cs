using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using VRTK;
[RequireComponent(typeof(SteamVR_TrackedObject))]

public class ControllerInput : MonoBehaviour {

	SteamVR_TrackedObject trackedObj;
	SteamVR_Controller.Device device;
	Player player;
    UIController uiCont;
    InventoryController invCont;
    Satchel satchel;

    private GameObject attachedObject;
    bool controllerAssigned = false;

	// Use this for initialization
	void Awake () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
        uiCont = GameObject.Find("UIController").GetComponent<UIController>();
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		device = SteamVR_Controller.Input ((int)trackedObj.index);
        if(controllerAssigned == false)
        {
            uiCont.AssignControllers(trackedObj.gameObject);
            controllerAssigned = true;
        }
	}

	void Update(){
		if (device.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {
			Vector2 touchpad = (device.GetAxis (Valve.VR.EVRButtonId.k_EButton_Axis0));
			if (touchpad.y > 0.7f) {
				print ("UP");
				player.onTouchpadUp();
			}
            else if(touchpad.x > 0.7f)
            {
                uiCont.toggleUserInterface();
            }
		}
        
	}
    
}
