using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour {

    public int width = 6;
    public int height = 6;

    private Dictionary<int[], HexCell> hexGrid = new Dictionary<int[], HexCell>();

    //Adds a hex cell reference to the map at the index.
    //q=column, r=row
    public void SetHexLocation(int q, int r, HexCell cell) {
        int[] index = { q, r};
        hexGrid.Add(index, cell);
    }
    //Get a hex cell at an index
    //q=column, r=row
    public HexCell GetHex(int q, int r) {
        int[] index = { q, r };
        return hexGrid[index];
    }

    public Vector3 HexToCenterCoord(int q, int r) {

        float x = (2 * q + r) * HexConst.innerRadius;;
        float y = 0;
        float z = 1.5f * r * HexConst.outerRadius;

        return new Vector3(x, y, z);
    }

    public int[] CoordToHexIndex(Vector3 pos) {
        return null;
    }
}
