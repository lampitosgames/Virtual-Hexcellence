using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks monster stats
/// </summary>
public class MonsterStats : MonoBehaviour {
    //Stats:
    public float Health;

    /// <summary>
    /// Unity update function
    /// </summary>
    void Update() {
        //If the monster has no health left, disable it
        if (Health <= 0) {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Check for collisions
    /// </summary>
    /// <param name="collision">Automatically passed by unity</param>
    void OnCollisionEnter(Collision collision) {
        //If the collided gameobject is tagged as deadly (arrows, for instance)
        if (collision.gameObject.tag == "Deadly") {
            //Decrease health by an ammount
            Health -= collision.gameObject.GetComponent<WeaponProjectile>().damage;
        }
    }
}
