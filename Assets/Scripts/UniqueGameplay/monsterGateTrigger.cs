using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterGateTrigger : MonsterStats {
    public GameObject gate = null;
    public override void KillSelf() {
        gate.SetActive(false);
        base.KillSelf();
    }
}
