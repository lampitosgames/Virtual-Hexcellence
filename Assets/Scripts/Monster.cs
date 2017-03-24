using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A temporary script to test and showcase the pathfinding logic
/// </summary>
public class Monster : MonoBehaviour {
    //Player object reference
    GameObject player;
    //AI Controller reference
    AIController aiController;
    //Current cell of the player, 
    int[] curCell = null, playerCell = new int[] { 0, 0, 0 };

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
	void Start () {
        //Grab references to the player and AIController
		player = GameObject.FindGameObjectWithTag("Player");
        aiController = GameObject.Find("AIController").GetComponent<AIController>() as AIController;
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
                curCell = HexConst.CoordToHexIndex(transform.position - new Vector3(0, 0.8f, 0));
                //Try to path from this cell to the player's cell
                pathToPlayer = aiController.PathBetween(playerCell, curCell);
                curPInd = 0;

                //There is a path
                //If the player is within <aggroRadius> hexes
            } else if (pathToPlayer.Count-1 <= aggroRadius && pathToPlayer.Count > 2)  {
                if (this.MoveToPlayer()) {
                    pathToPlayer = null;
                    actionPoints -= 1;
                }
            //Player is within Melee range
            } else if (pathToPlayer.Count == 2) {
                //TODO: Attack player
                Debug.Log("ATTACKING PLAYER");
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
        if (curPInd == pathToPlayer.Count-2) {
            //don't move more
            return true;
        }
        Vector3 movementDest = HexConst.HexToWorldCoord(pathToPlayer[movementSpeed][0], pathToPlayer[movementSpeed][1], pathToPlayer[movementSpeed][2]) + new Vector3(0, 0.8f, 0);
        //If the enemy has reached the target destination
        if ((transform.position - movementDest).magnitude < 0.2f) {
            //Set the transform position to equal the destination
            transform.position = movementDest;
            //Return true since movement has ended
            return true;
        }
        Vector3 nextWaypoint = HexConst.HexToWorldCoord(pathToPlayer[curPInd + 1][0], pathToPlayer[curPInd + 1][1], pathToPlayer[curPInd + 1][2]) + new Vector3(0, 0.8f, 0);
        //Step towards the waypoint
        transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, realtimeSpeed * Time.deltaTime);
        //If next waypoint has been reached, move on to the next one
        if ((transform.position - nextWaypoint).magnitude < 0.2f) {
            curPInd++;
        }
        return false;
    }

    public void ResetTurn() {
        pathToPlayer = null;
        actionPoints = 2;
    }

    /// <summary>
    /// Unity draw gizmos function
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
