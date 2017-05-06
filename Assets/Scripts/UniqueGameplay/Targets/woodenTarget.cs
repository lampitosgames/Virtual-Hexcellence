using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woodenTarget : MonoBehaviour {
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Deadly") {
			StartCoroutine(FlashSelf());
        }
    }


	private IEnumerator FlashSelf() {
		yield return new WaitForSeconds (0.8f);
		gameObject.transform.parent.gameObject.SetActive(false);
		gameObject.SetActive(false);
	}
}
