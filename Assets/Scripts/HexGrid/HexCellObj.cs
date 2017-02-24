using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCellObj : MonoBehaviour {
    int q, r, h;
    void Start() {
        //Save the grid game object
        LevelController levelController = GameObject.Find("LevelController").GetComponent("LevelController") as LevelController;

        //Get the hex index for this hex cell.  Pass in the transform.
        int[] thisHexIndex = HexConst.CoordToHexIndex(transform.position);
        q = thisHexIndex[0];
        r = thisHexIndex[1];
        h = thisHexIndex[2];

        //Pass a reference to this hex cell to the hex grid
        levelController.AddCell(q, r, h, gameObject);
    }
}
