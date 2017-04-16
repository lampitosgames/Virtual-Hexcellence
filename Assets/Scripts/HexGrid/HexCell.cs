using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all hex cell objects.
/// Hex Cell objects usually go into hex grids
/// </summary>
public abstract class HexCell {
    //Hex Cell coordinates
    public int q, r, h;
    //Center position of the cell
    public Vector3 centerPos;

    /// <summary>
    /// Getter
    /// Lets us quickly get q,r,h as an int[]
    /// </summary>
    public int[] CellCoords {
        get { int[] temp = { q, r, h }; return temp; }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    public HexCell(int q, int r, int h) {
        this.q = q;
        this.r = r;
        this.h = h;
        this.centerPos = HexConst.HexToWorldCoord(q, r, h);
    }

    /// <summary>
    /// Comparitor
    /// </summary>
    /// <param name="o">other AICell</param>
    /// <returns></returns>
    public bool Equals(HexCell o) {
        return (q == o.q && r == o.r && h == o.h);
    }
}
