using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.VR;

/// <summary>
/// The player object in the game.
/// The player can be an FPS controller or a VR player
/// </summary>
public class Player : MonoBehaviour {
    //player coords
    public int q, r, h;

    public int[] PlayerCoords {
        get { int[] temp = { q, r, h }; return temp; }
    }

    //Materials for hexes being walked over
    public Material currenthexmaterial;
    public Material highlightMaterial;

    //Controller references
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
    public AbilityEnum currentAction = AbilityEnum.NOT_USING_ABILITIES_ATM; //Records the player's current ability state.
    public UICellObj gazedAt;
    public int currentActionCost = 0;

    //move this at some point
    public Material extraCrispyTileMaterial;

    /// <summary>
    /// Unity's start() function called after the object is initialized
    /// </summary>
    void Start() {
        //Get controller references
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        aiController = GameObject.Find("AIController").GetComponent<AIController>();
        uiController = GameObject.Find("UIController").GetComponent<UIController>();
        inventoryController = GameObject.Find("InventoryController").GetComponent<InventoryController>();

        //Pass self reference to level controller
        levelController.player = this;
        //Get a reference to the main game camera
        playerCamera = GetComponentInChildren<Camera>().gameObject;

        //Check for VR
        if (GameObject.Find("FPSController") == null) {
            vrActive = true;
        } else {
            //Disable everything VR related and fix FOV of Camera
            SteamVR.SafeDispose();
            VRSettings.enabled = false;
            playerCamera.GetComponent<Camera>().fieldOfView = 60;
        }
    }

    /// <summary>
    /// Unity's Update() function called once per step
    /// </summary>
    void Update() {
        //Get player position.  Different in VR
        if (vrActive) {
            int[] hexCoords = HexConst.CoordToHexIndex(new Vector3(transform.position.x, uiController.transform.position.y, transform.position.z));
            q = hexCoords[0];
            r = hexCoords[1];
            h = hexCoords[2];
        } else {
            int[] hexCoords = HexConst.CoordToHexIndex(new Vector3(transform.position.x, transform.position.y, transform.position.z));
            q = hexCoords[0];
            r = hexCoords[1];
            h = hexCoords[2];
        }

        //If player presses "m" to move, aren't yet moving, and have enough action points to do so
        if (Input.GetKeyUp("m") && actionPoints > 0 && !playerMoving) {
            //Start movement
            this.playerMoving = true;
            uiController.setVisibility(true);
        }
    }

    /// <summary>
    /// This is called from the Controller input class on the camera rig
    /// </summary>
    public void onTouchpadUp() {
        //Start movement
        if (actionPoints > 0 && !playerMoving) {
            this.playerMoving = true;
            uiController.setVisibility(true);
        }
    }

    /// <summary>
    /// Draw a GUI to the screen.  This is debug
    /// </summary>
    void OnGUI() {
        //Make a new background box
        GUI.skin.label.fontSize = 12;
        GUI.Box(new Rect(10, 10, 180, 150), "Actions: " + this.actionPoints + "\nAction cost: " + this.currentActionCost);
        GUI.Label(new Rect(20, 40, 180, 20), "Press 'm' to move");
        GUI.Label(new Rect(20, 60, 180, 20), "Press 'n' to toggle movemap off.");
        GUI.Label(new Rect(20, 80, 180, 20), "Click on the destination hex");
        GUI.Label(new Rect(20, 100, 180, 60), "Press '1' while the move map is\n up to switch to launching\na fireball");
        //GUI.Label(new Rect(20, 100, 180, 20), "Press 'e' to pick up nearby items.");
        //GUI.Label(new Rect(20, 120, 180, 20), "Press 'x' to drop an item from your inventory.");

    }

    /// <summary>
    /// Called every update by the LevelController on the player's turn
    /// Returns true when the player's turn has ended
    /// </summary>
    /// <returns>is the player turn over?</returns>
    public bool TakeTurn() {
        if (playerMoving) {
            //Testing the ability selection
            if (Input.GetKey(KeyCode.Alpha1)) {
                uiController.ClearCells();
                testFireballTargeting();
            } else {
                uiController.ClearCells();
                MovePlayer();
            }
            //If the player lets go of the left mouse button, act.
            if (Input.GetMouseButtonUp(0))
            {
                playerMoving=!onActionTaken();
            }
        }
        if (actionPoints == 0) {
            return true;
        }
        return false;
    }

