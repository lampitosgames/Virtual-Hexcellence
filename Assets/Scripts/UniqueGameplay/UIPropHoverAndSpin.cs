using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPropHoverAndSpin : MonoBehaviour {
    public float hoverSpeed = 1.0f;
    public float rotateSpeed = 40.0f;
    float scale = 0.02f;
    float angle = 0f;
    Vector3 initialPos, initialRot;
	// Use this for initialization
	void Start () {
        scale = GameObject.Find("UIController").GetComponent<UIController>().uiScale;
        initialPos = transform.position + new Vector3(0, 1.0f * scale, 0);
        initialRot = transform.rotation.eulerAngles;
    }
	
	// Update is called once per frame
	void Update () {
        angle += hoverSpeed * Time.deltaTime;
        if (angle > 360) { angle = 0; }
        transform.position = initialPos + new Vector3(0, scale * Mathf.Sin(angle), 0);
        transform.position = initialPos + new Vector3(0, scale * Mathf.Sin(angle), 0);
        transform.Rotate(Vector3.up * (rotateSpeed * Time.deltaTime));
    }
}
