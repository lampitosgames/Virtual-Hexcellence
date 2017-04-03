using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.VR;

public class Player : MonoBehaviour {
	public int q, r, h;

    ////Materials for hexes being walked over
    public Material currenthexmaterial;
    public Material highlightMaterial;

    //Global controllers
    LevelController levelController;
    AIController aiController;
    UIController uiController;
    InventoryController inventoryController;
    //Relevant Gameobjects
    GameObject playerCamera;

    //State variables
    public List<List<PathCell>> movable = new List<List<PathCell>>();
    public bool playerMoving = false;
    public int actionPoints = 3;
	public bool vrActive = false;
	private bool vrMoveComplete = false;

	/// <summary>
    /// Unity's start() function called after the object is initialized
    /// </summary>
	void Start () {
		levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        levelController.player = this;
        aiController = GameObject.Find("AIController").GetComponent<AIController>();
        uiController = GameObject.Find("UIController").GetComponent<UIController>();
        inventoryController = GameObject.Find("InventoryController").GetComponent<InventoryController>();
		playerCamera = GetComponentInChildren<Camera> ().gameObject;
		if (GameObject.Find ("FPSController") == null) {
			vrActive = true;
		} else {
			//Disable everything VR related and fix FOV of Camera
			SteamVR.SafeDispose ();
			VRSettings.enabled = false;
			playerCamera.GetComponent<Camera> ().fieldOfView = 60;
		}
	}

	/// <summary>
    /// Unity's Update() function called once per step
    /// </summary>
	void Update () {
		
        //Get player position after checking whether the game is in VR
		if (vrActive) {
			int[] hexCoords = HexConst.CoordToHexIndex (new Vector3 (transform.position.x, uiController.transform.position.y, transform.position.z));
			q = hexCoords[0];
			r = hexCoords[1];
			h = hexCoords[2];
		} else{
			int[] hexCoords = HexConst.CoordToHexIndex (new Vector3 (transform.position.x, transform.position.y, transform.position.z));
			q = hexCoords[0];
			r = hexCoords[1];
			h = hexCoords[2];
		}


        //If player presses "m" to move
        if (Input.GetKeyUp("m")) {
            if (actionPoints > 0 && !playerMoving) {
                this.playerMoving = true;
                uiController.setVisibility(true);
            }
        }
    }

	/// <summary>
	/// This is called from the Controller input class on the camera rig
	/// </summary>
	public void onTouchpadUp(){
		if (actionPoints > 0 && !playerMoving) {
			this.playerMoving = true;
			uiController.setVisibility(true);
		}
	}

    /// <summary>
    /// Draw a GUI to the screen
    /// </summary>
    void OnGUI() {
        //Make a new background box
        GUI.skin.label.fontSize = 12;
        GUI.Box(new Rect(10, 10, 180, 150), "Actions: " + this.actionPoints);
        GUI.Label(new Rect(20, 40, 180, 20), "Press 'm' to move");
        GUI.Label(new Rect(20, 60, 180, 20), "Press 'up' to scale map");
        GUI.Label(new Rect(20, 80, 180, 20), "Click on the destination hex");
        GUI.Label(new Rect(20, 100, 180, 20), "Press 'e' to pick up nearby items.");
        GUI.Label(new Rect(20, 120, 180, 20), "Press 'x' to drop an item from your inventory.");
    }

    /// <summary>
    /// Called every update by the LevelController on the player's turn
    /// Returns true when the player's turn has ended
    /// </summary>
    /// <returns>is the player turn over?</returns>
    public bool TakeTurn() {
        if (playerMoving) {
            playerMoving = !MovePlayer();
        }
        if (actionPoints == 0) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Display UI for player movement
    /// </summary>
    /// <returns>Returns true when movement has happened</returns>
    public bool MovePlayer() {
        //If the player is standing on a hex (not falling, jumping)
        if (levelController[q, r, h] != null) {
            //Get the neighbors of the current hex
            movable = aiController.ReachableInSteps(new int[] { q, r, h }, 2, actionPoints);
            //Give all valid neighbors the neighbor material
            uiController.ShowValidMoves(movable);
            //Give the current hex the current hex material
            uiController[q, r, h].gameObject.GetComponent<Renderer>().material = currenthexmaterial;
        }

		//Non VR Movement
		if (!vrActive) {
			//Get the cell the player is looking at
			Vector3 lineOfSight = playerCamera.transform.forward * 1000;
			RaycastHit hit;
			if (Physics.Raycast (playerCamera.transform.position, lineOfSight, out hit)) {
				//Get the UI hex cell the player is looking at
				UICellObj hitObj = hit.transform.gameObject.GetComponent<UICellObj> () as UICellObj;

				//if it isn't null
				if (hitObj != null) {
					//get the selected cell
					PathCell lookedCell = aiController [hitObj.q, hitObj.r, hitObj.h];
                    for (int i = 0; i < movable.Count; i++) {
                        foreach (PathCell m in movable[i]) {
                            if (lookedCell.Equals(m) && !lookedCell.Equals(aiController[q, r, h])) {
                                //set the material
                                hitObj.gameObject.GetComponent<Renderer>().material = highlightMaterial;
                                //If the player clicks, move them there and end movement
                                if (Input.GetMouseButtonUp(0)) {
                                    transform.parent.transform.position = levelController[hitObj.q, hitObj.r, hitObj.h].centerPos;
                                    if (levelController[hitObj.q, hitObj.r, hitObj.h].hasGoal) {
                                        levelController.numOfGoals -= 1;
                                        levelController[hitObj.q, hitObj.r, hitObj.h].goal.GetComponent<Renderer>().enabled = false;
                                    }
                                    actionPoints -= i+1;
                                    uiController.ClearCells();
                                    uiController.setVisibility(false);
                                    return true;
                                }
                            }
                        }
                    }
				}
			}
		} 
		//VR Specific movement
		else {
			if (vrMoveComplete) {
				vrMoveComplete = false;
				return true;
			}
		}
        return false;
    }

	public void vrMove(Vector3 targetPosition){
		int[] figurePositionHex = HexConst.CoordToHexIndex (targetPosition);
		transform.position = levelController [figurePositionHex [0], figurePositionHex [1], figurePositionHex [2]].centerPos;
		actionPoints -= 1;
		uiController.ClearCells ();
		uiController.setVisibility (false);
		vrMoveComplete = true;
	}


}

