using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Satchel : MonoBehaviour {

    public Material defaultMatertial;
    public Material highlightMaterial;
    InventoryController invController;
    UIController uiController;

    public float hoverDistance = 0.25f;

    
    
    // Use this for initialization
    void Start ()
    {
        invController = GameObject.Find("InventoryController").GetComponent<InventoryController>();
        uiController = GameObject.Find("UIController").GetComponent<UIController>() as UIController;
        GetComponent<Renderer>().material = defaultMatertial;
    }
	void Update()
    {
        
        if (invController.GrabbedObjectController != null && invController.GrabbedObject != null)
        {
            DetectHover(invController.GrabbedObjectController.transform.position);
        }
    }
	public void DetectHover(Vector3 controllerPos)
    {
        float distanceToController = Vector3.Distance(this.transform.position, controllerPos);
        if(distanceToController < hoverDistance)
        {
            GetComponent<Renderer>().material = highlightMaterial;
            StartCoroutine("AddToInventory");
        }
        else
        {
            GetComponent<Renderer>().material = defaultMatertial;
        }
    }
    IEnumerator AddToInventory()
    {
        GameObject item = invController.GrabbedObject;
        uiController.DisplayUserInterface(true);
        uiController.toggleInterfaces("itemCollected");
        item.transform.FindChild("ItemParticle").gameObject.GetComponent<ParticleSystem>().Stop();
        item.transform.FindChild("ItemCollectParticle").gameObject.GetComponent<ParticleSystem>().Play();
        //ParticleSystem.Destroy(item.transform.FindChild("ItemParticle").gameObject);
        yield return new WaitForSeconds(0.15f);
        invController.CollectItem(item);
        yield return new WaitForSeconds(2.5f);
        uiController.toggleInterfaces("inventory");
        
    }
}
