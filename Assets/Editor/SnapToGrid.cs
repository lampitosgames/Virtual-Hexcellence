using UnityEngine;
using UnityEditor;
using System.Collections;
namespace Assets.Editor.Menu
{
	public class SnapToGrid {

		[MenuItem("Snapping/Snap to Grid %g")]
		private static void MenuSnapToGrid() {
			foreach (Transform transform in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable)) {
				if(transform.name.Contains("HexCell")) {
                    transform.position = new Vector3 (
						Mathf.Round(transform.position.x / HexConst.innerRadius) * HexConst.innerRadius,
						0,
						Mathf.Round(transform.position.z / HexConst.outerRadius) * HexConst.outerRadius
					);
				}
			}
		}

		[MenuItem("Snapping/Snap to Y %H")]
		private static void MenuSnapToY() {
			foreach (Transform transform in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable)) {
				if (transform.name.Contains ("HexCell")) {
					Vector3 size = colSize (transform.gameObject);
					float ySnap = size.y;
                    transform.position = new Vector3 (
                        transform.position.x,
						Mathf.Round (transform.position.y / ySnap) * ySnap,
                        transform.position.z
					);
				}
			}
		}

		public static Vector3 colSize(GameObject gameObj) {
			GameObject obj = gameObj as GameObject;
			Renderer objRenderer = obj.GetComponent(typeof(Renderer)) as Renderer;
			return objRenderer.bounds.size;
		}
	}
}