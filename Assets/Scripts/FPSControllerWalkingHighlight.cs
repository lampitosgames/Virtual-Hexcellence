using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//WARNING: HORRIBLE CODE AHEAD
public class FPSControllerWalkingHighlight : MonoBehaviour {
    public HexGrid grid;
    public GameObject currentHex;
    public GameObject prevHex;

    public Material currentHexMaterial;
    public Material normalHexMaterial;
    public Material neighborHexMaterial;

	// Use this for initialization
	void Start () {
        //Get the grid
        grid = GameObject.Find("HexGridObj").GetComponent("HexGrid") as HexGrid;
    }
	
	// Update is called once per frame
	void Update () {
        //Get the cell location of the player
        int[] cellIndex = HexConst.CoordToHexIndex(transform.position);
        //Get the cell the player is standing on
        currentHex = grid.GetHex(cellIndex[0], cellIndex[1]);
        //if (currentHex != prevHex && prevHex != null) {
        //    prevHex.GetComponent<Renderer>().material = normalHexMaterial;
        //}
        //prevHex = currentHex;
        if (currentHex != null) {
            currentHex.GetComponent<Renderer>().material = currentHexMaterial;
        }
        GameObject[] neighbors = grid.GetNeighbors(cellIndex[0], cellIndex[1]);
        for (int i=0; i<neighbors.Length; i++) {
            neighbors[i].GetComponent<Renderer>().material = neighborHexMaterial;
        }

    }
}
