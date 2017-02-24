using UnityEngine;
using UnityEditor;
using System.Collections;
namespace Assets.Editor.Menu
{
	public class SnapToGrid {
        //Snap to grid hotkey
		[MenuItem("Snapping/Snap to Grid %g")]
		private static void MenuSnapToGrid() {
            //Loop through all gameobjects (this could be really clunky)
            //TODO: Find a better way of getting hex cells
			foreach (Transform transform in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable)) {
                //If the object's transform is named "HexCell", it is a snap-able hex cell
				if(transform.name.Contains("HexCell")) {
                    //Get the hex coordinates the object is overlapping with
                    int[] hexCoords = HexConst.CoordToHexIndex(transform.position);
                    //Move this object to the specific grid-aligned coordinates.
					transform.position = HexConst.HexToWorldCoord(hexCoords[0], hexCoords[1], hexCoords[2]);
				}
			}
		}
	}
}