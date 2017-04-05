using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    //Note: Current inventory implementation is temporary and subject to change.

    //public int bobblesCollected=0;
    public float itemSearchRadius = 5.0f;
    //protected bool itemDropped=false;

    public List<GameObject> itemsContained;
    public List<GameObject> uiItems;
    public GameObject userInterface;
    private GameObject player;
    private GameObject vrController;
    GameObject inventoryUI;
    // Use this for initialization
    void Start()
    {
        inventoryUI = GameObject.Find("InventoryUI");
        player = GameObject.Find("Camera (eye)");
        SetupItemPickupUI();
        BuildInventoryUI();
    }
    void Awake()
    {
        vrController = GameObject.Find("Controller (right)");
    }
    // Update is called once per frame
    void Update()
    {
        //Check to see if we're dropping any items; if we are then we probably going to not want to pick them back up again immediately
        if (!Input.GetButtonUp("DropItem"))
        {
            List<GameObject> nearbyObjects = ItemCheck();
            if (nearbyObjects.Count > 0)
            {
                //Debug.Log("An item is nearby. Items collected: " + itemsContained.Count);
                if (Input.GetButtonUp("CollectItem"))
                {
                    for (int i = 0; i < nearbyObjects.Count; i++)
                    {
                        //if we have more than 12 items, the inventory is full
                        /*if (itemsContained.Count >= 12)
                        {
                            Debug.Log("Inventory full.");
                            i = nearbyObjects.Count;
                        }*/
                        AddItemToInventory(nearbyObjects[i], null);
                    }

                }
            }
            else
            {
                //Debug.Log("An item is not nearby. Items collected: " + itemsContained.Count);
            }
        }
        else
        {
            if (Input.GetButtonUp("DropItem"))
            {
                if (itemsContained.Count > 0)
                {
                    RemoveItemFromInventory(itemsContained.Count - 1);
                }
                else
                {
                    Debug.Log("Your inventory is already empty.");
                }
            }
        }


        ConstrainRotationOfUI();
        InventoryUIMovement(); //Needs work
    }

    //OUTDATED, DEPRECIATED
    //Check to see if there are any Items nearby.
    //Return whether there are Items nearby
    List<GameObject> ItemCheck()
    {
        List<GameObject> tempItems = new List<GameObject>();
        //Get all items
        //Used presuming that there will be fewer items than nearby GameObjects in collision radius.
        //This might need refactoring later, depending on if there's a more efficient method I could use.
        GameObject[] itemsGlobal = GameObject.FindGameObjectsWithTag("Item");
        for (int i = 0; i < itemsGlobal.Length; i++)
        {
            Vector3 temp = itemsGlobal[i].gameObject.transform.position - gameObject.transform.position;
            float itemDistSqrd = temp.sqrMagnitude; //squared distance used instead of distance to avoid expensive sqrt calculations

            if (itemDistSqrd < Mathf.Pow(itemSearchRadius, 2))
            {
                tempItems.Add(itemsGlobal[i]);
            }
        }
        return tempItems;
    }
    //Constrain UI rotation to be pointed at the player, but not actually attached to the player object. Prevents rotation in Zed axis
    void ConstrainRotationOfUI()
    {
        if (player)
        {
            Vector3 playerForward = player.transform.forward.normalized;
            userInterface.transform.position = new Vector3(player.transform.position.x + playerForward.x, player.transform.position.y, player.transform.position.z + playerForward.z);
            userInterface.transform.rotation = Quaternion.LookRotation(player.transform.forward);
        }
    }
    //New system of collecting items, grab an object in 3D, hit the collect button then it gets added to inventory
    public void CollectItem(GameObject item)
    {
        ParticleSystem.Destroy(item.transform.FindChild("ItemParticle").gameObject);
        Sprite icon = item.transform.FindChild("ItemCollectUI").GetComponent<ItemUI>().ReturnSpriteImage();
        Destroy(item.transform.FindChild("ItemParticle").gameObject);
        AddItemToInventory(item, icon);
        item.transform.DetachChildren();
        Destroy(item);
    }
    //Takes an item from the game world, moves it to the inventory and adds it to your inventory.
    void AddItemToInventory(GameObject item, Sprite icon)
    {
        itemsContained.Add(item);
        //item.gameObject.SetActive(false);
        AddUIElementToInventory(item, icon);
    }
    //Takes an item at the given index in the inventory and removes it.
    //Presumes that items contained indeed contains an item at that position.
    void RemoveItemFromInventory(int index)
    {
        //Move the item then reactivate it.
        GameObject temp = itemsContained[index];
        temp.gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward + gameObject.transform.right * Random.Range(-0.5f, 0.5f);
        temp.SetActive(true);
        itemsContained.RemoveAt(index);
        Debug.Log("Removed the item at " + index + " from inventory");
    }
    void BuildInventoryUI()
    {

    }
    //Build out the UI elements required to fill the inventory, right now spawning on canvas at 0,0
    void AddUIElementToInventory(GameObject item, Sprite icon)
    {
        string itemName = item.transform.Find("ItemName").GetComponent<Text>().text;
        string itemDesc = item.transform.Find("ItemDescription").GetComponent<Text>().text;

        GameObject inventoryItemParent = new GameObject();
        GameObject inventoryItemText = new GameObject();
        GameObject inventoryItemImage = new GameObject();

        AddCanvasComponents(inventoryItemParent);
        inventoryItemParent.name = "Item";

        inventoryItemText.AddComponent<Text>();
        AddCanvasComponents(inventoryItemText);
        inventoryItemText.name = "ItemText";

        Text inventoryText = inventoryItemText.GetComponent<Text>();
        inventoryText.text = itemName;
        inventoryText.fontSize = 14;
        inventoryText.alignment = TextAnchor.MiddleCenter;
        inventoryText.color = Color.white;
        inventoryText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        RectTransform textTrans = inventoryItemText.GetComponent<RectTransform>();
        textTrans.sizeDelta = new Vector2(100, 30);

        inventoryItemImage.AddComponent<Image>();
        AddCanvasComponents(inventoryItemImage);
        inventoryItemImage.name = "ItemImg";
        Image invImg = inventoryItemImage.GetComponent<Image>();
        invImg.sprite = icon;

        GameObject newItemParent = (GameObject)Instantiate(inventoryItemParent, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject newItemText = (GameObject)Instantiate(inventoryItemText, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject newItem = (GameObject)Instantiate(inventoryItemImage, new Vector3(0, 0, 0), Quaternion.identity);

        newItem.name = inventoryItemImage.name;
        newItem.transform.SetParent(newItemParent.transform, false);

        newItemText.transform.position = inventoryUI.transform.position;
        newItemText.name = inventoryItemText.name;
        newItemText.transform.SetParent(newItemParent.transform, false);
        newItemParent.name = inventoryItemParent.name;
        newItemParent.transform.SetParent(inventoryUI.transform, false);
        newItemParent.tag = "UI_Item";

        RectTransform parentItemTrans = newItemParent.GetComponent<RectTransform>();
        parentItemTrans.anchorMax = new Vector2(0, 1);
        parentItemTrans.anchorMin = new Vector2(0, 1);
        parentItemTrans.pivot = new Vector2(0, 1);
        parentItemTrans.anchoredPosition = new Vector2(0, 0);

        //float spacing = 125.0f;
        //int itemIndex = itemsContained.Count;
        parentItemTrans.localPosition = new Vector3(0, 0, 0);
        newItemText.GetComponent<RectTransform>().localPosition = new Vector3(50, -125, -0.01f);
        newItem.GetComponent<RectTransform>().localPosition = new Vector3(50, -50, -0.01f);

    }
    void AddCanvasComponents(GameObject uiElement)
    {
        uiElement.AddComponent<CanvasRenderer>();
        uiElement.AddComponent<RectTransform>();
    }
    void UpdateInventoryUI()
    {

    }
    //TODO: Move inventory with controller AND hide inventory UI on input
    void InventoryUIMovement()
    {
        GameObject[] uiItems = GameObject.FindGameObjectsWithTag("UI_Item");
        if (uiItems.Length > 0)
        {
            /*Vector3 forward = vrController.transform.TransformDirection(vrController.transform.forward);
            Vector3 toOther = inventoryUI.transform.position - vrController.transform.position;
            float dotProd = Vector3.Dot(forward, toOther);*/
            Vector3 heading = vrController.transform.forward - vrController.transform.position;
            Vector3 projection = Vector3.Project(heading, inventoryUI.transform.position);
            Debug.Log("MOVEMENT");
            Debug.Log(projection.x + " || " + projection.y + " || " + projection.z);
            foreach (GameObject uiElement in uiItems)
            {
                uiElement.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    //Add event listeners required to pick up the item on pressing the button in vr
    void SetupItemPickupUI()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            GameObject itemItself = item.transform.FindChild("Item").gameObject;
            GameObject itemUI = itemItself.transform.FindChild("ItemCollectUI").gameObject;
            //Button inventoryButton = itemUI.transform.FindChild("Button").gameObject.GetComponent<Button>();
            //inventoryButton.onClick.AddListener(() => CollectItem(itemItself));
        }
    }
}
