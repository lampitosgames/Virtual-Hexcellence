using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMonsterCollider : MonoBehaviour {
	public MonsterStats activeParent = null;
	
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Deadly") {
			activeParent.OnCollisionEnter(collision);
		}
	}
}
