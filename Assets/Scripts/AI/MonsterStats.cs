using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : MonoBehaviour {

	public float Health;

	// Use this for initialization
	void Update () {
		if (Health <= 0) {
			GetComponent<MeshRenderer> ().enabled = false;
		}
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Deadly") {
			Health -= collision.gameObject.GetComponent<Weapon_Damage> ().damage;

		}
	}
}
