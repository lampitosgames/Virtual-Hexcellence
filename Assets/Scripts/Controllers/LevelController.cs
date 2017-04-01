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

    //A reference to the player
    public Player player;
    bool playerTurn = true;

    //Goal-related variables
    public int numOfGoals = 0;
    public List<int[]> goalHexes = new List<int[]>();
    bool win = false;
    bool lose = false;

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

    void Update() {
        if (!this.win && !this.lose) {
            if (numOfGoals <= 0) {
                this.EndGame(true);
            }
            //If it is the player turn
            if (playerTurn) {
                //Step through player's turn.  This will return true when the player's turn is over
                if (player.TakeTurn()) {
                    //Switch to monster's turn after the player turn
                    this.StartMonsterTurn();
                }
                //Not player turn
            } else {
                //If the monster turn is over
                if (aiController.MonsterTurn()) {
                    //Switch to player turn
                    this.StartPlayerTurn();
                }
            }
        }
    }

    public void StartMonsterTurn() {
        player.actionPoints = 0;
        player.playerMoving = false;
        foreach (Monster m in aiController.monsters) {
            m.ResetTurn();
        }
        this.playerTurn = false;
    }

    public void StartPlayerTurn() {
        player.actionPoints = 3;
        player.playerMoving = false;
        this.playerTurn = true;
    }

    public void EndGame(bool playerWon) {
        if (playerWon) {
            this.win = true;
        } else {
            this.lose = true;
        }
    }

    void OnGUI() {
        GUI.skin.label.fontSize = 72;
        if (this.win) {
            GUI.Label(new Rect(Screen.width/2 - 250, Screen.height/2 - 250, 500, 500), "You Win!");
        } else if (this.lose) {
            GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 500), "You Lose D:");
        }
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

    public void AddGoal(int q, int r, int h, GameObject goalObj) {
        this.levelGrid[q, r, h].hasGoal = true;
        this.levelGrid[q, r, h].goal = goalObj;
        this.numOfGoals += 1;
    }
}
