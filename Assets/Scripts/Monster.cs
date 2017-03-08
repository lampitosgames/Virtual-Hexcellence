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
    
    /// <summary>
    /// Unity update method
    /// </summary>
	void Start () {
        //Grab references to the player and AIController
		player = GameObject.FindGameObjectWithTag("Player");
        aiController = GameObject.Find("AIController").GetComponent<AIController>() as AIController;
	}

    /// <summary>
    /// Unity update method
    /// </summary>
    void Update() {
        //Get the position of the player's feet
        Vector3 playerPos = player.GetComponent<Transform>().position - new Vector3(0, 0.8f, 0);
        //Get the cell the player is standing on
        playerCell = HexConst.CoordToHexIndex(playerPos);
        //Path from the start cell to the current cell
        pathToPlayer = aiController.PathBetween(playerCell, curCell);

        AICell cCell = aiController[curCell[0], curCell[1], curCell[2]];
    }

    /// <summary>
    /// Unity draw gizmos function
    /// </summary>
    void OnDrawGizmos() {
        //If the path exists
        if (path != null) {
            Gizmos.color = Color.cyan;
            //Loop through the path and draw a line between all cells to form a path.
            for (int i = 1; i < path.Count; i++) {
                Vector3 curCoords = HexConst.HexToWorldCoord(path[i][0], path[i][1], path[i][2]) + new Vector3(0, 1, 0);
                Vector3 prevCoords = HexConst.HexToWorldCoord(path[i - 1][0], path[i - 1][1], path[i - 1][2]) + new Vector3(0, 1, 0);
                Gizmos.DrawLine(curCoords, prevCoords);
            }
        }
    }
}
