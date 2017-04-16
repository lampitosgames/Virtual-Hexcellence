using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The object representing the cell in the game world
/// </summary>
public class HexCellObj : MonoBehaviour {
    public enum tileType {
        GRASS,
        STONE,
        CAVE,
        WATER
    };

    //Model height/scale
    public float modelHeight, modelScale;

    //Coordinates
    public int q, r, h;

    //Tile type (assumed grass)
    public tileType type = tileType.GRASS;

    /// <summary>
    /// Unity start method
    /// </summary>
    void Start() {
        //Get the model height & scale from the renderer
        modelHeight = gameObject.GetComponent<Renderer>().bounds.size.y;
        modelScale = gameObject.transform.localScale.y;
        
        //Get a reference to the level controller
        LevelController levelController = GameObject.Find("LevelController").GetComponent("LevelController") as LevelController;

        //Get the hex index for this hex cell.  Pass in the transform.
        //Use the model height to allow for y-scaling.  This lets the engine handle hex cell models with variable heights
        int[] thisHexIndex = HexConst.CoordToHexIndex(transform.position + new Vector3(0, 0.5f * modelHeight, 0));
        q = thisHexIndex[0];
        r = thisHexIndex[1];
        h = thisHexIndex[2];

        //Tell the level controller to initialize this hex cell
        levelController.AddCell(q, r, h, gameObject);
    }
}
