using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for minimap items
/// Other classes can inherit this to implement different item functions.
/// Yes, this is very similar to Goal and should probably be abstracted into it
/// </summary>
public class SpecialItem : Objective {
    public GameObject minimapObject;


    public void Update()
    {
        //Can't add self in the start() function because it is creation order dependant on hex cells
        if (!addedSelf)
        {
            //Save the grid game object
            levelController = GameObject.Find("LevelController").GetComponent("LevelController") as LevelController;
            //Tell the level controller to initialize this hex cell
            levelController.AddItem(q, r, h, gameObject);
            addedSelf = true;
        }
    }
    
}
