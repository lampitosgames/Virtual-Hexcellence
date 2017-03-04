using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public int q, r, h;
	LevelController levelController;


	// Use this for initialization
	void Start () {
		int[] hexCoords = HexConst.CoordToHexIndex(gameObject.transform.position);
		q = hexCoords[0];
		r = hexCoords[1];
		h = hexCoords[2];
		levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
	}

	// Update is called once per frame
	void Update () {
		//Up
		if (Input.GetKeyDown("[8]")) {
			MoveDir(8);
		}
		//Down
		else if (Input.GetKeyDown("[2]")) {
			MoveDir(5);
		}
		//UpLeft
		else if (Input.GetKeyDown("[7]")) {
			MoveDir(7);
		}
		//UpRight
		else if (Input.GetKeyDown("[9]")) {
			MoveDir(9);
		}
		//DownLeft
		else if (Input.GetKeyDown("[1]")) {
			MoveDir(1);
		}
		//DownRight
		else if (Input.GetKeyDown("[3]")) {
			MoveDir(3);
		}
	}

	void MoveDir(int dir) {
		switch (dir)
		{
		case 8:
			if (levelController.levelGrid.GetHex(this.q, this.r, this.h) != null) {
				this.q += 1;
				Vector3 newPosition = HexConst.HexToWorldCoord(this.q, this.r, this.h);
				gameObject.transform.position = newPosition;
			}

			break;
		case 5:
			if (levelController.levelGrid.GetHex(this.q, this.r, this.h) != null) {
				this.q -= 1;
				Vector3 newPosition = HexConst.HexToWorldCoord(this.q, this.r, this.h);
				gameObject.transform.position = newPosition;

			}

			break;
		case 7:
			if (levelController.levelGrid.GetHex(this.q, this.r, this.h) != null) {
				this.q += 1;
				this.r -= 1;
				Vector3 newPosition = HexConst.HexToWorldCoord(this.q, this.r, this.h);
				gameObject.transform.position = newPosition;
			}
			break;
		case 9:
			if (levelController.levelGrid.GetHex(this.q, this.r, this.h) != null) {
				this.q += 0;
				this.r += 1;
				Vector3 newPosition = HexConst.HexToWorldCoord(this.q, this.r, this.h);
				gameObject.transform.position = newPosition;
			}

			break;
		case 1:
			if (levelController.levelGrid.GetHex(this.q, this.r, this.h) != null) {
				this.q += 0;
				this.r -= 1;
				Vector3 newPosition = HexConst.HexToWorldCoord(this.q, this.r, this.h);
				gameObject.transform.position = newPosition;
			}

			break;
		case 3:
			if (levelController.levelGrid.GetHex(this.q, this.r, this.h) != null) {
				this.q -= 1;
				this.r += 1;
				Vector3 newPosition = HexConst.HexToWorldCoord(this.q, this.r, this.h);
				gameObject.transform.position = newPosition;
			}

			break;
		default:
			break;
		}
		print("Moving");
	}

}
