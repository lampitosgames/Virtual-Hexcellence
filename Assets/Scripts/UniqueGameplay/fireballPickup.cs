using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballPickup : MonoBehaviour {
    public GameObject fireballWorldObject = null;
    Player player;
    bool init = false;

    void Update() {
        if (!init) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
            init = true;
        }

        HexCellObj hexObj = gameObject.GetComponent<HexCellObj>();

        if (!player.hasFireball && player.q == hexObj.q && player.r == hexObj.r && player.h == hexObj.h) {
            player.hasFireball = true;
            fireballWorldObject.SetActive(false);
        }
    }
}
