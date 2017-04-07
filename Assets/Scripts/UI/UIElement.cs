using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for elements of the map UI
/// Used for 
/// </summary>
public abstract class UIElement : MonoBehaviour {

    public abstract void onSelect();
    public abstract void onDeselect();
}
