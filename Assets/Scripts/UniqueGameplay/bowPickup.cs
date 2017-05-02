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
    public GameObject uiPrefab = null;

    void Update() {
        HexCellObj hexObj = gameObject.GetComponent<HexCellObj>();

        if (!init) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
            GameObject.Find("LevelController").GetComponent<LevelController>().AddDisplayedObject(hexObj.q, hexObj.r, hexObj.h, this.uiPrefab);
            init = true;
        }
        
        if (!player.hasBow && player.q == hexObj.q && player.r == hexObj.r && player.h == hexObj.h) {
            player.hasBow = true;
            GameObject parentPlayer = GameObject.FindGameObjectWithTag("Player");
            bow.transform.parent = parentPlayer.transform;
            foreach (GameObject a in arrows) {
                a.transform.parent = parentPlayer.transform;
            }
            //Clear the ui prefab
            GameObject.Find("LevelController").GetComponent<LevelController>()[hexObj.q, hexObj.r, hexObj.h].displayedPrefabs = new List<GameObject>();
            GameObject.Find("UIController").GetComponent<UIController>()[hexObj.q, hexObj.r, hexObj.h].forceGoalMaterial = false;
        }
    }
}
