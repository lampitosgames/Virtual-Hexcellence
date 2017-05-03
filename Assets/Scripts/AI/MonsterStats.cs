using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : MonoBehaviour {

	public float Health;
    AIController aiController;

    void Start() {
        aiController = GameObject.Find("AIController").GetComponent<AIController>();

		//Give all collider children a childMonsterCollider object
		foreach (Collider c in gameObject.GetComponentsInChildren<Collider>()) {
			if (c.gameObject != gameObject) {
				c.gameObject.AddComponent<ChildMonsterCollider> ();
				c.gameObject.GetComponent<ChildMonsterCollider> ().activeParent = this;
			}
			c.gameObject.tag = "Target";
		}
    }

	// Use this for initialization
	void Update () {
		if (Health <= 0) {
            this.KillSelf();
		}
	}
	
	public void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Deadly") {
			Health -= collision.gameObject.GetComponent<Weapon_Damage> ().damage;
		}
	}

    public virtual void KillSelf() {
        //Remove self from AIController
        Monster self = gameObject.GetComponent<Monster>();
        aiController.monsters.Remove(self);
        aiController[self.CurrentCell[0], self.CurrentCell[1], self.CurrentCell[2]].hasEnemy = false;
        this.gameObject.SetActive(false);
    }
}
