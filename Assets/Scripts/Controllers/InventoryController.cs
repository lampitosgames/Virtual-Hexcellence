using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {
    //Note: Current inventory implementation is temporary and subject to change.

    //public int bobblesCollected=0;
    public float itemSearchRadius = 5.0f;
    public KeyCode collectKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.X;
    //protected bool itemDropped=false;

    public List<GameObject> itemsContained;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //Check to see if we're dropping any items; if we are then we probably going to not want to pick them back up again immediately
        if (!Input.GetKey(dropKey)) {
            List<GameObject> nearbyObjects = ItemCheck();
            if (nearbyObjects.Count > 0) {
                //Debug.Log("An item is nearby. Items collected: " + itemsContained.Count);
                if (Input.GetKeyDown(collectKey)) {
                    for (int i = 0; i < nearbyObjects.Count; i++) {
                        //if we have more than 12 items, the inventory is full
                        if (itemsContained.Count >= 12) {
                            //Debug.Log("Inventory full.");
                            i = nearbyObjects.Count;
                        }
                        AddItemToInventory(nearbyObjects[i]);
                    }

                }
            } else {
                //Debug.Log("An item is not nearby. Items collected: " + itemsContained.Count);
            }
        } else {
            if (Input.GetKeyDown(dropKey)) {
                if (itemsContained.Count > 0) {
                    RemoveItemFromInventory(itemsContained.Count - 1);
                } else {
                    //Debug.Log("Your inventory is already empty.");
                }
            }
        }
    }

    //Check to see if there are any Items nearby and add them if any are present.
    //Return whether there are Items nearby
    List<GameObject> ItemCheck() {
        List<GameObject> tempItems = new List<GameObject>();
        //Get all items
        //Used presuming that there will be fewer items than nearby GameObjects in collision radius.
        //This might need refactoring later, depending on if there's a more efficient method I could use.
        GameObject[] itemsGlobal = GameObject.FindGameObjectsWithTag("Item");
        for (int i = 0; i < itemsGlobal.Length; i++) {
            Vector3 temp = itemsGlobal[i].gameObject.transform.position - gameObject.transform.position;
            float itemDistSqrd = temp.sqrMagnitude; //squared distance used instead of distance to avoid expensive sqrt calculations

            if (itemDistSqrd < Mathf.Pow(itemSearchRadius, 2)) {
                tempItems.Add(itemsGlobal[i]);
            }
        }
        return tempItems;
    }

    //Takes an item from the game world, moves it to the inventory and adds it to your inventory.
    void AddItemToInventory(GameObject item) {
        itemsContained.Add(item);
        item.gameObject.SetActive(false);
    }

    //Takes an item at the given index in the inventory and removes it.
    //Presumes that items contained indeed contains an item at that position.
    void RemoveItemFromInventory(int index) {
        //Move the item then reactivate it.
        GameObject temp = itemsContained[index];
        temp.gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward + gameObject.transform.right * Random.Range(-0.5f, 0.5f);
        temp.SetActive(true);
        itemsContained.RemoveAt(index);
        //Debug.Log("Removed the item at "+index+" from inventory");
    }
}
