using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The LevelController is the stateful holder of level data.
/// It will accept info from lower objects and change the game state accordingly.
/// The level controller handles the game looop.
/// </summary>
public class LevelController : MonoBehaviour {
    //The level grid holds HexCellData objects, the meat of cell states
    public HexGrid<HexCellData> levelGrid = new HexGrid<HexCellData>();
    //References to other controllers
    public AIController aiController; //Handles AI and pathfinding
	public UIController uiController; //Handles UI interactions

    //A reference to the player
    public Player player = null;
    //Is it the player's turn
    bool playerTurn = true;

    //Goal-related variables
    public int numOfGoals = 0;
    public List<int[]> goalHexes = new List<int[]>();
    bool win = false;
    bool lose = false;
    private bool initGoals = false;

    //temporary for debugging
    public int cellsReady;

    //Prefab for goals on minimap
    public GameObject goalPrefab = null;

    /// <summary>
    /// Allow getting/setting for the level grid using [q,r,h]
    /// </summary>
    /// <param name="q">column</param>
    ///  <param name="r">row</param>
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
        //Get a references to other controllers
        aiController = GameObject.Find("AIController").GetComponent<AIController>() as AIController;
		uiController = GameObject.Find ("UIController").GetComponent<UIController>() as UIController;
	}

    /// <summary>
    /// Main game loop
    /// </summary>
    void Update() {
        //If the player has not won or lost
        if (!this.win && !this.lose && this.initGoals) {
            //If the player has reached all goals, end the game with the player winning
            if (numOfGoals <= 0) {
                this.EndGame(true);
            }

            //If it is the player turn
            if (playerTurn) {
                //Step through player's turn.  This will return true when the player's turn is over
                //DOCUMENTATION NOTE:
                //This is the common structure for all game loop methods.  The LevelController's update function gets stuck, logically speaking, on calling this
                //funciton every loop until it returns true.  This tells the player object to run trhough its turn logic while still allowing all objects to have
                //individual, complex behavior like animation and basic movement (i.e. the player can move around the hex on the monster's turn).
                //This is a common paradigm across the project
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

    /// <summary>
    /// Method that simply resets state variables so the game loop will determine the monsters to be active.
    /// </summary>
    public void StartMonsterTurn() {
        player.actionPoints = 0;
        player.playerActing = false;
        foreach (Monster m in aiController.monsters) {
            m.ResetTurn();
        }
        this.playerTurn = false;
    }

    /// <summary>
    /// Method that resets state variables so the game loop will determine the player to be active
    /// </summary>
    public void StartPlayerTurn() {
        player.actionPoints = 3;
        player.playerActing = false;
        this.playerTurn = true;
    }

    /// <summary>
    /// Ends the game!  Whether the player wins or loses, sets the global state of win/loss
    /// </summary>
    /// <param name="playerWon">Did the player win?</param>
    public void EndGame(bool playerWon) {
        if (playerWon) {
            this.win = true;
        } else {
            this.lose = true;
        }
    }

    /// <summary>
    /// TEMP: Draws the win state to the screen
    /// </summary>
    void OnGUI() {
        //Debugging
        //GUI.skin.label.fontSize = 18;
        //GUI.Label(new Rect(300, 100, 360, 20), "Called AddCell "+cellsReady+" times");
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

        //Create an pathing hex object to go into the pathing grid
        PathCell pathCell = new PathCell(q, r, h);
        aiController[q,r,h] = pathCell;

        //Create a UI cell object to go into the UI grid
		UICell uiCell = new UICell(q,r,h);
		uiController.addCellToUIMap(uiCell);
        //Set the scale of the object to equal the world hex it represents
        uiController[q, r, h].setModelScale(cellObj.GetComponent<HexCellObj>().modelScale);

        cellsReady++;
    }

    /// <summary>
    /// AddGoal is what individual goal objects use to inject themselves into the level win condition.
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">heignt</param>
    /// <param name="goalObj">self reference</param>
    public void AddGoal(int q, int r, int h, GameObject goalObj) {
        this.levelGrid[q, r, h].hasGoal = true;
        this.levelGrid[q, r, h].goal = goalObj;
        this.numOfGoals += 1;
        this.initGoals = true;
    }

    //If a cell has a goal, return
    //Works like GetEnemy in AIController 
    public GameObject GetGoal(int q, int r, int h)
    {
        //Null check
        if (levelGrid[q, r, h] == null)
        {
            return null;
        }
        if (levelGrid[q, r, h].hasGoal)
        {
            return goalPrefab;
        }

        return null;
    }
}
