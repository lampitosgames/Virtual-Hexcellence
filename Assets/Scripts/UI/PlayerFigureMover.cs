namespace VRTK.Examples
{
	using UnityEngine;

	public class PlayerFigureMover : VRTK_InteractableObject
	{
		private LevelController levelController;
		public PlayerFigure playerFigure;


		/// <summary>
		/// Called when the player grabs the object with a VR controller
		/// </summary>
		/// <param name="usingObject">The controller that grabbed</param>
		/// <returns></returns>
		public override void StartUsing(GameObject usingObject)
		{
			base.StartUsing(usingObject);
			doMove ();
		}

		protected override void Update(){
			base.Update ();
		}
			
		public void doMove() {
			//Subtract the player position
			Vector3 scaledFigurePosition = new Vector3 (transform.position.x-transform.parent.position.x,transform.position.y-transform.parent.position.y,transform.position.z-transform.parent.position.z);
			//Scale up the position from the miniature to full scale and reset the y axis
			scaledFigurePosition = scaledFigurePosition * 50;
			//scaledFigurePosition.y = scaledFigurePosition.y - 50;
			//Convert the position into hex coordinates and move the player
			int[] figurePositionHex = HexConst.CoordToHexIndex (scaledFigurePosition);
			if (levelController.levelGrid.GetHex(figurePositionHex[0], figurePositionHex[1], figurePositionHex[2]) != null) {
				print ("There is a Hex Here");
				Vector3 newPosition = HexConst.HexToWorldCoord(figurePositionHex[0], figurePositionHex[1], figurePositionHex[2]);
				GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Player> ().vrMove (newPosition);
				playerFigure.transform.localPosition = this.transform.localPosition;
				playerFigure.StopUsing (playerFigure.controller);
				Destroy (this.gameObject);
			}else{
				Debug.LogError ("OH NO!! THERE IS NO HEX WHERE THE PLAYER FIGURE IS!!" + "q: "+figurePositionHex[0]+ "r: "+figurePositionHex[1]+ "h: "+figurePositionHex[2]);
			}
		}

		protected void Start()
		{
			levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
		}

	}
}