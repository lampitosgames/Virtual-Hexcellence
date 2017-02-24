using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//WARNING: HORRIBLE CODE AHEAD
//TODO: Delete this monstrocity
public class FPSControllerWalkingHighlight : MonoBehaviour {
    LevelController levelController;

    public Material currenthexmaterial;
    public Material normalhexmaterial;
    public Material neighborhexmaterial;
    public HexCellData[] neighbors = new HexCellData[0];

    // use this for initialization
    void Awake() {
        //get the grid
        levelController = GameObject.Find("LevelController").GetComponent("LevelController") as LevelController;
    }

    // update is called once per frame
    void Update() {
        foreach (HexCellData cell in neighbors) {
            cell.hexCellObject.GetComponent<Renderer>().material = normalhexmaterial;
        }

        //get the cell location of the player
        int[] cellIndex = HexConst.CoordToHexIndex(new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z));
        //get the cell the player is standing on
        HexCellData currenthex = levelController.levelGrid.GetHex(cellIndex[0], cellIndex[1], cellIndex[2]);
        if (currenthex != null) {
            currenthex.hexCellObject.GetComponent<Renderer>().material = currenthexmaterial;
        }
        neighbors = levelController.levelGrid.GetNeighbors(cellIndex[0], cellIndex[1], cellIndex[2]);
        for (int i = 0; i < neighbors.Length; i++) {
            neighbors[i].hexCellObject.GetComponent<Renderer>().material = neighborhexmaterial;
        }

    }
}
