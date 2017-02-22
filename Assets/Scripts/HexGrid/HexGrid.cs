using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour {
    private Dictionary<int, GameObject> hexGrid = new Dictionary<int, GameObject>();

    /// <summary>
    /// Adds a hex cell reference to the map at the index.
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="cell">hex gameobject</param>
    public void SetHex(int q, int r, GameObject cell) {
        int index = Hash(new int[] { q, r});
        if (hexGrid.ContainsKey(index)) {
            hexGrid[index] = cell;
        } else {
            hexGrid.Add(index, cell);
        }
    }

    /// <summary>
    /// Get a hex cell at the index
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <returns>hex gameobject.  If none is found, returns null</returns>
    public GameObject GetHex(int q, int r) {
        int index = Hash(new int[] { q, r });
        if (hexGrid.ContainsKey(index)) {
            return hexGrid[index];
        } else {
            return null;
        }
    }

    /// <summary>
    /// Get the hexes surrounding the coordinates.  Won't return empty locations
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <returns>array of neighboring game objects</returns>
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

    /// <summary>
    /// Hashes an integer array
    /// </summary>
    /// <param name="arr">Integer array to hash</param>
    /// <returns>hashed value</returns>
    private int Hash(int[] arr) {
        int result = 17;
        for (int i = 0; i < arr.Length; i++) {
            unchecked {
                result = result * 23 + arr[i];
            }
        }
        return result;
    }
}
