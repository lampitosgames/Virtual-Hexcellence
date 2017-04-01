using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    private bool addedSelf = false;

    public bool reached = false;
    public int q, r, h;
    // Use this for initialization
    void Start() {
        float modelHeight = gameObject.GetComponent<Renderer>().bounds.size.y;
        //Get the hex index for this hex cell.  Pass in the transform.
        int[] thisHexIndex = HexConst.CoordToHexIndex(transform.position + new Vector3(0, -0.5f * modelHeight, 0));
        q = thisHexIndex[0];
        r = thisHexIndex[1];
        h = thisHexIndex[2];
    }

    void Update() {
        if (!addedSelf) {
            //Save the grid game object
            LevelController levelController = GameObject.Find("LevelController").GetComponent("LevelController") as LevelController;
            //Tell the level controller to initialize this hex cell
            levelController.AddGoal(q, r, h, gameObject);
            addedSelf = true;
        }
    }
}
