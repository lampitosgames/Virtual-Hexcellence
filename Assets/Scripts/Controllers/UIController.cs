using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the palyer's UI
/// </summary>
public class UIController : MonoBehaviour {
	//Assign the prefab in editor to spawn as minimap hex object
	public GameObject uiGridPrefab;

    //uiGrid holds all UICells which relate to each hex on the map
    public HexGrid<UICell> uiGrid = new HexGrid<UICell>();


    /// <summary>
    /// Allow getting/setting for the UI grid using [q,r,h]
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    public UICell this[int q, int r, int h] {
        get { return this.uiGrid[q, r, h]; }
        set { this.uiGrid[q, r, h] = value; }
    }

    /// <summary>
    /// Called from LevelController by each hex, instantiates an object at corresponding position
    /// </summary>
    /// <param name="cell"></param>
    public void addCellToUIMap(UICell cell){
        //Create a new GameObject to represent a location in the world
        GameObject newHologramCell = (GameObject)Instantiate(uiGridPrefab, cell.centerPos, transform.rotation);
        cell.setGameObject(newHologramCell);
        //Put the cell into the UIGrid
		uiGrid[cell.q, cell.r, cell.h] = cell;
        //Set the game object's parent transform for scaling/rotation purposes.
		newHologramCell.transform.parent = this.gameObject.transform;
	}

    /// <summary>
    /// This is tomporary to test different scales and positions of the minimap
    /// </summary>
    void Update(){
		if(Input.GetKeyDown("up")){
			transform.localScale = transform.localScale*0.5f;
			transform.position = new Vector3 (0,1,0);
		}
	}
		

}
