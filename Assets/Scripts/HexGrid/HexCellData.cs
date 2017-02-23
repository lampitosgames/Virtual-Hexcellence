using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCellData {
    public int q, r, h;
    public GameObject hexCellObject;
    public Vector3 centerPos;

	public HexCellData(int q, int r, int h, GameObject hexCellObject) {
        this.q = q;
        this.r = r;
        this.h = h;
        this.hexCellObject = hexCellObject;
        this.centerPos = HexConst.HexToWorldCoord(q, r, h);
    }
}
