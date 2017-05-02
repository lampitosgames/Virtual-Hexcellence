using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterGateTrigger : MonsterStats {
    public GameObject gate = null;
    public override void KillSelf() {
        gate.SetActive(false);
        GameObject.Find("LevelController").GetComponent<LevelController>()[-11, 8, 3].displayedPrefabs = new List<GameObject>();
        base.KillSelf();
    }
}