    //Meant to act as a way to more appropriately implement context-based ability usage.
    public bool onActionTaken()
    {
        //Call an appropriate ability execution based on the player's current action 
        //Will likely replace that implementation with delegating abilities from the inventory directly?
        switch (currentAction)
        {
            //invalid action--if we're on the minimap screen and try to act on an invalid tile...
            case AbilityEnum.INVALID_ACTION: Debug.Log("That's not a valid tile to target."); break;

            //move the player to the gazed-at tile
            #region Move Player
            case AbilityEnum.MOVE_PLAYER:
                Debug.Log("Motion is all.");
                transform.parent.transform.position = levelController[gazedAt.q, gazedAt.r, gazedAt.h].centerPos;
                if (levelController[gazedAt.q, gazedAt.r, gazedAt.h].hasGoal)
                {
                    levelController.numOfGoals -= 1;
                    levelController[gazedAt.q, gazedAt.r, gazedAt.h].goal.SetActive(false);
                }
                actionPoints -= currentActionCost;
                uiController.ClearCells();
                uiController.setVisibility(false);

                return true;
            #endregion

            //casts an insanely overpowered fireball at a target location.
            case AbilityEnum.FIREBALL: Debug.Log("BURN STUFF!");
                PathCell targetedCell = aiController[gazedAt.q, gazedAt.r, gazedAt.h];

                //kill any monsters on the cells you target
                foreach (Monster m in aiController.monsters)
                {
                    PathCell monsterLoc = aiController.pathGrid[m.CurrentCell[0], m.CurrentCell[1], m.CurrentCell[2]];
                    if (aiController.DistBetween(targetedCell,monsterLoc)<=1)
                    {
                        m.gameObject.GetComponent<MonsterStats>().Health-=9001;
                    }
                }

                //now turn the remaining tiles extra crispy
                PathCell[] surroundingCells = aiController.pathGrid.GetRadius(gazedAt.q, gazedAt.r, gazedAt.h,1,-1,true);
                foreach (PathCell cell in surroundingCells)
                {
                    HexCellData cellData = levelController.levelGrid[cell.q, cell.r, cell.h];
                    cellData.hexCellObject.gameObject.GetComponent<Renderer>().material = this.extraCrispyTileMaterial;
                }

                actionPoints -= currentActionCost;
                uiController.ClearCells();
                uiController.setVisibility(false);
                return true;
            default: break;
        }

        return false;
    }

