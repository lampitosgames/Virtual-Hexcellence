using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
        get { return new int[] { q, r, h }; }
    }

    //VR Stuff
    public bool AttatchedToVRPlayer;
    private bool vrActive = false;
    private bool vrMoveComplete = false;

    //Materials for hexes being walked over
    public Material currenthexmaterial;
    public Material highlightMaterial;
    //move this at some point
    public Material burntTileMaterial;

    //Controller references
    LevelController levelController;
    AIController aiController;
    UIController uiController;

    //Relevant Gameobjects
    public GameObject playerCamera;


    public UICellObj VRHitObj = null;
    public bool vrPressDown = false;
    public bool vrPressUp = false;

    public bool playerActing = false;

    //Actions (Moving, attacking, abilities, etc.)
    public List<List<PathCell>> movable = new List<List<PathCell>>();
    public int actionPoints = 3;
    public AbilityEnum currentAction = AbilityEnum.NOT_USING_ABILITIES;

    //Hard-coded inventory
    public bool hasBow = false;
    public bool hasFireball = false;

    /// <summary>
    /// Unity's start() function called after the object is initialized
    /// </summary>
    void Start() {
        //Get controller references
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        aiController = GameObject.Find("AIController").GetComponent<AIController>();
        uiController = GameObject.Find("UIController").GetComponent<UIController>();

        //Get a reference to the main game camera
        playerCamera = GetComponentInChildren<Camera>().gameObject;

        //Check for VR
        if (PlayerSettings.virtualRealitySupported == true) {
            vrActive = true;
            if (!AttatchedToVRPlayer) {
                Destroy(this.gameObject);
            } else {
                levelController.player = this;
            }
        } else {
            vrActive = false;
            if (AttatchedToVRPlayer) {
                Destroy(this.gameObject);
            } else {
                levelController.player = this.gameObject.GetComponent<Player>();
            }
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
            int[] hexCoords = HexConst.CoordToHexIndex(new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z));
            q = hexCoords[0];
            r = hexCoords[1];
            h = hexCoords[2];
        } else {
            int[] hexCoords = HexConst.CoordToHexIndex(new Vector3(transform.position.x, transform.position.y, transform.position.z));
            q = hexCoords[0];
            r = hexCoords[1];
            h = hexCoords[2];
        }

        //If player presses "m" to start acting, isn't yet acting, and has enough action points to do so
        if (Input.GetKeyUp("m")) {
            StartMove();
        }
        if (Input.GetKeyUp(KeyCode.Alpha1) && actionPoints > 0 && this.hasFireball) {
            StartFireball();
        }
        //Cancel abilities
        if (Input.GetKeyUp(KeyCode.Escape)) {
            CancelAction();
        }
    }

    /// <summary>
    /// Called to start movement
    /// </summary>
	public void StartMove() {
        //Start actions
        if (actionPoints > 0) {
            //Start action
            this.playerActing = true;
            this.currentAction = AbilityEnum.MOVE_PLAYER;
            uiController.setVisibility(true);
        }
    }

    /// <summary>
    /// Called to start casting fireball
    /// </summary>
    public void StartFireball() {
        if (actionPoints > 0 && this.hasFireball) {
            this.playerActing = true;
            this.currentAction = AbilityEnum.FIREBALL;
            uiController.setVisibility(true);
        }
    }

    public void CancelAction() {
        this.playerActing = false;
        this.currentAction = AbilityEnum.NOT_USING_ABILITIES;
        uiController.setVisibility(false);
    }

    /// <summary>
    /// Draw a GUI to the screen.  This is debug
    /// </summary>
    void OnGUI() {
        //Make a new background box
        GUI.skin.label.fontSize = 12;
        GUI.Box(new Rect(10, 10, 180, 150), "Actions: " + this.actionPoints);
        GUI.Label(new Rect(20, 40, 180, 20), "Press 'm' to move");
        GUI.Label(new Rect(20, 60, 180, 60), "Press '1' to cast fireball");
        GUI.Label(new Rect(20, 80, 180, 20), "Click on the targeted hex");
        GUI.Label(new Rect(20, 100, 180, 60), "Click to shoot arrows");
        GUI.Label(new Rect(20, 120, 180, 20), "Press 'esc' to cancel actions");

    }

    /// <summary>
    /// Called every update by the LevelController on the player's turn
    /// Returns true when the player's turn has ended
    /// </summary>
    /// <returns>is the player turn over?</returns>
    public bool TakeTurn() {
        //If the player is acting
        if (playerActing) {
            uiController.ClearCells();
            switch (currentAction) {
                case AbilityEnum.MOVE_PLAYER:
                    playerActing = !MovePlayer();
                    break;
                case AbilityEnum.FIREBALL:
                    playerActing = !Fireball();
                    break;
                default:
                    break;
            }
        }
        if (actionPoints == 0) {
            return true;
        }
        vrPressDown = false;
        vrPressUp = false;
        return false;
    }

    /// <summary>
    /// Update the UI for player movement and item usage
    /// </summary>
    /// <returns>Returns true when movement has happened</returns>
    public bool MovePlayer() {
        //Check to see if the player is standing on a hex; if so show all valid moves on the minimap.
        //If the player is standing on a hex (not falling, jumping)
        if (levelController[q, r, h] != null) {
            //Get the neighbors of the current hex
            movable = aiController.ReachableInSteps(new int[] { q, r, h }, 2, actionPoints);
            //Give all valid neighbors the neighbor material
            uiController.ShowValidMoves(movable);
            //Give the current hex the current hex material
            uiController[q, r, h].SetMaterial(currenthexmaterial, true);
        }

        UICellObj hitObj = null;

        //Non VR Movement
        if (!vrActive) {
            //Get the cell the player is looking at
            Vector3 lineOfSight = playerCamera.transform.forward * 1000;
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, lineOfSight, out hit)) {
                //Get the UI hex cell the player is looking at
                hitObj = hit.transform.gameObject.GetComponent<UICellObj>() as UICellObj;
            }
            //VR Specific movement
        } else {
            hitObj = VRHitObj;
        }

        //if it isn't null
        if (hitObj != null) {
            //get the selected cell
            PathCell lookedCell = aiController[hitObj.q, hitObj.r, hitObj.h];
            PathCell startCell = aiController[q, r, h];

            //loop through all cells we're close enough to reach
            for (int i = 0; i < movable.Count; i++) {
                foreach (PathCell m in movable[i]) {
                    if (lookedCell.Equals(m) && !lookedCell.Equals(startCell)) {
                        //set the material
                        uiController[hitObj.q, hitObj.r, hitObj.h].SetMaterial(highlightMaterial, true);
                        GameObject thisUICell = uiController[lookedCell.q, lookedCell.r, lookedCell.h].gameObject;
                        uiController.playerFigure.transform.position = thisUICell.transform.position + new Vector3(0, thisUICell.GetComponent<Renderer>().bounds.size.y / 2, 0);
                        //If the player clicked the mouse
                        if (Input.GetMouseButtonUp(0) || vrPressUp) {
                            //Move the player
                            transform.parent.position = levelController[hitObj.q, hitObj.r, hitObj.h].centerPos;
                            //If the target has a goal
                            if (levelController[hitObj.q, hitObj.r, hitObj.h].hasGoal) {
                                //Update the goal
                                levelController.numOfGoals -= 1;
                                levelController[hitObj.q, hitObj.r, hitObj.h].goal.GetComponent<Goal>().Accomplished();
                            }
                            //Reduce number of player action points remaining
                            actionPoints -= i + 1;
                            //Clear the UI controller
                            uiController.ClearCells();
                            uiController.setVisibility(false);
                            currentAction = AbilityEnum.NOT_USING_ABILITIES;
                            return true;
                        }
                    }
                }
            }
        }

        //Movement not finished
        return false;
    }

    /// <summary>
    /// Fireball ability
    /// </summary>
    /// <returns>True once fireball has been cast</returns>
    public bool Fireball() {
        //If the player is standing on a hex (not falling, jumping)
        if (levelController[q, r, h] != null) {
            //Get the area around the current hex
            uiController.ShowValidTopDownRadius(q, r, h, 5, true);
        }

        UICellObj hitObj = null;

        //Non VR Movement
        if (!vrActive) {
            //Get the cell the player is looking at
            Vector3 lineOfSight = playerCamera.transform.forward * 1000;
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, lineOfSight, out hit)) {
                //Get the UI hex cell the player is looking at
                hitObj = hit.transform.gameObject.GetComponent<UICellObj>() as UICellObj;
            }
            //VR Specific movement
        } else {
            hitObj = VRHitObj;
        }

        //if it isn't null
        if (hitObj != null) {
            //get the selected cell; if it's within 5 units of the starting cell show the ability AoE
            PathCell lookedCell = aiController[hitObj.q, hitObj.r, hitObj.h];
            PathCell startCell = aiController[q, r, h];

            //if this is in the right range to cast this, it's a valid move.
            if (aiController.DistBetween(lookedCell, startCell) <= 5) {
                uiController.ShowValidTopDownRadius(hitObj.q, hitObj.r, hitObj.h, 1, true, TargetingMaterial.TARGETED_ZONE);

                //If the player presses the mouse button
                if (Input.GetMouseButtonUp(0) || vrPressUp) {
                    PathCell targetedCell = aiController[hitObj.q, hitObj.r, hitObj.h];
                    //kill any monsters on the cells you target
                    foreach (Monster m in aiController.monsters) {
                        PathCell monsterLoc = aiController.pathGrid[m.CurrentCell[0], m.CurrentCell[1], m.CurrentCell[2]];
                        if (aiController.DistBetween(targetedCell, monsterLoc) <= 1) {
                            m.gameObject.GetComponent<MonsterStats>().Health -= 9001;
                        }
                    }

                    //now turn the remaining tiles extra crispy
                    PathCell[] surroundingCells = aiController.pathGrid.GetRadius(hitObj.q, hitObj.r, hitObj.h, 1, -1, true);
                    foreach (PathCell cell in surroundingCells) {
                        HexCellData cellData = levelController.levelGrid[cell.q, cell.r, cell.h];
                        cellData.hexCellObject.gameObject.GetComponent<Renderer>().material = this.burntTileMaterial;
                    }

                    actionPoints -= 1;
                    uiController.ClearCells();
                    uiController.setVisibility(false);
                    currentAction = AbilityEnum.NOT_USING_ABILITIES;
                    return true;
                }
            }
        }

        //Not finished casting
        return false;
    }

    //Other planned abilities:
    //Ice Wall--will likely involve multiple, sequential targeting inputs
    //Vine Surge--launches a line of vines that root enemies.
}