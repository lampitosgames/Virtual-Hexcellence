using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICellObj : MonoBehaviour {
    public UICell parent;
    public int q, r, h;

	GameObject uiMonster = null;

	public void AddMonster(GameObject prefab) {
		float scale = GameObject.Find ("UIController").GetComponent<UIController> ().uiScale;
		uiMonster = Instantiate (prefab, 
								 //The position of the hex cell, shifted up by half the monster's height and the cell's height
								 gameObject.transform.position + new Vector3(0, scale*(HexConst.height + prefab.GetComponent<MeshRenderer>().bounds.size.y/2), 0),
								 gameObject.transform.rotation,
								 gameObject.transform);
		uiMonster.transform.localScale /= scale;
	}
	public void RemoveMonster () {
		if (uiMonster != null) {
			Destroy (uiMonster);
		}
	}
}
