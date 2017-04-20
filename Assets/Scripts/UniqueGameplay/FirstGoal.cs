using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstGoal : Goal {
    public GameObject gateToClose;
    public override void Accomplished() {
        gateToClose.SetActive(false);
        base.Accomplished();
    }
}