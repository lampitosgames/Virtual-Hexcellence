using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A set of static functions used for hex grid calculations
/// </summary>
public static class HexConst {
    //Side length of a hex.  Also distance from the hex center to any of the hex corners
    public const float radius = 2.31f;

    /// <summary>
    /// Convert hex coordinates to world coordinates
    /// </summary>
    /// <param name="q">hex column</param>
    /// <param name="r">hex row</param>
    /// <returns>point in world</returns>
    public static Vector3 HexToWorldCoord(int q, int r) {
        float x = HexConst.radius * 1.5f * r;
        float y = 0;
        float z = HexConst.radius * Mathf.Sqrt(3) * (q + (r / 2f));
        return new Vector3(x, y, z);
    }
    
    /// <summary>
    /// Get world coordinates and convert them to hex coordinates
    /// </summary>
    /// <param name="pos">a point in the world</param>
    /// <returns>integer array of axial hex coordinates</returns>
    public static int[] CoordToHexIndex(Vector3 pos) {
        float q = (pos.z * Mathf.Sqrt(3f) / 3f - pos.x / 3f) / HexConst.radius;
        float r = (pos.x * (2f / 3f)) / HexConst.radius;
        return hexRound(q, r);
    }
    
    /// <summary>
    /// Convert from axial hex coordinates to cube hex coordinates
    /// </summary>
    /// <param name="axialCoords">Integer array of axial coordinates</param>
    /// <returns>Integer array of cube coordinates</returns>
    public static int[] AxialToCube(int[] axialCoords) {
        // x = q;
        // z = r;
        // y = -x - z;
        return new int[] { axialCoords[0], -axialCoords[0] - axialCoords[1], axialCoords[1] };
    }
    
    /// <summary>
    /// Convert from cube to axial hex coordinates.
    /// does so by simply dropping the 'y' axis of the cube coordinates
    /// </summary>
    /// <param name="cubeCoords">Integer array of cube coordinates</param>
    /// <returns>Integer array of axial coordinates</returns>
    public static int[] CubeToAxial(int[] cubeCoords) {
        return new int[] { cubeCoords[0], cubeCoords[2] };
    }

    /// <summary>
    /// Rounds axial hex coordinates to make sure they satisfy the x + y + z = 0 rule of cube coordinates
    /// </summary>
    /// <param name="q">partial axial column coordinate</param>
    /// <param name="r">partial axial row coordinate</param>
    /// <returns>verified axial coordinates</returns>
    private static int[] hexRound(float q, float r) {
        //Convert axial to cube coordinates for the algorithm
        //Not using the provided static methods because input values haven't been rounded to ints
        float x = q;
        float z = r;
        float y = -x - z;

        //Round the floats
        float rx = Mathf.Round(x);
        float ry = Mathf.Round(y);
        float rz = Mathf.Round(z);

        //Find how much each value had to be rounded by.  These will always be <=0.5
        float x_diff = Mathf.Abs(rx - x);
        float y_diff = Mathf.Abs(ry - y);
        float z_diff = Mathf.Abs(rz - z);

        //If the x difference is the largest when rounded, make it the changed parameter
        if (x_diff > y_diff && x_diff > z_diff) {
            rx = -ry - rz;
        //If y is largest, it is the changed parameter
        } else if (y_diff > z_diff) {
            ry = -rx - rz;
        //z is largest and it becomes the changed parameter
        } else {
            rz = -rx - ry;
        }

        //Convert cube coords to axial
        int[] returnArray = { Mathf.RoundToInt(rx), Mathf.RoundToInt(rz) };
        return returnArray;

    }
}
