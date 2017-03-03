using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A hex-based grid
/// INCREDIBLY USEFUL ARTICLE: http://www.redblobgames.com/grids/hexagons
/// That article is basically a hexagon bible.  I took a ton of pseudocode from it
/// </summary>
/// <typeparam name="T">Data type held at each hex</typeparam>
public class HexGrid<T> : IEnumerable<T> {
    //The first dimensional dictionary accesses items by hashed coordinate values.  The second-dimensional dictionary hold cell heights
    private Dictionary<int, Dictionary<int, T>> hexGrid = new Dictionary<int, Dictionary<int, T>>();

    /// <summary>
    /// Allow get/set for grids using [q,r,h]
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    public T this[int q, int r, int h] {
        get { return this.GetHex(q, r, h); }
        set { this.SetHex(q, r, h, value); }
    }

    /// <summary>
    /// Adds a cell reference to the map at the index.
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="r">height</param>
    /// <param name="cell">cell item</param>
    public void SetHex(int q, int r, int h, T cell) {
        int coords = Hash(new int[] { q, r });
        //If there is not a height dictionary at these coordinates
        if (!hexGrid.ContainsKey(coords)) {
            hexGrid[coords] = new Dictionary<int, T>();
        }
        //Add the cell at this height
        hexGrid[coords][h] = cell;
    }

    /// <summary>
    /// Get a cell at the index
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    /// <returns>Cell item. If none is found, returns default value for the data type (usually null)</returns>
    public T GetHex(int q, int r, int h) {
        //Hash the hex coordinates
        int coords = Hash(new int[] { q, r });
        //If there are cells at these coordinates
        if (hexGrid.ContainsKey(coords)) {
            //Get the dictionary that holds cells at different heights
            Dictionary<int, T> atCoords = hexGrid[coords];
            //If there is a cell at this height, return it
            if (atCoords.ContainsKey(h)) {
                return atCoords[h];
            }
        }
        return default(T);
    }

    /// <summary>
    /// Get hex cells in a radius surrounding the origin
    /// Optional parameters allow for search height (+/- how much height will it search)
    /// Iteration over height is middle-out (starts at 0, then +1, then -1, then +2, etc.)
    /// If nearby cells have a lot of height variance, set searchHeight to -1.  The program will do a dictionary key lookup instead of looping.
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    /// <param name="radius">radius around cell (in steps)</param>
    /// <param name="searchHeight">Search Height.  Default == 1.  -1 just finds the nearest y-shifted neighbor</param>
    /// <param name="includeOrigin">Include the origin cell (provided coords) in the return value.  Default == false</param>
    /// <returns></returns>
    public T[] GetRadius(int q, int r, int h, int radius, int searchHeight = 1, bool includeOrigin = false) {
        //Get coordinates in a radius around the center
        List<int[]> aCoords = GetAxialCoordsInRadius(q, r, radius, includeOrigin);

        //A list of non-null cells to return
        List<T> returnList = new List<T>();

        //Loop through found coordinates
        for (int i = 0; i < aCoords.Count; i++) {
            int[] coords = aCoords[i];
            //Hash the coordinate value for dictionary lookup
            int hashed = Hash(aCoords[i]);
            //If a height dictionary exists at the hashed location (null lookup)
            if (hexGrid.ContainsKey(hashed)) {
                //Get the height dictionary at these coordinates
                Dictionary<int, T> heightDic = hexGrid[Hash(aCoords[i])];
                //If height is bounded
                if (searchHeight > 0) {
                    //Check coordinate for current height
                    if (heightDic.ContainsKey(h)) {
                        returnList.Add(heightDic[h]);
                    }
                    //Loop through the height radius (default 1)
                    for (int k = 1; k <= searchHeight; k++) {
                        //Check up first, then down
                        if (heightDic.ContainsKey(h+k)) {
                            returnList.Add(heightDic[h+k]);
                            break;
                        }
                        if (heightDic.ContainsKey(h-k)) {
                            returnList.Add(heightDic[h-k]);
                            break;
                        }
                    }

                //Height is unbounded, find nearest neighbor
                } else {
                    //Initialize a "closest" value
                    int closestIndex = int.MaxValue;
                    //Loop through all keys in the height dictionary
                    foreach (KeyValuePair<int, T> height in heightDic) {
                        //Get the delta height.  If its smaller, this cell is the new closest
                        closestIndex = (Math.Abs(h - height.Key) < closestIndex) ? height.Key : closestIndex;
                    }
                    //Add the closest cell to the return list
                    returnList.Add(heightDic[closestIndex]);
                }
            }
        }
        return returnList.ToArray();
    }
    
    /// <summary>
    /// Get hex cells in a radius surrounding the origin.  Gets only the highest cells (top-down)
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    /// <param name="radius">radius around cell (in steps)</param>
    /// <param name="includeOrigin">Include the origin cell (provided coords) in the return value.  Default == false</param>
    /// <returns></returns>
    public T[] TopDownRadius(int q, int r, int h, int radius, bool includeOrigin = false) {
        //Get coordinates in a radius around the center
        List<int[]> aCoords = GetAxialCoordsInRadius(q, r, radius, includeOrigin);

        //A list of non-null cells to return
        List<T> returnList = new List<T>();

        //Loop through found coordinates
        for (int i = 0; i < aCoords.Count; i++) {
            //Hash the coordinate value for dictionary lookup
            int hashed = Hash(aCoords[i]);
            //If a height dictionary exists at the hashed location (null lookup)
            if (hexGrid.ContainsKey(hashed)) {
                //Get the height dictionary at these coordinates
                Dictionary<int, T> heightDic = hexGrid[Hash(aCoords[i])];
                //Add the highest item to the return list
                returnList.Add(heightDic[heightDic.Keys.Max()]);
            }
        }
        return returnList.ToArray();
    }

    /// <summary>
    /// Get coordinates in a radius around center.  This is a private split out function for radial selections
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="radius">radius</param>
    /// <param name="includeOrigin">include the origin coordinates?</param>
    /// <returns></returns>
    private List<int[]> GetAxialCoordsInRadius(int q, int r, int radius, bool includeOrigin) {
        //Convert the given axial coordinate to cube coordinates
        int[] cCoords = HexConst.AxialToCube(q, r, 0);
        //Store found axial coordinates in a list
        List<int[]> aCoords = new List<int[]>();
        //Loop from -radius to +radius on the x-axis
        for (int dx = -1 * radius; dx <= radius; dx++) {
            //Loop from -radius (constrained by the x radius) to +radius (also constrained by the x radius) on the y axis
            for (int dy = Math.Max(-radius, -dx - radius); dy <= Math.Min(radius, -dx + radius); dy++) {
                //Grab the final z coordinate
                int dz = -dx - dy;
                //include the origin cell in the return?
                if (includeOrigin == false && dx == 0 && dy == 0 && dz == 0) {
                    continue;
                }
                //Convert the location back to axial coordinates and add it to the list
                aCoords.Add(new int[] { cCoords[0] + dx, cCoords[2] + dz });
            }
        }

        return aCoords;
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
        foreach (KeyValuePair<int, Dictionary<int, T>> heightDictionary in hexGrid) {
            foreach (KeyValuePair<int, T> cell in heightDictionary.Value) {
                allCells.Add(cell.Value);
            }
        }
        return allCells.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
