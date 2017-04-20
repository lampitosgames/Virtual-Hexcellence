using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows player to shoot in first person
/// </summary>
public class FPSArrowShooting : MonoBehaviour {
    public GameObject arrowPrefab = null;
    Player player = null;
    GameObject playerCamera;

    void Start() {
        //Get a reference to the player
        player = gameObject.GetComponent<Player>();
        //Get a reference to the main game camera
        playerCamera = GetComponentInChildren<Camera>().gameObject;
    }

    // Update is called once per frame
    void Update() {
        //If player clicks and isn't taking actions
        if (Input.GetMouseButtonUp(0) && !player.playerActing) {
            Vector3 start = playerCamera.transform.position;
            //start += playerCamera.transform.forward.normalized;
            GameObject newArrow = Instantiate(arrowPrefab, start, playerCamera.transform.rotation);
            newArrow.GetComponent<Rigidbody>().velocity = 20 * playerCamera.transform.forward.normalized;
        }
    }
}