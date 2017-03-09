using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The AICells hold state for individual cells in the AIController's pathGrid
/// </summary>
public class AICell : HexCell {
    //Pathing algorithm fields
    public AICell parent = null;
    public int g = int.MaxValue;
    //Is there an enemy standing on this cell?
    public bool hasEnemy = false;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    public AICell(int q, int r, int h) : base(q, r, h) {}

    /// <summary>
    /// Comparitor
    /// </summary>
    /// <param name="o">other AICell</param>
    /// <returns></returns>
    public bool Equals(AICell o) {
        return (q == o.q && r == o.r && h == o.h);
    }
}
