using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public int q, r, h;

    //Materials for hexes being walked over
    public Material currenthexmaterial;
    public Material normalhexmaterial;
    public Material neighborhexmaterial;
    public Material highlightMaterial;

    //Global controllers
    LevelController levelController;
    AIController aiController;
    //Relevant Gameobjects
    GameObject playerCamera;

    //State variables
    public List<AICell> movable = new List<AICell>();
    public bool playerMoving = false;
    public int actionPoints = 3;


	// Use this for initialization
	void Start () {
		levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        levelController.player = this;
        aiController = GameObject.Find("AIController").GetComponent<AIController>();
        playerCamera = this.transform.FindChild("FirstPersonCharacter").gameObject as GameObject;
	}

	// Update is called once per frame
	void Update () {
        //Get player position
        int[] hexCoords = HexConst.CoordToHexIndex(new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z));
        q = hexCoords[0];
        r = hexCoords[1];
        h = hexCoords[2];

        //If player presses "m" to move
        if (Input.GetKeyUp("m")) {
            if (actionPoints > 0 && !playerMoving) {
                this.playerMoving = true;
            }
        }
    }

    void OnGUI() {
        //Make a new background box
        GUI.Box(new Rect(10, 10, 140, 90), "Actions: " + this.actionPoints);
        GUI.Label(new Rect(20, 40, 120, 20), "Press 'm' to move");
    }

    public bool TakeTurn() {
        //Reset all neighbor cells
        foreach (AICell cell in movable) {
            HexCellData hexCell = levelController[cell.q, cell.r, cell.h];
            hexCell.hexCellObject.GetComponent<Renderer>().material = normalhexmaterial;
        }

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
        //Get the neighbors of the current hex
        if (aiController[q, r, h] != null) {
            movable = aiController.ReachableInSteps(new int[] { q, r, h }, 2);
        }
        //Give all valid neighbors the neighbor material
        foreach (AICell cell in movable) {
            HexCellData hexCell = levelController[cell.q, cell.r, cell.h];
            hexCell.hexCellObject.GetComponent<Renderer>().material = neighborhexmaterial;
        }

        //If the player is standing on a hex (not falling, jumping)
        if (levelController[q,r,h] != null) {
            //Give the current hex the current hex material
            levelController[q, r, h].hexCellObject.GetComponent<Renderer>().material = currenthexmaterial;
        }

        //Get the cell the player is looking at
        //TODO: Delete this absolute abomination of nested code
        Vector3 lineOfSight = playerCamera.transform.forward * 1000;
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, lineOfSight, out hit)) {
            HexCellObj hitObj = hit.transform.gameObject.GetComponent<HexCellObj>() as HexCellObj;
            if (hitObj != null) {
                if (aiController[hitObj.q, hitObj.r, hitObj.h] != null) {
                    //get the selected cell
                    AICell lookedCell = aiController[hitObj.q, hitObj.r, hitObj.h];
                    foreach (AICell m in movable) {
                        if (lookedCell.Equals(m) && !lookedCell.Equals(aiController[q, r, h])) {
                            //set the material
                            levelController[hitObj.q, hitObj.r, hitObj.h].hexCellObject.gameObject.GetComponent<Renderer>().material = highlightMaterial;
                            //If the player clicks, move them there and end movement
                            if (Input.GetMouseButtonUp(0)) {
                                gameObject.transform.position = HexConst.HexToWorldCoord(hitObj.q, hitObj.r, hitObj.h) + new Vector3(0, 0.8f, 0);
                                actionPoints -= 1;
                                //Reset all neighbor cells
                                foreach (AICell cell in movable) {
                                    HexCellData hexCell = levelController[cell.q, cell.r, cell.h];
                                    hexCell.hexCellObject.GetComponent<Renderer>().material = normalhexmaterial;
                                }
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
}
