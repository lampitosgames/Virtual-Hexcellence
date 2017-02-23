using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//WARNING: HORRIBLE CODE AHEAD
//TODO: Delete this monstrocity
public class FPSControllerWalkingHighlight : MonoBehaviour {
    LevelController levelController;

    public GameObject currenthex;

    public Material currenthexmaterial;
    public Material normalhexmaterial;
    public Material neighborhexmaterial;

    // use this for initialization
    void Awake() {
        //get the grid
        levelController = GameObject.Find("LevelController").GetComponent("LevelController") as LevelController;
    }

    // update is called once per frame
    void Update() {
        //get the cell location of the player
        int[] cellIndex = HexConst.CoordToHexIndex(new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z));
        Debug.Log(cellIndex[2]);
        //get the cell the player is standing on
        currenthex = levelController.levelGrid.GetHex(cellIndex[0], cellIndex[1], cellIndex[2]).hexCellObject;
        if (currenthex != null) {
            currenthex.GetComponent<Renderer>().material = currenthexmaterial;
        }
        HexCellData[] neighbors = levelController.levelGrid.GetNeighbors(cellIndex[0], cellIndex[1], cellIndex[2]);
        for (int i = 0; i < neighbors.Length; i++) {
            neighbors[i].hexCellObject.GetComponent<Renderer>().material = neighborhexmaterial;
        }

    }
}
