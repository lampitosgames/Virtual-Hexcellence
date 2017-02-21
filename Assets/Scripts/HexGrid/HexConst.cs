using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexConst {
    public const float radius = 2.31f;

    //Convert hex coordinates to world coordinates
    public static Vector3 HexToWorldCoord(int q, int r) {
        float x = HexConst.radius * 1.5f * r;
        float y = 0;
        float z = HexConst.radius * Mathf.Sqrt(3) * (q + (r / 2f));
        return new Vector3(x, y, z);
    }

    //Get world coordinates and convert to hex
    public static int[] CoordToHexIndex(Vector3 pos) {
        float q = (pos.z * Mathf.Sqrt(3f) / 3f - pos.x / 3f) / HexConst.radius;
        float r = (pos.x * (2f / 3f)) / HexConst.radius;
        return hexRound(q, r);
    }

    //Rounds axial hex coordinates to make sure they satisfy the x + y + z = 0 rule of cube coordinates
    private static int[] hexRound(float q, float r) {
        //Convert axial to cube coordinates for the algorithm
        float x = q;
        float z = r;
        float y = -x - z;

        float rx = Mathf.Round(x);
        float ry = Mathf.Round(y);
        float rz = Mathf.Round(z);

        float x_diff = Mathf.Abs(rx - x);
        float y_diff = Mathf.Abs(ry - y);
        float z_diff = Mathf.Abs(rz - z);

        if (x_diff > y_diff && x_diff > z_diff) {
            rx = -ry - rz;
        } else if (y_diff > z_diff) {
            ry = -rx - rz;
        } else {
            rz = -rx - ry;
        }

        //Convert cube coords to axial
        int[] returnArray = { Mathf.RoundToInt(rx), Mathf.RoundToInt(rz) };
        return returnArray;

    }
}
