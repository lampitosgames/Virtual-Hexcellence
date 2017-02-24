using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
    public HexGrid<HexCellData> levelGrid = new HexGrid<HexCellData>();
    public AIController aiController;
	// Use this for initialization
	void Awake() {
        aiController = GameObject.Find("AIController").GetComponent<AIController>() as AIController;
	}

    public void AddCell(int q, int r, int h, GameObject cellObj) {
        //Create a hex data object to go into the level grid
        HexCellData newCell = new HexCellData(q, r, h, cellObj);
        levelGrid.SetHex(q, r, h, newCell);

        //Create an ai hex object to go into the pathing grid
        AICell aiCell = new AICell(q, r, h);
        aiController.pathGrid.SetHex(q, r, h, aiCell);
    }
}
