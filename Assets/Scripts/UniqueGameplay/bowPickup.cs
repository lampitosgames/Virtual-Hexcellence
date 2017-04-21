using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script that lets the player pick up a bow
/// </summary>
public class bowPickup : MonoBehaviour {
    Player player;
    bool init = false;

    void Update() {
        if (!init) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
            init = true;
        }

        HexCellObj hexObj = gameObject.GetComponent<HexCellObj>();

        if (!player.hasBow && player.q == hexObj.q && player.r == hexObj.r && player.h == hexObj.h) {
            player.hasBow = true;
        }
    }
}
