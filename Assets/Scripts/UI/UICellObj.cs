using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICellObj : MonoBehaviour {
    public UICell parent;
    public int q, r, h;

	GameObject uiMinimapInteractableObject = null;
    List<GameObject> uiMinimapProps = new List<GameObject>(); //stores props--i.e. static stuff

	public void AddInteractable(GameObject prefab) {
		float scale = GameObject.Find ("UIController").GetComponent<UIController> ().uiScale;
		uiMinimapInteractableObject = Instantiate (prefab, 
								 //The position of the hex cell, shifted up by half the monster's height and the cell's height
								 gameObject.transform.position + new Vector3(0, scale*(HexConst.height + prefab.GetComponent<MeshRenderer>().bounds.size.y/2), 0),
								 gameObject.transform.rotation,
								 gameObject.transform);
		uiMinimapInteractableObject.transform.localScale /= scale;
	}
    public void AddProp(GameObject prefab)
    {
        float scale = GameObject.Find("UIController").GetComponent<UIController>().uiScale;
        GameObject temp = Instantiate(prefab,
                                 //The position of the hex cell, shifted up by half the monster's height and the cell's height
                                 gameObject.transform.position + new Vector3(0, scale * (HexConst.height + prefab.GetComponent<MeshRenderer>().bounds.size.y / 2), 0),
                                 gameObject.transform.rotation,
                                 gameObject.transform);
        temp.transform.localScale /= scale;

        uiMinimapProps.Add(temp);
    }

    public void RemoveMinimapObject () {
		if (uiMinimapInteractableObject != null) {
			uiMinimapInteractableObject.SetActive(false);
		}
	}

    public void HideMinimapProps()
    {
        if (uiMinimapProps != null)
        {
            foreach (GameObject obj in uiMinimapProps)
            {
                obj.SetActive(false);
            }
        }
    }
}
