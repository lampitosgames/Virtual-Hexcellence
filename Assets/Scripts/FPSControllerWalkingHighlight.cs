using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An addon script to the FPS controller that highlights cells being walked over
/// </summary>
//WARNING: HORRIBLE CODE AHEAD
//TODO: Delete this monstrocity
public class FPSControllerWalkingHighlight : MonoBehaviour {
    //Reference to the level controller that provides access to the hex grid
    LevelController levelController;
	TileSelect tileSelection;

    //Materials for hexes being walked over
    public Material currenthexmaterial;
    public Material normalhexmaterial;
    public Material neighborhexmaterial;
	public Material highlightMaterial;

    //Store neighboring hexes
    public HexCellData[] neighbors = new HexCellData[0];

	private GameObject gameCamera;
	private HexCellData previousHex;


    /// <summary>
    /// Unity awake method
    /// </summary>
    void Awake() {
        //get the grid
        levelController = GameObject.Find("LevelController").GetComponent("LevelController") as LevelController;
		tileSelection = GameObject.Find("AIController").GetComponent("TileSelect") as TileSelect;
		gameCamera = this.transform.FindChild ("FirstPersonCharacter").gameObject as GameObject;
    }

    /// <summary>
    /// Unity update method
    /// </summary>
    void Update() {
        //Reset all neighbor cells
        foreach (HexCellData cell in neighbors) {
			if (!cell.Highlighted) {
				cell.hexCellObject.GetComponent<Renderer> ().material = normalhexmaterial;
			}
        }

        //get the cell location of the player
        int[] cellIndex = HexConst.CoordToHexIndex(new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z));
        //get the cell the player is standing on
        HexCellData currenthex = levelController.levelGrid.GetHex(cellIndex[0], cellIndex[1], cellIndex[2]);
        //If the player is standing on a hex (not falling, jumping)
        if (currenthex != null) {
            //Give the current hex the current hex material
            currenthex.hexCellObject.GetComponent<Renderer>().material = currenthexmaterial;
        }

        //Get the neighbors of the current hex
        neighbors = levelController.levelGrid.GetNeighbors(cellIndex[0], cellIndex[1], cellIndex[2]);
        //Give all valid neighbors the neighbor material
        foreach (HexCellData cell in neighbors) {
            cell.hexCellObject.GetComponent<Renderer>().material = neighborhexmaterial;
        }

		int [] selectedCellIndex = tileSelection.selectCell (gameCamera);
		if (selectedCellIndex != cellIndex && selectedCellIndex != null) {
			HexCellData hoveredHex = levelController.levelGrid.GetHex(selectedCellIndex[0], selectedCellIndex[1], selectedCellIndex[2]);
			if (hoveredHex != null) {
				foreach (HexCellData validMove in neighbors) {
					if (validMove == hoveredHex) {
						hoveredHex.hexCellObject.GetComponent<Renderer> ().material = highlightMaterial;
						if (previousHex != hoveredHex && previousHex != null) {
							previousHex.hexCellObject.GetComponent<Renderer> ().material = normalhexmaterial;
						}
						if (Input.GetMouseButtonDown (0)) {
							transform.position = new Vector3 (hoveredHex.centerPos.x, hoveredHex.centerPos.y + 0.8f, hoveredHex.centerPos.z);
						}
						previousHex = hoveredHex;
					}
				}
			}
		}

    }

}
