using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples.Archery;

public class Weapon_Damage : MonoBehaviour {

    public bool StickToCollision;
    public Rigidbody attatchedBody;
    public float damage;

    private bool hasCollided = false;
    private GameObject hit;
    private Vector3 location;

    void Start() {
        //Ignore the FPSController bounds
        foreach (GameObject playerColliderBox in GameObject.FindGameObjectsWithTag("PlayerCollisionBox")) {
            Physics.IgnoreCollision(playerColliderBox.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        }
    }

    void Update() {
        if (hasCollided) {
            location = hit.transform.position + (gameObject.transform.position - hit.transform.position);
            gameObject.transform.position = location;
            if (hit.activeSelf == false || hit == null) {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.relativeVelocity.magnitude > 0.7f) {
            if (StickToCollision) {
                hasCollided = true;
                hit = collision.gameObject;
                location = hit.transform.position + (gameObject.transform.position - hit.transform.position);
                GetComponent<Rigidbody>().isKinematic = true;
                attatchedBody.isKinematic = true;
            }
        }
    }
}