using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetTracker : MonoBehaviour {
    public GameObject target1 = null;
    public GameObject target2 = null;
    public GameObject target3 = null;

    public GameObject monsterToActivate = null;

    private bool triggered = false;

    // Update is called once per frame
    void Update () {
		if (!triggered && !target1.activeSelf && !target2.activeSelf && !target3.activeSelf) {
            monsterToActivate.SetActive(true);
            triggered = true;
            //Auto-trigger monster turn
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
            player.actionPoints = 0;
        }
	}
}
