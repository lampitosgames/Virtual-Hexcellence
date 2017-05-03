using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woodenTarget : MonoBehaviour {
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Deadly") {
            gameObject.transform.parent.gameObject.SetActive(false);
            gameObject.SetActive(false);

        }
    }
}
