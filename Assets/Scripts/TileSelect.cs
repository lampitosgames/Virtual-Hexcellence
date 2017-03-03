using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelect : MonoBehaviour {

	private 
	// Use this for initialization
	void Start () {
		
	}

	public int[] selectCell(GameObject gameCamera)
	{
		Vector3 lineOfSight = gameCamera.transform.forward * 100;
		RaycastHit hit;
		Debug.DrawRay (gameCamera.transform.position, lineOfSight, Color.green);
		if (Physics.Raycast (gameCamera.transform.position, lineOfSight, out hit)) {
			GameObject hitObj = hit.transform.gameObject;
			return HexConst.CoordToHexIndex(new Vector3(hitObj.transform.position.x, hitObj.transform.position.y, hitObj.transform.position.z));

		}
		return null;
	}
}
