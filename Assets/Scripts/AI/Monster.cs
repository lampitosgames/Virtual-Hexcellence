using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for monsters.  Provides basic pathfinding logic and allows them to take turns
/// </summary>
public class Monster : MonoBehaviour {
    //Player object reference
    GameObject player;
    //Controller references
    AIController aiController;
    LevelController levelController;

    //Store path from start to player location
    List<int[]> pathToPlayer = null;
    //current path index
    int curPInd;
    //Current cell of self and player 
    int[] curCell = null, playerCell = new int[] { 0, 0, 0 };

    //Getter for the current cell
    public int[] CurrentCell
    {
        get { return curCell; }
    }

    //TEMP: Attack material (when the monster attacks, change it to this material)
    public Material attackMaterial;

    //State variables
    public int aggroRadius = 10;
    public int movementSpeed = 2;
    public float realtimeSpeed = 4f;
    int actionPoints = 2;

    /// <summary>
    /// Unity start method
    /// </summary>
    void Start() {
        //Grab required references
        player = GameObject.FindGameObjectWithTag("Player");
        aiController = GameObject.Find("AIController").GetComponent<AIController>() as AIController;
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>() as LevelController;

        curCell = HexConst.CoordToHexIndex(transform.position - new Vector3(0, 0.8f, 0));
        //Add self to the list of monsters
        aiController.monsters.Add(this);
    }

    /// <summary>
    /// Called every step on this monsters turn
    /// Logically flows through different stages of the monster and updates it in the world, returning true once this monster's turn is over
    /// </summary>
    /// <returns>turn is over?</returns>
    public bool TakeTurn() {
        //Monster has action points left?
        if (this.actionPoints > 0) {
            //If there is not a current path to the player
            if (pathToPlayer == null) {
                //Get player positino
                Vector3 playerPos = player.GetComponent<Transform>().position;
                //Get the current cell and the player's cell
                curCell = HexConst.CoordToHexIndex(transform.position - new Vector3(0, 0.8f, 0));
                playerCell = HexConst.CoordToHexIndex(playerPos);

                //Update the AI controller.  This monster will be moving off it's current cell
                aiController[curCell[0], curCell[1], curCell[2]].hasEnemy = false;
                //Try to path from this cell to the player's cell
                pathToPlayer = aiController.PathBetween(playerCell, curCell);
                //If there is no valid path, end turn
                if (pathToPlayer == null) {
                    return EndTurn();
                }
                //Start the path index at 0
                curPInd = 0;

            //There is a path
            //If the player is within <aggroRadius> hexes
            } else if (pathToPlayer.Count - 1 <= aggroRadius && pathToPlayer.Count > 2) {
                //Stall the logic by repeatedly calling MoveToPlayer() until it returns true
                if (this.MoveToPlayer()) {
                    pathToPlayer = null;
                    actionPoints -= 1;
                }

             //Player is within Melee range
            } else if (pathToPlayer.Count == 2) {
                //TODO: Attack player.  For now, if a monster detects it can attack the player, the player loses
                //End the game in a failure state
                levelController.EndGame(false);
                //Turn a different color so the player knows what attacked them
                gameObject.GetComponent<Renderer>().material = attackMaterial;
                //Reset everything
                pathToPlayer = null;
                actionPoints -= 1;

            } else {
                //Player not close enough to do anything, end turn
                return EndTurn();
            }
        //Turn is over, end it
        } else {
            return EndTurn();
        }

        //If the logic made it this far, the turn is not over
        return false;
    }

    /// <summary>
    /// End of turn code for housekeeping
    /// </summary>
    /// <returns>Always returns true.  Allows you to replace "return true" with "return EndTurn()"</returns>
    public bool EndTurn() {
        //Reset path to player
        pathToPlayer = null;
        //Update the AI controller to show an enemy is standing on the tile
        curCell = HexConst.CoordToHexIndex(transform.position - new Vector3(0, 0.8f, 0));
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
            //don't move more
            return true;
        }
        //Get the move destination by converting it's hex coordinates to a world position
        Vector3 movementDest = HexConst.HexToWorldCoord(pathToPlayer[movementSpeed][0], pathToPlayer[movementSpeed][1], pathToPlayer[movementSpeed][2]) + new Vector3(0, 0.8f, 0);

        //If this monster has reached the target destination
        if ((transform.position - movementDest).magnitude < 0.2f) {
            //Set the transform position to equal the destination
            transform.position = movementDest;
            //Return true since movement has ended
            return true;
        }
        //Get the 'next waypoint' to move towards.  This is simply the next cell in the path's world coordinates
        Vector3 nextWaypoint = HexConst.HexToWorldCoord(pathToPlayer[curPInd + 1][0], pathToPlayer[curPInd + 1][1], pathToPlayer[curPInd + 1][2]) + new Vector3(0, 0.8f, 0);
        //Step towards the waypoint
        transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, realtimeSpeed * Time.deltaTime);
        //If next waypoint has been reached, move on to the next one
        if ((transform.position - nextWaypoint).magnitude < 0.2f) {
            curPInd++;
        }
        //If logic has made it this far, movement is not over.
        return false;
    }

    /// <summary>
    /// Reset the monster's state variables so it can take another turn
    /// </summary>
    public void ResetTurn() {
        pathToPlayer = null;
        actionPoints = 2;
    }

    /// <summary>
    /// Unity draw gizmos function.  This is just a debug to draw pathfinding data in the world
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
