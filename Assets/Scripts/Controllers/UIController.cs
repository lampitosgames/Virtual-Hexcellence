using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the palyer's UI
/// </summary>
public class UIController : MonoBehaviour {
	//Assign the prefab in editor to spawn as minimap hex object
	public GameObject uiGridPrefab;
	public GameObject playerFigurePrefab;
    //uiGrid holds all UICells which relate to each hex on the map
    public HexGrid<UICell> uiGrid = new HexGrid<UICell>();
	private GameObject playerFigure;
    int[] figurePositionHex;
	LevelController levelController;

    //Materials for hexes
    public Material defaultHexMaterial;
    public Material neighborhexmaterial;
    public Material highlightMaterial;

    void Start(){
		levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
		spawnPlayerFigure ();
        setVisibility(false);
	}

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
        newHologramCell.GetComponent<UICellObj>().parent = cell;
        newHologramCell.GetComponent<UICellObj>().q = cell.q;
        newHologramCell.GetComponent<UICellObj>().r = cell.r;
        newHologramCell.GetComponent<UICellObj>().h = cell.h;
        //Put the cell into the UIGrid
        uiGrid[cell.q, cell.r, cell.h] = cell;
        //Set the game object's parent transform for scaling/rotation purposes.
		newHologramCell.transform.SetParent(gameObject.transform);
	}

    public void ShowValidMoves(List<int[]> hexCells) {
        foreach (int[] coords in hexCells) {
            uiGrid[coords[0], coords[1], coords[2]].gameObject.GetComponent<Renderer>().material = neighborhexmaterial;
        }
    }

    public void ClearCells() {
        foreach (UICell cell in uiGrid) {
            cell.gameObject.GetComponent<Renderer>().material = defaultHexMaterial;
        }
    }

	/// <summary>
	/// Create player figure if there isnt already one
	/// </summary>
	void spawnPlayerFigure(){
		if (!playerFigure) {
			playerFigure = (GameObject)Instantiate (playerFigurePrefab, transform.position, transform.rotation);
			playerFigure.transform.SetParent(gameObject.transform);
            //Subtract the player position
            figurePositionHex = HexConst.CoordToHexIndex(new Vector3(playerFigure.transform.position.x - transform.parent.position.x, playerFigure.transform.position.y - transform.parent.position.y, playerFigure.transform.position.z - transform.parent.position.z));
        } else{playerFigure.transform.position = this.transform.position;}
	}


	/// <summary>
	/// Called by a canvas button when minimap is open, Moves the player to the hex corresponding to the player figure
	/// </summary>
	public void doMove() {
		//Subtract the player position
		Vector3 scaledFigurePosition = new Vector3 (playerFigure.transform.position.x-transform.parent.position.x,playerFigure.transform.position.y-transform.parent.position.y,playerFigure.transform.position.z-transform.parent.position.z);
		//Scale up the position from the miniature to full scale and reset the y axis
		scaledFigurePosition = scaledFigurePosition * 50;
		scaledFigurePosition.y = scaledFigurePosition.y - 50;
		//Convert the position into hex coordinates and move the player
		int[] figurePositionHex = HexConst.CoordToHexIndex (scaledFigurePosition);
		if (levelController.levelGrid.GetHex(figurePositionHex[0], figurePositionHex[1], figurePositionHex[2]) != null) {
			print ("There is a Hex Here");
			Vector3 newPosition = HexConst.HexToWorldCoord(figurePositionHex[0], figurePositionHex[1], figurePositionHex[2]);
			GameObject.FindGameObjectWithTag ("Player").transform.position = newPosition;
            ClearCells();
		}else{
			Debug.LogError ("OH NO!! THERE IS NO HEX WHERE THE PLAYER FIGURE IS!!" + "q: "+figurePositionHex[0]+ "r: "+figurePositionHex[1]+ "h: "+figurePositionHex[2]);
		}
	}

    /// <summary>
    /// This is tomporary to test different scales and positions of the minimap
    /// </summary>
    void Update(){
		if(Input.GetKeyDown("up")){
			scaleandReposition ();
		}
		if(Input.GetKeyDown("b")){
			setVisibility (true);
		}
		if(Input.GetKeyDown("n")){
			setVisibility (false);
		}
		if(Input.GetKeyDown("v")){
			doMove();
		}
	}

	/// <summary>
	/// Rescale and move
	/// </summary>
	void scaleandReposition(){
		transform.localScale = transform.localScale*0.02f;
		transform.position = new Vector3 (0,1,0);
		spawnPlayerFigure ();
	}

	/// <summary>
	/// Enable/Disable the renderer this will eventually become on/off for the minimap
	/// </summary>
	public void setVisibility(bool visible){
		if (visible) {
			foreach (MeshRenderer renderer in transform.GetComponentsInChildren<MeshRenderer>()) {
				renderer.enabled = true;
			}
		} else {
			foreach (MeshRenderer renderer in transform.GetComponentsInChildren<MeshRenderer>()) {
				renderer.enabled = false;
			}
		}
	}

}
