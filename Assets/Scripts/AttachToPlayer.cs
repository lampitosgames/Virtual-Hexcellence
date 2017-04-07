using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlayer : MonoBehaviour {

    private GameObject player;
    private Vector3 offset;
    private Vector3 initialRot;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Camera (eye)") as GameObject;
        offset = this.transform.position;
        this.transform.position = player.transform.position + offset;
        initialRot = this.transform.eulerAngles;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 newPos = new Vector3(playerPos.x + offset.x, offset.y, playerPos.z + offset.z);
        Quaternion playerLook = Quaternion.LookRotation(player.transform.forward);
        
        this.transform.localRotation = new Quaternion(0, playerLook.y, 0, playerLook.w);
        this.transform.Rotate(new Vector3(initialRot.x, initialRot.y, initialRot.z));
        this.transform.position = newPos;
	}
}
