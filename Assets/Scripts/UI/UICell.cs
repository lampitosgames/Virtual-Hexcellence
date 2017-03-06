using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICell : HexCell {

	public GameObject targetCell;

	//store reference to the cell this element is representing
	public void setTargetCell(GameObject cell){
		targetCell = cell;
	}


	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="q">column</param>
	/// <param name="r">row</param>
	/// <param name="h">height</param>
	public UICell(int q, int r, int h) : base(q, r, h) {}



}
