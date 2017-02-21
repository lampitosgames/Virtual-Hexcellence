using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {
    HexGrid grid;
    int q, r;
    private void Awake() {
        //Get the hex index for this hex cell.  Pass in the transform.
        int[] thisHexIndex = HexConst.CoordToHexIndex(transform.position);
        q = thisHexIndex[0];
        r = thisHexIndex[1];
        //Save the grid game object
        grid = GameObject.Find("HexGridObj").GetComponent("HexGrid") as HexGrid;
        //Pass a reference to this hex cell to the hex grid
        grid.SetHexLocation(q, r, gameObject);
    }
}