    /// <summary>
    /// Update the UI for player movement and item usage
    /// </summary>
    /// <returns>Returns true when movement has happened</returns>
    public bool MovePlayer() {
        #region Check to see if the player is standing on a hex; if so show all valid moves on the minimap.
        //If the player is standing on a hex (not falling, jumping)
        if (levelController[q, r, h] != null) {
            //Get the neighbors of the current hex
            movable = aiController.ReachableInSteps(new int[] { q, r, h }, 2, actionPoints);
            //Give all valid neighbors the neighbor material
            uiController.ShowValidMoves(movable);
            //Give the current hex the current hex material
            uiController[q, r, h].gameObject.GetComponent<Renderer>().material = currenthexmaterial;
        }
        #endregion

        //Actions are considered invalid until they are proven valid.
        currentAction = AbilityEnum.INVALID_ACTION;

        //Non VR Movement
        if (!vrActive) {
            //Get the cell the player is looking at
            Vector3 lineOfSight = playerCamera.transform.forward * 1000;
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, lineOfSight, out hit)) {
                //Get the UI hex cell the player is looking at
                UICellObj hitObj = hit.transform.gameObject.GetComponent<UICellObj>() as UICellObj;

                //if it isn't null
                if (hitObj != null) {
                    //get the selected cell
                    PathCell lookedCell = aiController[hitObj.q, hitObj.r, hitObj.h];
                    PathCell startCell = aiController[q, r, h];

                    //If the cell is within our available move distance, loop through the cells in our list of cells we can move to.
                    //This is arguably faster than the alternative implementation (getting the distance away as the wolf runs directly
                    //via A*) 
                    #region Check to see if our tile is "on the list" and act accordingly
                    if (aiController.DistBetween(lookedCell, startCell) <= movable.Count*2)
                    {
                        //loop through all cells we're close enough to reach
                        for (int i = 0; i < movable.Count; i++)
                        {
                            foreach (PathCell m in movable[i])
                            {
                                if (lookedCell.Equals(m) && !lookedCell.Equals(startCell))
                                {
                                    //set the material
                                    hitObj.gameObject.GetComponent<Renderer>().material = highlightMaterial;

                                    //Our suggested action is valid and will move the player
                                    currentAction = AbilityEnum.MOVE_PLAYER;

                                    //Set the player's gaze to the current tile.
                                    gazedAt = hitObj;

                                    //Set the player's current action cost
                                    currentActionCost = i + 1;

                                    //If the player clicks, move them there and end movement
                                    //Refactored to onMove; left in comments for documentation purposes.
                                    #region refactored-out mouse input code
                                    /*if (Input.GetMouseButtonUp(0))
                                    {
                                        transform.parent.transform.position = levelController[hitObj.q, hitObj.r, hitObj.h].centerPos;
                                        if (levelController[hitObj.q, hitObj.r, hitObj.h].hasGoal)
                                        {
                                            levelController.numOfGoals -= 1;
                                            levelController[hitObj.q, hitObj.r, hitObj.h].goal.SetActive(false);
                                        }
                                        actionPoints -= i + 1;
                                        uiController.ClearCells();
                                        uiController.setVisibility(false);
                                        return true;
                                    }*/
                                    #endregion
                                }
                            }
                        }
                    }
                    #endregion


                    //Alternative implementation
                    //Gets the path between the cell and the looked-at cell, then uses the distance between them
                    //as a basis for move costs. Left in comments for documentation purposes.
                    #region code for alternative implementation (note: commented out.)
                    //Check to see if the current cell is a valid move.
                    //Start by getting the path between them.
                    /*List<int[]> testPath = aiController.PathBetween(startCell.CellCoords, lookedCell.CellCoords);
                    if (testPath != null)
                    {
                        int testDistance = Mathf.Max(testPath.Count - 1, 0);
                        Debug.Log(testDistance);
                        if (testDistance <= actionPoints * 2 && testDistance > 0)
                        {
                            hitObj.gameObject.GetComponent<Renderer>().material = highlightMaterial;
                            currentAction = AbilityEnum.MOVE_PLAYER;

                            //If the player clicks, move them there and end movement
                            if (Input.GetMouseButtonUp(0))
                            {
                                transform.parent.transform.position = levelController[hitObj.q, hitObj.r, hitObj.h].centerPos;
                                if (levelController[hitObj.q, hitObj.r, hitObj.h].hasGoal)
                                {
                                    levelController.numOfGoals -= 1;
                                    levelController[hitObj.q, hitObj.r, hitObj.h].goal.SetActive(false);
                                }
                                actionPoints -= ((testDistance + 1) / 2); //remove action points equal.
                                uiController.ClearCells();
                                uiController.setVisibility(false);
                                return true;
                            }
                        }
                        }*/
                    #endregion
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

    public void vrMove(Vector3 targetPosition) {
        int[] figurePositionHex = HexConst.CoordToHexIndex(targetPosition);
        transform.position = levelController[figurePositionHex[0], figurePositionHex[1], figurePositionHex[2]].centerPos;
        actionPoints -= 1;
        uiController.ClearCells();
        uiController.setVisibility(false);
        vrMoveComplete = true;
    }

    //Testing methods for calling/using abilities.
    //Currently hooking these up in Player itself as a temporary thing.
    //I'll make sure these work with everything later; for now I just want to have some sort of abilities I can call
    //and a way to test the "select within a radius" functionality.
    //Ability effect not functional; ability targeting is.
    public bool testFireballTargeting() {
        //If the player is standing on a hex (not falling, jumping)
        if (levelController[q, r, h] != null) {
            //Get the area around the current hex
            uiController.ShowValidTopDownRadius(q, r, h, 5, true);
        }

        //Actions are considered invalid until they are proven valid.
        currentAction = AbilityEnum.INVALID_ACTION;

        //I don't know how to do the VR version; will likely test that whenever I get the opportunity.

        //Get the cell the player is looking at
        Vector3 lineOfSight = playerCamera.transform.forward * 1000;
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, lineOfSight, out hit)) {
            //Get the UI hex cell the player is looking at
            UICellObj hitObj = hit.transform.gameObject.GetComponent<UICellObj>() as UICellObj;

            //if it isn't null
            if (hitObj != null)
            {
                //get the selected cell; if it's within 5 units of the starting cell show the ability AoE
                PathCell lookedCell = aiController[hitObj.q, hitObj.r, hitObj.h];
                PathCell startCell = aiController[q, r, h];

                //if this is in the right range to cast this, it's a valid move.
                if (aiController.DistBetween(lookedCell, startCell) <= 5)
                {
                    uiController.ShowValidTopDownRadius(hitObj.q, hitObj.r, hitObj.h, 1, true, TargetingMaterial.TARGETED_ZONE);
                    currentAction = AbilityEnum.FIREBALL;
                    gazedAt = hitObj;
                    currentActionCost = 1;
                }
            }
        }
        return true;
    }

    //Other planned abilities:
    //Ice Wall--will likely involve multiple, sequential targeting inputs
    //Vine Surge--launches a line of vines that root enemies.
}

