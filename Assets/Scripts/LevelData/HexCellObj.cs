using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The actual mesh representing the cell in the game world
/// </summary>
// TODO: Abstract HexGrid cells.  There is a lot of repeated functionality.
public class HexCellObj : MonoBehaviour {
    //Model height
    public float modelHeight = HexConst.modelHeight;

    //Coordinates
    int q, r, h;

    /// <summary>
    /// Unity start method
    /// </summary>
    void Start() {
        modelHeight = gameObject.GetComponent<Renderer>().bounds.size.y;
        //modelHeight *= transform.localScale.y;
        //Save the grid game object
        LevelController levelController = GameObject.Find("LevelController").GetComponent("LevelController") as LevelController;

        //Get the hex index for this hex cell.  Pass in the transform.
        int[] thisHexIndex = HexConst.CoordToHexIndex(transform.position + new Vector3(0, 0.5f*modelHeight, 0));
        q = thisHexIndex[0];
        r = thisHexIndex[1];
        h = thisHexIndex[2];

        //Tell the level controller to initialize this hex cell
        levelController.AddCell(q, r, h, gameObject);
    }
}
