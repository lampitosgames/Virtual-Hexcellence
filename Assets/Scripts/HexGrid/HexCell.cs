using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {
    HexGrid grid;
    int q, r;
    private void Awake() {
        //Save the grid game object
        grid = GameObject.Find("HexGridObj").GetComponent("HexGrid") as HexGrid;

        //Get the hex index for this hex cell.  Pass in the transform.
        int[] thisHexIndex = HexConst.CoordToHexIndex(transform.position);
        q = thisHexIndex[0];
        r = thisHexIndex[1];

        //Pass a reference to this hex cell to the hex grid
        grid.SetHex(q, r, gameObject);
    }
}
