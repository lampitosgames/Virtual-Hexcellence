using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples.Archery;

/// <summary>
/// Attached to projectiles.
/// Currently only used for arrows.  Specifies damage dealt upon sticking into a monster
/// This script also allows projectiles to remain in monsters
/// </summary>
public class WeaponProjectile : MonoBehaviour {
    //How much damage does the projectile deal?
    public float damage;
    //Should the projectile stick to the enemy it collides with?
    public bool StickToCollision;
    public Rigidbody attatchedBody;

    /// <summary>
    /// Unity's collision method
    /// </summary>
    /// <param name="collision">automatically passed by unity</param>
    void OnCollisionEnter(Collision collision) {
        //Debug.Log (collision.relativeVelocity.magnitude);
        //If the projectile was moving fast enough
        if (collision.relativeVelocity.magnitude > 0.7f) {
            //If the object should stick to things it damages
            if (StickToCollision && collision.gameObject.tag == "Target") {
                //Set the parent transform
                gameObject.transform.parent = collision.gameObject.transform;
                //Attach to the other rigidbody
                GetComponent<Rigidbody>().isKinematic = true;
                attatchedBody.isKinematic = true;
                //Debug.Log ("Good Collison");
            }
        }
    }
}
