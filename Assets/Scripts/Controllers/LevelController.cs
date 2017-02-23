using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
    public HexGrid<HexCellData> levelGrid = new HexGrid<HexCellData>();
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddCell(int q, int r, int h, GameObject cellObj) {
        HexCellData newCell = new HexCellData(q, r, h, cellObj);
        levelGrid.SetHex(q, r, h, newCell);
    }
}
