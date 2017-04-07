using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base monster class.  Monsters work with the AIController to attack the player
/// </summary>
public class Monster : MonoBehaviour {
    //Player object reference
    GameObject player;
    //AI Controller reference
    AIController aiController;
    //Level Controller reference
    LevelController levelController;
    //Current cell of the player & self
    int[] curCell = null, playerCell = new int[] { 0, 0, 0 };

    //Attack material
    public Material attackMaterial;

    //Store path from start to player location
    List<int[]> pathToPlayer = null;

    //State variables
    public int aggroRadius = 10;
    public int movementSpeed = 2;
    public float realtimeSpeed = 4f;
    int actionPoints = 2;

    //current path index
    int curPInd;

    /// <summary>
    /// Unity update method
    /// </summary>
    void Start() {
        //Grab references to other game objects
        player = GameObject.FindGameObjectWithTag("Player");
        aiController = GameObject.Find("AIController").GetComponent<AIController>() as AIController;
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>() as LevelController;
        //Add self to the list of monsters
        aiController.monsters.Add(this);
    }

    /// <summary>
    /// Call this every step for the monster to take it's turn.
    /// Returns true when it's turn is over
    /// </summary>
    /// <returns>turn is over?</returns>
    public bool TakeTurn() {
        if (this.actionPoints > 0) {
            //If there is not a current path to the player
            if (pathToPlayer == null) {
                //Get the position of the player's feet
                Vector3 playerPos = player.GetComponent<Transform>().position;
                //Get the cell the player is standing on
                playerCell = HexConst.CoordToHexIndex(playerPos);
                //Get the monster's current cell
                curCell = HexConst.CoordToHexIndex(transform.position - new Vector3(0, 0.8f, 0));
                //Update the AI controller.  Enemy is moving off of this cell
                aiController[curCell[0], curCell[1], curCell[2]].hasEnemy = false;
                //Try to path from this cell to the player's cell
                pathToPlayer = aiController.PathBetween(playerCell, curCell);
                //If there is no valid path, end turn
                if (pathToPlayer == null) {
                    return EndTurn();
                }
                //current index in the path is zero (initialize)
                curPInd = 0;

                //There is a path
                //If the player is within <aggroRadius> hexes
            } else if (pathToPlayer.Count - 1 <= aggroRadius && pathToPlayer.Count > 2) {
                //Call the move to player functuion until movement phase ends
                if (this.MoveToPlayer()) {
                    //Decriment action points and reset the path to the player (in case its updated)
                    pathToPlayer = null;
                    actionPoints -= 1;
                }
                //Player is within Melee range
            } else if (pathToPlayer.Count == 2) {
                //Attack the player
                //End the game with the player losing (passing in false.  Player win state is triggered by passing in true)
                levelController.EndGame(false);
                //Change self material to the attacking material
                //TODO: Play animation
                gameObject.GetComponent<Renderer>().material = attackMaterial;
                //Reset variables
                pathToPlayer = null;
                actionPoints -= 1;
            } else {
                //Player not close enough to do anything
                return EndTurn();
            }
        } else {
            //Turn over.  End the turn
            return EndTurn();
        }
        return false;
    }

    /// <summary>
    /// End of turn code for housekeeping
    /// </summary>
    /// <returns>Always returns true.  Allows you to replace "return true" with "return EndTurn()"</returns>
    public bool EndTurn() {
        pathToPlayer = null;
        //Set the current cell (the one the monster ended its turn on)
        curCell = HexConst.CoordToHexIndex(transform.position - new Vector3(0, 0.8f, 0));
        //Update the AI controller to show an enemy is standing on the tile
        aiController[curCell[0], curCell[1], curCell[2]].hasEnemy = true;
        return true;
    }

    /// <summary>
    /// Step towards the player along a path
    /// Assumes pathToPlayer is not null
    /// </summary>
    /// <returns>Returns true when move action has finished</returns>
    public bool MoveToPlayer() {
        //If the monster is already adjacent to the player
        if (curPInd == pathToPlayer.Count - 2) {
            //don't move again
            return true;
        }
        //Get the target cell's coordinates in 3D
        Vector3 movementDest = HexConst.HexToWorldCoord(pathToPlayer[movementSpeed][0], pathToPlayer[movementSpeed][1], pathToPlayer[movementSpeed][2]) + new Vector3(0, 0.8f, 0);
        //If the enemy has reached the target destination
        if ((transform.position - movementDest).magnitude < 0.2f) {
            //Set the transform position to equal the destination
            transform.position = movementDest;
            //Return true since movement has ended
            return true;
        }
        //Get the next 'waypoint.'  The monster moves towards this coordinate over time
        Vector3 nextWaypoint = HexConst.HexToWorldCoord(pathToPlayer[curPInd + 1][0], pathToPlayer[curPInd + 1][1], pathToPlayer[curPInd + 1][2]) + new Vector3(0, 0.8f, 0);
        //Step towards the waypoint
        transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, realtimeSpeed * Time.deltaTime);
        //If next waypoint has been reached, move on to the next one
        if ((transform.position - nextWaypoint).magnitude < 0.2f) {
            curPInd++;
        }
        //If the script made it this far, movement is still happening
        return false;
    }

    /// <summary>
    /// Called at the end of monster's turns.  Resets all state data so the monster can take another turn.
    /// </summary>
    public void ResetTurn() {
        pathToPlayer = null;
        actionPoints = 2;
    }

    /// <summary>
    /// Unity draw gizmos function.
    /// Right now this is debug code to show the monster's path to the player
    /// </summary>
    void OnDrawGizmos() {
        //If the path exists
        if (pathToPlayer != null) {
            Gizmos.color = Color.cyan;
            //Loop through the path and draw a line between all cells to form a path.
            for (int i = 1; i < pathToPlayer.Count; i++) {
                Vector3 curCoords = HexConst.HexToWorldCoord(pathToPlayer[i][0], pathToPlayer[i][1], pathToPlayer[i][2]) + new Vector3(0, 1, 0);
                Vector3 prevCoords = HexConst.HexToWorldCoord(pathToPlayer[i - 1][0], pathToPlayer[i - 1][1], pathToPlayer[i - 1][2]) + new Vector3(0, 1, 0);
                Gizmos.DrawLine(curCoords, prevCoords);
            }
        }
    }
}
