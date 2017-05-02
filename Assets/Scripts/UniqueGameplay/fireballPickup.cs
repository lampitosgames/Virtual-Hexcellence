using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballPickup : MonoBehaviour {
    public GameObject fireballWorldObject = null;
    public GameObject uiPrefab;
    Player player;
    bool init = false;

    void Update() {
        HexCellObj hexObj = gameObject.GetComponent<HexCellObj>();
        if (!init) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
            GameObject.Find("LevelController").GetComponent<LevelController>().AddDisplayedObject(hexObj.q, hexObj.r, hexObj.h, this.uiPrefab);
            init = true;
        }
        
        if (!player.hasFireball && player.q == hexObj.q && player.r == hexObj.r && player.h == hexObj.h) {
            player.hasFireball = true;
            fireballWorldObject.SetActive(false);
            //Clear the ui prefab
            GameObject.Find("LevelController").GetComponent<LevelController>()[hexObj.q, hexObj.r, hexObj.h].displayedPrefabs = new List<GameObject>();
            GameObject.Find("UIController").GetComponent<UIController>()[hexObj.q, hexObj.r, hexObj.h].forceGoalMaterial = false;
        }
    }
}
