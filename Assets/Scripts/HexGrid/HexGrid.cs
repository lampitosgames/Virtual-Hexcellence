using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour {
    private Dictionary<int, GameObject> hexGrid = new Dictionary<int, GameObject>();

    //Adds a hex cell reference to the map at the index.
    //q=column, r=row
    public void SetHexLocation(int q, int r, GameObject cell) {
        int index = Hash(new int[] { q, r});
        if (hexGrid.ContainsKey(index)) {
            hexGrid[index] = cell;
        } else {
            hexGrid.Add(index, cell);
        }
    }
    //Get a hex cell at an index
    //q=column, r=row
    public GameObject GetHex(int q, int r) {
        int index = Hash(new int[] { q, r });
        if (hexGrid.ContainsKey(index)) {
            return hexGrid[index];
        } else {
            return null;
        }
    }

    public GameObject[] GetNeighbors(int q, int r) {
        List<GameObject> neighbors = new List<GameObject>();
        neighbors.Add(GetHex(q + 1, r));
        neighbors.Add(GetHex(q, r + 1));
        neighbors.Add(GetHex(q - 1, r + 1));
        neighbors.Add(GetHex(q - 1, r));
        neighbors.Add(GetHex(q, r - 1));
        neighbors.Add(GetHex(q + 1, r - 1));
        neighbors.RemoveAll(Cell => Cell == null);
        return neighbors.ToArray();
    }

    //Give an int array a hashed value
    private int Hash(int[] index) {
        int result = 17;
        for (int i = 0; i < index.Length; i++) {
            unchecked {
                result = result * 23 + index[i];
            }
        }
        return result;
    }
}
