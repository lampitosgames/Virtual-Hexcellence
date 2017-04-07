using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class for handling each ability's UI and behavior.
public abstract class Ability : MonoBehaviour {
    //TO-DO: 
    /*
        * Add comments for displaying targeting
        * Add comments for the ability's effects
        * Add static method for determining what material to take from a MaterialEnum.
    */

    public abstract bool displayTargeting();
    public abstract void executeAbility();
}
