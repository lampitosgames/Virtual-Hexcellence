using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for objectives. (Superclass to Goal and SpecialItem.)
/// Other classes can inherit this to implement different objective functionality.
/// </summary>
public abstract class Objective : MonoBehaviour {

    //protected for the sake of SpecialItem.
    protected bool addedSelf = false;

    //Has the player reached this objective?
    public bool reached = false;
    //Coords
    public int q, r, h;
    
    protected LevelController levelController = null;

    /// <summary>
    /// Unity's start function
    /// </summary>
    public void Start()
    {
        //get the objective's model height so we can properly determine the cell it is on
        float modelHeight = gameObject.GetComponent<Renderer>().bounds.size.y;
        //Get the hex index for this hex cell.  Pass in the transform.
        int[] thisHexIndex = HexConst.CoordToHexIndex(transform.position + new Vector3(0, -0.5f * modelHeight, 0));
        q = thisHexIndex[0];
        r = thisHexIndex[1];
        h = thisHexIndex[2];
    }
}
