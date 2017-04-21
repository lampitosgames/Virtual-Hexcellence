using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script that lets the player pick up a bow
/// </summary>
public class bowPickup : MonoBehaviour {
    Player player;
    bool init = false;

    public GameObject bow = null;
    public List<GameObject> arrows = new List<GameObject>();

    void Update() {
        if (!init) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
            init = true;
        }

        HexCellObj hexObj = gameObject.GetComponent<HexCellObj>();

        if (!player.hasBow && player.q == hexObj.q && player.r == hexObj.r && player.h == hexObj.h) {
            player.hasBow = true;
            GameObject parentPlayer = GameObject.FindGameObjectWithTag("Player");
            bow.transform.parent = parentPlayer.transform;
            foreach (GameObject a in arrows) {
                a.transform.parent = parentPlayer.transform;
            }
        }
    }
}
