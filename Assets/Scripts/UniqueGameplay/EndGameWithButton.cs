using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameWithButton : MonoBehaviour {
	public LevelController levelController;
	// Use this for initialization
	void Start () {
		levelController = GameObject.Find ("LevelController").GetComponent<LevelController> () as LevelController;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Q)) {
			levelController.EndGame (true);
		}
	}
}
