using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour {

	public Vector3 spinRate;

	
	// Update is called once per frame
	void Update () {
		Vector3 scaledRate = spinRate * Time.deltaTime;
		Vector3 newVector = new Vector3 (transform.rotation.eulerAngles.x + scaledRate.x, transform.rotation.eulerAngles.y + scaledRate.y, transform.rotation.eulerAngles.z + scaledRate.z);
		transform.rotation = Quaternion.Euler (newVector); 
	}
}
