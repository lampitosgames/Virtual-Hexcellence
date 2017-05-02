using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Former base class for objectives, now a subclass of Objective.
/// Other classes can inherit this to implement different objective functionality
/// </summary>
public class Goal : Objective
{
    /// <summary>
    /// Unity's update function
    /// </summary>
    void Update() {
        //Can't add self in the start() function because it is creation order dependant on hex cells
        if (!addedSelf) {
            //Save the grid game object
            levelController = GameObject.Find("LevelController").GetComponent("LevelController") as LevelController;
            //Tell the level controller to initialize this hex cell
            levelController.AddGoal(q, r, h, gameObject);
            addedSelf = true;
        }
    }

    public virtual void Accomplished() {
        levelController[q, r, h].hasGoal = false;
        gameObject.SetActive(false);

    }
}
