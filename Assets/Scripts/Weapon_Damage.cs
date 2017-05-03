using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples.Archery;

public class Weapon_Damage : MonoBehaviour {

    public bool StickToCollision;
    public Rigidbody attatchedBody;
    public float damage;
	public Material flashmat;

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
			if (hit == null || hit.activeSelf == false) {
				Destroy(gameObject);
			}
			if (hit != null && gameObject != null) {
				location = hit.transform.position + (gameObject.transform.position - hit.transform.position);
				gameObject.transform.position = location;
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
				if (collision.gameObject.tag == "Target") {
					StartCoroutine (FlashMaterial (collision.gameObject));
				}
            }
        }
    }

	private IEnumerator FlashMaterial(GameObject objToFlash) {
		List<Material> materials = new List<Material> ();
		GameObject toFlash = objToFlash;
		try {
			while (toFlash.GetComponent<Monster> () == null) {
				toFlash = toFlash.transform.parent.gameObject;
			}
		} catch {
			toFlash = objToFlash;
		}

		foreach (Renderer r in toFlash.GetComponentsInChildren<Renderer>()) {
			materials.Add (r.material);
			r.material = flashmat;
		}
		yield return new WaitForSeconds (0.15f);
		if (toFlash != null) {
			int i = 0;
			foreach (Renderer r in toFlash.GetComponentsInChildren<Renderer>()) {
				r.material = materials [i];
				i++;
			}
		}
	}
}