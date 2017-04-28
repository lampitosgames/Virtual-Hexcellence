using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's UI
/// </summary>
public class UIController : MonoBehaviour {
    //Assign the prefab in editor to spawn as minimap hex object
    public GameObject uiGridPrefab;
	public GameObject playerFigurePrefab;
	public GameObject playerFigure;
    //uiGrid holds all UICells which relate to each hex on the map
    public HexGrid<UICell> uiGrid = new HexGrid<UICell>();
    int[] figurePositionHex;
    LevelController levelController;
	AIController aiController;

	Player player = null;
	Vector3 uiControllerCenterPos = new Vector3(0, 1, 0);

    public float uiScale = 0.02f; //the scale of the minimap compared to the world map.

    //Materials for hexes
    public Material defaultHexMaterial;
    public Material possibleMoveMat1;
    public Material possibleMoveMat2;
    public Material possibleMoveMat3;
    public Material highlightMaterial;

	void Start() {
		levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
		aiController = GameObject.Find("AIController").GetComponent<AIController>();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Player> ();
        
        StartCoroutine(LoadTileCheck()); //Coroutine that waits for all cells to become ready before continuing.
    }

    //Helper coroutine for Start
    //Makes sure all tiles are completely loaded before scaling, repositioning, and setting visibility.
    //In absence of this fix, approx. 16 tiles and the player figure load before the rest, and are scaled down+set invisible while the rest remain at full size.
    IEnumerator LoadTileCheck()
    {
        spawnPlayerFigure();
        yield return new WaitUntil(() => levelController.cellsReady>=236); //wait until all cells are ready; should probably futureproof this for diff. numbers of tiles
        scaleandReposition();//properly scales down the UI grid.
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
    public void addCellToUIMap(UICell cell) {
        //Create a new GameObject to represent a location in the world
        GameObject newHologramCell = (GameObject)Instantiate(uiGridPrefab, cell.centerPos, transform.rotation);
        cell.setGameObject(newHologramCell);
		cell.aiController = this.aiController;
        newHologramCell.GetComponent<UICellObj>().parent = cell;
        newHologramCell.GetComponent<UICellObj>().q = cell.q;
        newHologramCell.GetComponent<UICellObj>().r = cell.r;
        newHologramCell.GetComponent<UICellObj>().h = cell.h;
        //Put the cell into the UIGrid
        uiGrid[cell.q, cell.r, cell.h] = cell;
        //Set the game object's parent transform for scaling/rotation purposes.
        newHologramCell.transform.SetParent(gameObject.transform);
    }

    //Show valid moves for the player's move command.
    public void ShowValidMoves(List<List<PathCell>> hexCells) {
        Material matToUse;
        for (int i = 0; i < hexCells.Count; i++) {
            foreach (PathCell coords in hexCells[i]) {
                if (i == 0) {
                    matToUse = possibleMoveMat1;
                } else if (i == 1) {
                    matToUse = possibleMoveMat2;
                } else {
                    matToUse = possibleMoveMat3;
                }
                uiGrid[coords.q, coords.r, coords.h].gameObject.GetComponent<Renderer>().material = matToUse;
            }
        }
    }

    /// <summary>
    /// Displays the top-down radius around an area.
    /// Currently used for ability ranges.
    /// Will likely abstract some of this at some point.
    /// </summary>
    public void ShowValidTopDownRadius(int q, int r, int h, int radius, bool includeOrigin = false, TargetingMaterial matParam = TargetingMaterial.COSTS_ONE) {
        Material matToUse;
        switch (matParam) {
            case TargetingMaterial.COSTS_ONE: matToUse = possibleMoveMat1; break;
            case TargetingMaterial.COSTS_TWO: matToUse = possibleMoveMat2; break;
            case TargetingMaterial.COSTS_THREE: matToUse = possibleMoveMat3; break;
            case TargetingMaterial.TARGETED_ZONE: matToUse = highlightMaterial; break;
            default: matToUse = possibleMoveMat1; break;
        }
        UICell[] topDown = uiGrid.TopDownRadius(q, r, h, radius, includeOrigin);
        foreach (UICell rangeTile in topDown) {
            rangeTile.gameObject.GetComponent<Renderer>().material = matToUse;
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
    void spawnPlayerFigure() {
        if (!playerFigure) {
            playerFigure = (GameObject)Instantiate(playerFigurePrefab, transform.position, transform.rotation);
            playerFigure.transform.SetParent(gameObject.transform);
            //Subtract the player position
            figurePositionHex = HexConst.CoordToHexIndex(new Vector3(playerFigure.transform.position.x - transform.parent.position.x, playerFigure.transform.position.y - transform.parent.position.y, playerFigure.transform.position.z - transform.parent.position.z));
        } else {
			playerFigure.transform.position = HexConst.HexToWorldCoord(player.q, player.r, player.h) + uiControllerCenterPos;
        }
    }

    /// <summary>
    /// Rescale and move
    /// </summary>
    void scaleandReposition() {
        transform.localScale = transform.localScale * uiScale;
        transform.position = uiControllerCenterPos;
        spawnPlayerFigure();
    }

    /// <summary>
    /// Enable/Disable the renderer this will eventually become on/off for the minimap
    /// </summary>
    public void setVisibility(bool visible) {
		//Show UI
        if (visible) {
			//Get cells in a radius
			UICell[] toDisplay = uiGrid.GetRadius (player.q, player.r, player.h, 8, -2, true);
			//Display them all
			foreach (UICell c in toDisplay) {
				c.Display (true);
			}

			//Center the minimap
			Vector3 translationVec = HexConst.HexToWorldCoord(player.q, player.r, player.h) + uiControllerCenterPos - uiGrid[player.q,player.r,player.h].gameObject.transform.position;
			gameObject.transform.position += translationVec;

			//Show player figure
			spawnPlayerFigure();
			foreach (MeshRenderer r in playerFigure.GetComponentsInChildren<MeshRenderer>()) {
				r.enabled = true;
			}
		
		//Hide UI
        } else {
			//Hide all UICells
			foreach (UICell c in uiGrid) {
				c.Display (false);
			}
			//Hide player figure
			foreach (MeshRenderer r in playerFigure.GetComponentsInChildren<MeshRenderer>()) {
				r.enabled = false;
			}
        }
    }

}
