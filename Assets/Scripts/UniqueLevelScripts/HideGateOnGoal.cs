using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideGateOnGoal : MonoBehaviour {
    public GameObject gate;

    public void disableGate() {
        gate.SetActive(false);
    }
}
