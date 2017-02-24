using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A hex-based grid
/// INCREDIBLY USEFUL ARTICLE: http://www.redblobgames.com/grids/hexagons
/// That article is basically a hexagon bible.  I took a ton of pseudocode from it
/// </summary>
/// <typeparam name="T">Data type held at each hex</typeparam>
public class HexGrid<T> : IEnumerable<T> {
    private Dictionary<int, T> hexGrid = new Dictionary<int, T>();

    /// <summary>
    /// Adds a cell reference to the map at the index.
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="r">height</param>
    /// <param name="cell">cell item</param>
    public void SetHex(int q, int r, int h, T cell) {
        int index = Hash(new int[] { q, r, h });
        if (hexGrid.ContainsKey(index)) {
            hexGrid[index] = cell;
        } else {
            hexGrid.Add(index, cell);
        }
    }

    /// <summary>
    /// Get a cell at the index
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    /// <returns>Cell item. If none is found, returns default value for the data type (usually null)</returns>
    public T GetHex(int q, int r, int h) {
        int index = Hash(new int[] { q, r, h });
        if (hexGrid.ContainsKey(index)) {
            return hexGrid[index];
        } else {
            return default(T);
        }
    }

    /// <summary>
    /// Get the hex cells surrounding the coordinates.  Won't return empty locations.
    /// Only gets cells with height +/- 2 from provided height
    /// TODO: Make this obsolete with a more generalized and efficient radial-based function
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    /// <returns>array of neighboring cell items</returns>
    public T[] GetNeighbors(int q, int r, int h) {
        List<T> neighbors = new List<T>();
        //Height lower
        neighbors.Add(GetHex(q + 1, r, h-1));
        neighbors.Add(GetHex(q, r + 1, h-1));
        neighbors.Add(GetHex(q - 1, r + 1, h-1));
        neighbors.Add(GetHex(q - 1, r, h-1));
        neighbors.Add(GetHex(q, r - 1, h-1));
        neighbors.Add(GetHex(q + 1, r - 1, h-1));
        //Same Height
        neighbors.Add(GetHex(q + 1, r, h));
        neighbors.Add(GetHex(q, r + 1, h));
        neighbors.Add(GetHex(q - 1, r + 1, h));
        neighbors.Add(GetHex(q - 1, r, h));
        neighbors.Add(GetHex(q, r - 1, h));
        neighbors.Add(GetHex(q + 1, r - 1, h));
        //Height higher
        neighbors.Add(GetHex(q + 1, r, h+1));
        neighbors.Add(GetHex(q, r + 1, h+1));
        neighbors.Add(GetHex(q - 1, r + 1, h+1));
        neighbors.Add(GetHex(q - 1, r, h+1));
        neighbors.Add(GetHex(q, r - 1, h+1));
        neighbors.Add(GetHex(q + 1, r - 1, h+1));
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

    /// <summary>
    /// Get an iterable list of cells for use in a foreach loop
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator<T> GetEnumerator() {
        List<T> allCells = new List<T>();
        foreach (KeyValuePair<int, T> cell in hexGrid) {
            allCells.Add(cell.Value);
        }
        return allCells.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
