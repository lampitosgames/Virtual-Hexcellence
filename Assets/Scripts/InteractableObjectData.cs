using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class InteractableObjectData : MonoBehaviour
{
    VRTK_InteractableObject interactionData;
    GameObject controllerData;
    InventoryController inventoryCont;
    void Start()
    {
        inventoryCont = GameObject.Find("InventoryController").GetComponent<InventoryController>();
        interactionData = this.gameObject.GetComponent<VRTK_InteractableObject>();

        if (interactionData != null)
        {
            interactionData.InteractableObjectGrabbed += new InteractableObjectEventHandler(GrabbedObject);
            interactionData.InteractableObjectUngrabbed += new InteractableObjectEventHandler(UngrabbedObject);
            Debug.Log("Attach Script");
        }
        if(this.transform.FindChild("ItemParticle").gameObject != null)
        {
            this.transform.FindChild("ItemParticle").gameObject.GetComponent<ParticleSystem>().Play();
            this.transform.FindChild("ItemCollectParticle").gameObject.GetComponent<ParticleSystem>().Stop();
        }
    }
    public void GrabbedObject(object obj, InteractableObjectEventArgs e)
    {
        
        Debug.Log(this.gameObject + " Attached to Controller");
        controllerData = e.interactingObject;
        inventoryCont.GrabbedObjectController = controllerData;
        inventoryCont.GrabbedObject = this.gameObject;
    }
    public void UngrabbedObject(object obj, InteractableObjectEventArgs e)
    {
        Debug.Log(this.gameObject + " Detached to Controller");
        inventoryCont.GrabbedObject = null;
    }
}
