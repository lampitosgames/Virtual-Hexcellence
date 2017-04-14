using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO: Severely refactor this to be more robust
/// Everything having to do with the inventory.  Interactions in the game world can put items into the player's inventory (like weapons and ability runes)
/// </summary>
public class InventoryController : MonoBehaviour {
    //Radius in the world to search for items when picking up
    //This functionality is temporary
    public float itemSearchRadius = 5.0f;
    public KeyCode collectKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.X;
    //Items in the inventory
    public List<GameObject> itemsContained;

    /// <summary>
    /// Unity update method
    /// </summary>
    void Update() {
        //If the player is trying to pick up an item (and not currently dropping one)
        if (!Input.GetKey(dropKey) && Input.GetKeyUp(collectKey)) {
            //Get nearby objects
            List<GameObject> nearbyObjects = ItemCheck();
            //If objects are nearby for pickup
            if (nearbyObjects.Count > 0) {
                //Add the first one to the inventory
                AddItemToInventory(nearbyObjects[0]);
            }

            //If the player is dropping an item
        } else if (Input.GetKeyUp(dropKey) && itemsContained.Count > 0) {
            //Drop it
            RemoveItemFromInventory(itemsContained.Count - 1);
        }
    }

    /// <summary>
    /// Check to see if there are any items nearby
    /// </summary>
    /// <returns>List of nearby items</returns>
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

    /// <summary>
    /// Places an item from the game world into the player's inventory
    /// </summary>
    /// <param name="item">Item in the game</param>
    void AddItemToInventory(GameObject item) {
        itemsContained.Add(item);
        item.gameObject.SetActive(false);
    }

    /// <summary>
    /// Removes an item at a given index in the inventory
    /// </summary>
    /// <param name="index">Index of the item in the items list</param>
    void RemoveItemFromInventory(int index) {
        //Move the item to the player and then re-activate it
        GameObject item = itemsContained[index];
        //This transforms it to be located at the inventory controller?
        item.gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward + gameObject.transform.right * Random.Range(-0.5f, 0.5f);
        item.SetActive(true);
        itemsContained.RemoveAt(index);
    }
}
