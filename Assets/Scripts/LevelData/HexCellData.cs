using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A data object that stores information about a hex cell.
/// Things like: location, real-world coordinates, what characters occupy the cell, the cell's gameobject, items on top of the cell, etc.
/// This object type will expand over time.
/// All HexCellData objects are built and populated at runtime.
/// </summary>
// TODO: Abstract HexGrid cells.  There is a lot of repeated functionality.
public class HexCellData : HexCell {
    //Reference to the cell's game object
    public GameObject hexCellObject;
    public GameObject goal;
    public bool hasGoal;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    /// <param name="hexCellObject">Hex GameObject</param>
	public HexCellData(int q, int r, int h, GameObject hexCellObject) : base(q, r, h) {
        this.hexCellObject = hexCellObject;
    }
}
