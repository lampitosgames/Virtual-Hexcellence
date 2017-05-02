using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addUIProp : MonoBehaviour {
    public GameObject uiPrefab;
    bool init = false;

    void Update() {
        HexCellObj hexObj = gameObject.GetComponent<HexCellObj>();
        if (!init) {
            GameObject.Find("LevelController").GetComponent<LevelController>().AddDisplayedObject(hexObj.q, hexObj.r, hexObj.h, this.uiPrefab);
            GameObject.Find("UIController").GetComponent<UIController>()[hexObj.q, hexObj.r, hexObj.h].forceGoalMaterial = false;
            init = true;
        }
    }
}
