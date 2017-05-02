using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstGoal : Goal {
    public GameObject gateToClose;
    public override void Accomplished() {
        gateToClose.SetActive(false);
        //clear the gate prop from the ui
        GameObject.Find("LevelController").GetComponent<LevelController>()[0, 3, 0].displayedPrefabs = new List<GameObject>();
        base.Accomplished();
    }
}