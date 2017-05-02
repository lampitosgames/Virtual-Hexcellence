using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used for determining which type of minimap object to add.
public enum MinimapObjectType{
    ENEMY,GOAL,ITEM,PROP
}

public class UICellObj : MonoBehaviour {
    public UICell parent;
    public int q, r, h;

    GameObject uiMinimapEnemy = null;
    GameObject uiMinimapGoal = null;
    List<GameObject> uiMinimapItems = new List<GameObject>(); //stores items--i.e. interactables
    List<GameObject> uiMinimapProps = new List<GameObject>(); //stores props--i.e. static stuff

    //Adds an object to the minimap. Use this for enemies and goals.
    //The enum is to reduce repetitive code.
	public void AddMinimapObject(GameObject prefab, MinimapObjectType minitype = MinimapObjectType.ENEMY) {
        float scale = GameObject.Find("UIController").GetComponent<UIController>().uiScale;
        GameObject adjustedPrefab = Instantiate(prefab,
                                 //The position of the hex cell, shifted up by half the monster's height and the cell's height
                                 gameObject.transform.position + new Vector3(0, scale * (HexConst.height + prefab.GetComponent<MeshRenderer>().bounds.size.y / 2), 0),
                                 gameObject.transform.rotation,
                                 gameObject.transform);
        adjustedPrefab.transform.localScale /= scale;

        //do we have an enemy on this tile, an item, or a prop?
        switch (minitype)
        {
            case MinimapObjectType.ENEMY: //if we have an enemy on this tile, make the minimap enemy the enemy on this tile
                uiMinimapEnemy = adjustedPrefab; break;
            case MinimapObjectType.GOAL: //if we have a goal on this tile, make the minimap goal the goal on this tile.
                uiMinimapGoal = adjustedPrefab; break;
            default: break; //if we have anything else here, something borked up.
        }
    }

    //Adds multiple minimap objects. Use this for items and props.
    public void AddMinimapObjects(List<GameObject> prefabs, MinimapObjectType minitype = MinimapObjectType.PROP)
    {
        float scale = GameObject.Find("UIController").GetComponent<UIController>().uiScale;
        List<GameObject> adjustedPrefabs = new List<GameObject>();

        foreach (GameObject prefab in prefabs)
        {
            adjustedPrefabs.Add(scaledVersionOf(prefab, scale));
        }

        switch (minitype)
        {
            case MinimapObjectType.ITEM: //if we have items on this tile, add them to the list of items on this tile.
                uiMinimapItems = adjustedPrefabs; break;
            case MinimapObjectType.PROP: //if we have props on this tile, add it to the list of props on this tile.
                uiMinimapProps = adjustedPrefabs; break;
            default: break; //if we had anything else, something borked up
        }
    }

    //Helper method for the "add minimap object/s" methods.
    private GameObject scaledVersionOf(GameObject prefab, float scale)
    {
        GameObject adjustedPrefab = Instantiate(prefab,
                                 //The position of the hex cell, shifted up by half the monster's height and the cell's height
                                 gameObject.transform.position + new Vector3(0, scale * (HexConst.height + prefab.GetComponent<MeshRenderer>().bounds.size.y / 2), 0),
                                 gameObject.transform.rotation,
                                 gameObject.transform);
        adjustedPrefab.transform.localScale /= scale;
        return adjustedPrefab;
    }

    //Hides the contents of the cell.
    public void HideContents()
    {
        RemoveMinimapEnemy();
        RemoveMinimapGoal();
        HideMinimapItems();
        HideMinimapProps();
    }

    public void RemoveMinimapEnemy () {
		if (uiMinimapEnemy != null) {
			uiMinimapEnemy.SetActive(false);
		}
	}

    public void RemoveMinimapGoal()
    {
        if (uiMinimapGoal != null)
        {
            uiMinimapGoal.SetActive(false);
        }
    }

    //reset the minimap
    public void ResetMinimapItems () {
        uiMinimapItems = new List<GameObject>();
    }

    //reset the props
    public void ResetMinimapProps() {
        uiMinimapProps = new List<GameObject>();
    }

    public void HideMinimapItems()
    {
        if (uiMinimapItems != null)
        {
            foreach (GameObject obj in uiMinimapItems)
            {
                obj.SetActive(false);
            }
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
