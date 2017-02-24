using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The AICells hold state for individual cells in the AIController's pathGrid
/// </summary>
// TODO: Abstract HexGrid cells.  There is a lot of repeated functionality.
public class AICell {
    //Coordinates
    public int q, r, h;

    //Pathing algorithm fields
    public AICell parent = null;
    public int g = int.MaxValue;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    public AICell(int q, int r, int h) {
        this.q = q;
        this.r = r;
        this.h = h;
    }

    /// <summary>
    /// Comparitor
    /// </summary>
    /// <param name="o">other AICell</param>
    /// <returns></returns>
    public bool Equals(AICell o) {
        return (q == o.q && r == o.r && h == o.h);
    }
}
