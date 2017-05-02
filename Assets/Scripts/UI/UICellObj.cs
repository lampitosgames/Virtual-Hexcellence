using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICellObj : MonoBehaviour {
    public UICell parent;
    public int q, r, h;

    public List<GameObject> minimapObjects = new List<GameObject>();

    public void AddMinimapObjects(List<GameObject> prefabs) {
        //Get the UI scale
        float scale = GameObject.Find("UIController").GetComponent<UIController>().uiScale;

        //Add all prefabs to the minimap
        foreach (GameObject prefab in prefabs) {
            GameObject uiObject = Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);

            //Set full bounds of prefab
            foreach (Renderer renderer in uiObject.GetComponentsInChildren<Renderer>()) {
                if (renderer != uiObject.GetComponentInChildren<Renderer>()) {
                    uiObject.GetComponentInChildren<Renderer>().bounds.Encapsulate(renderer.bounds);
                }
            }

            //Move the prefab up so it doesn't intersect the game cell
            uiObject.transform.position += new Vector3(0, scale * (HexConst.height + prefab.GetComponentInChildren<Renderer>().bounds.size.y / 2), 0);
            uiObject.transform.localScale /= scale;
            //Add the new uiObject to the list of minimap objects
            minimapObjects.Add(uiObject);
        }
    }

    public void RemoveMinimapObjects() {
        //Destroy all gameobjects created by the list
        foreach (GameObject o in minimapObjects) {
            Destroy(o);
        }
        //Reset the list
        minimapObjects = new List<GameObject>();
    }
}
