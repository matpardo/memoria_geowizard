using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// A keyboard movement controller.
/// 
/// This class allows to control the movement of the player using the keyboard.
/// </summary>
public class KeyboardMovementController : PlayerMovementController


{

	protected override void tryToGetLoot() {
		return;
	}

    protected override void getMovement()
    {
    	return;
		if (Input.GetKey (KeyCode.Space)) {
			if (player.state == PlayerState.NO_STAFF) {
				player.pickStaff ();
				return;
			}
			if (player.state != PlayerState.STOPPED)
				return;
			if (upEvent ())
				player.askAhead ();
			else if (rightEvent ())
				player.askRight ();
			else if (leftEvent ())
				player.askLeft ();
		} else if (leftEvent ()) {
			player.turnLeft ();
		} else if (rightEvent ()) {
			player.turnRight ();
		} else if (upEvent ()) {
			player.move ();
		} else if (Input.GetKeyDown (KeyCode.Escape)) {
			ApplicationData.setLastLevel (1 +ApplicationData.getLastLevel () % 4);
			SceneLoader.GetInstance().cleanLoad("HallState");
		}
    }

	private bool leftEvent(){
		return Input.GetKey(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.LeftArrow);
	}

	private bool rightEvent(){
		return Input.GetKey (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.RightArrow);
	}

	private bool upEvent(){
		return Input.GetKey (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.UpArrow);
	}
}

