using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDestroy : MonoBehaviour {

	public float time;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		time = time - Time.deltaTime;

		if (time <= 0) {
			Destroy (gameObject);
		}
	}
}
