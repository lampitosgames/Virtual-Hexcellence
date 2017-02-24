using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {
    public HexGrid<AICell> pathGrid = new HexGrid<AICell>();
    

    public List<int[]> PathBetween(int[] startCoords, int[] endCoords) {
        AICell cStart = pathGrid.GetHex(startCoords[0], startCoords[1], startCoords[2]);
        AICell cEnd = pathGrid.GetHex(endCoords[0], endCoords[1], endCoords[2]);
        if (cStart == null || cEnd == null) { return null; }

        HashSet<AICell> closed = new HashSet<AICell>();
        HashSet<AICell> open = new HashSet<AICell>();

        cStart.g = DistBetween(cStart, cEnd);
        open.Add(cStart);

        while (open.Count > 0) {
            //Get the lowest g-valued cell;
            AICell cCell = null;
            foreach (AICell cell in open) {
                if (cCell == null) { cCell = cell;  continue; }
                if (cCell.g > cell.g) {
                    cCell = cell;
                }
            }
            //If the current cell is the goal, return the found path
            if (cCell.Equals(cEnd)) {
                return ReconstructPath(cCell, cStart);
            }

            //Remove cCell from open and add it to closed
            open.Remove(cCell);
            closed.Add(cCell);
            //Loop through the neighbors of ccell
            foreach (AICell nCell in pathGrid.GetNeighbors(cCell.q, cCell.r, cCell.h)) {
                //If the neighbor node is already in the closed set, continue
                if (closed.Contains(nCell)) { continue; }
                //If the neighbor isn't in the open set, reset its values
                if (!open.Contains(nCell)) {
                    nCell.g = cCell.g + DistBetween(cCell, nCell);
                    nCell.parent = cCell;
                    open.Add(nCell);
                } else {
                    if (nCell.g > cCell.g + DistBetween(cCell, nCell)) {
                        nCell.g = cCell.g + DistBetween(cCell, nCell);
                        nCell.parent = cCell;
                    }
                }
            }
        }
        return null;
    }

    private int DistBetween(AICell point1, AICell point2) {
        int[] ac = HexConst.AxialToCube(new int[] { point1.q, point1.r, point1.h });
        int[] bc = HexConst.AxialToCube(new int[] { point2.q, point2.r, point2.h });
        return ((int)Mathf.Abs(ac[0] - bc[0]) + (int)Mathf.Abs(ac[1] - bc[1]) + (int)Mathf.Abs(ac[2] - bc[2])) / 2;
    }

    private List<int[]> ReconstructPath(AICell endCell, AICell startCell) {
        List<int[]> returnList = new List<int[]>();
        AICell temp = endCell;
        do {
            returnList.Add(new int[] { temp.q, temp.r, temp.h });
            temp = temp.parent;
        } while (!temp.Equals(startCell));
        returnList.Add(new int[] { startCell.q, startCell.r, startCell.h });
        return returnList;
    }
}
