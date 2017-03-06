using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The LevelController is the stateful holder of level data.
/// It will accept info from lower objects and change the game state accordingly.
/// </summary>
public class LevelController : MonoBehaviour {
    //The level grid holds HexCellData objects, the meat of cell states
    public HexGrid<HexCellData> levelGrid = new HexGrid<HexCellData>();
    //A reference to the AIController
    public AIController aiController;
	public UIController uiController;

    /// <summary>
    /// Allow getting/setting for the level grid using [q,r,h]
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    public HexCellData this[int q, int r, int h] {
        get { return this.levelGrid[q, r, h]; }
        set { this.levelGrid[q, r, h] = value; }
    }

    /// <summary>
    /// Unity's awake method
    /// it is called before start()
    /// </summary>
	void Awake() {
        //Get a reference to the AIController
        aiController = GameObject.Find("AIController").GetComponent<AIController>() as AIController;
		uiController = GameObject.Find ("UIController").GetComponent<UIController> () as UIController;
	}

    /// <summary>
    /// AddCell is what the individual cell objects use to dynamically generate the level at runtime
    /// Cell objects detect their location in the world and pass that data to the LevelController via this method.
    /// This method generates corresponding entries in all relevant areas of the game state.
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    /// <param name="cellObj">Hex cell object</param>
    public void AddCell(int q, int r, int h, GameObject cellObj) {
        //Create a hex data object to go into the level grid
        HexCellData newCell = new HexCellData(q, r, h, cellObj);
        levelGrid[q,r,h] = newCell;

        //Create an ai hex object to go into the pathing grid
        AICell aiCell = new AICell(q, r, h);
        aiController[q,r,h] = aiCell;

        //Create a UI cell object to go into the UI grid
		UICell uiCell = new UICell(q,r,h);
		uiController.addCellToUIMap(uiCell);
        //Set the scale of the object to equal the world hex it represents
        uiController[q, r, h].setModelScale(cellObj.GetComponent<HexCellObj>().modelScale);
    }
}
