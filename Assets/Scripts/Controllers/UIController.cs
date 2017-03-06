using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	//Assign the prefab in editor to spawn as minimap hex object
	public GameObject uiGridPrefab;

	//uiGrid holds all UICells which relate to each hex on the map
	public List<GameObject> uiGrid = new List<GameObject>();

	//Called from LevelController by each hex, instantiates an object at corresponding position
	public void addCellToUIMap(UICell cell){
		GameObject newHologramCell = (GameObject)Instantiate(uiGridPrefab,cell.centerPos,transform.rotation);
		uiGrid.Add (newHologramCell);
		newHologramCell.transform.parent = this.gameObject.transform;
	}

	//This is tomporary to test different scales and positions of the minimap
	void Update(){
		if(Input.GetKeyDown("up")){
			transform.localScale = transform.localScale*0.5f;
			transform.position = new Vector3 (0,1,0);
		}
	}
		

}
