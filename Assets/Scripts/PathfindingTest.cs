using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTest : MonoBehaviour {
    GameObject player;
    AIController aiController;
    int[] curCell = null, startCell = null;

    List<int[]> path = null;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("FPSController") as GameObject;
        aiController = GameObject.Find("AIController").GetComponent<AIController>() as AIController;
	}

    void Update() {
        Vector3 playerPos = player.GetComponent<Transform>().position - new Vector3(0, 0.8f, 0);
        if (startCell == null) { startCell = HexConst.CoordToHexIndex(playerPos); }
        curCell = HexConst.CoordToHexIndex(playerPos);
        path = aiController.PathBetween(startCell, curCell);
    }

    void OnDrawGizmos() {
        if (path != null) {
            Gizmos.color = Color.cyan;
            for (int i = 1; i < path.Count; i++) {
                Vector3 curCoords = HexConst.HexToWorldCoord(path[i][0], path[i][1], path[i][2]) + new Vector3(0, 1, 0);
                Vector3 prevCoords = HexConst.HexToWorldCoord(path[i - 1][0], path[i - 1][1], path[i - 1][2]) + new Vector3(0, 1, 0);
                Gizmos.DrawLine(curCoords, prevCoords);
            }
        }
    }
}
