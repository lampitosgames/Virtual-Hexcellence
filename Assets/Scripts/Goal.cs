using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for objectives.
/// Other classes can inherit this to implement different objective functionality
/// </summary>
public class Goal : MonoBehaviour {
    private bool addedSelf = false;

    //Has the player reached this goal?
    public bool reached = false;
    //Coords
    public int q, r, h;

    /// <summary>
    /// Unity's start function
    /// </summary>
    void Start() {
        //get the goal's model height so we can properly determine the cell it is on
        float modelHeight = gameObject.GetComponent<Renderer>().bounds.size.y;
        //Get the hex index for this hex cell.  Pass in the transform.
        int[] thisHexIndex = HexConst.CoordToHexIndex(transform.position + new Vector3(0, -0.5f * modelHeight, 0));
        q = thisHexIndex[0];
        r = thisHexIndex[1];
        h = thisHexIndex[2];
    }

    /// <summary>
    /// Unity's update function
    /// </summary>
    void Update() {
        //Can't add self in the start() function because it is creation order dependant on hex cells
        if (!addedSelf) {
            //Save the grid game object
            LevelController levelController = GameObject.Find("LevelController").GetComponent("LevelController") as LevelController;
            //Tell the level controller to initialize this hex cell
            levelController.AddGoal(q, r, h, gameObject);
            addedSelf = true;
        }
    }
}
