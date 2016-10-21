using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using System;

public class LoadJoystick : MonoBehaviour {

	GamePadState state;
	GamePadState prevState;
	bool main_menu;
	bool resume_game;
	PlayerIndex playerIndex = 0;
	MenuEvent sn;
	bool x_status;
	bool b_status;

	// Use this for initialization
	void Start () {
		main_menu = false;
		resume_game = false;
		sn = gameObject.GetComponent<MenuEvent>();
		prevState = GamePad.GetState(playerIndex);
		state = prevState;
		x_status = false;
		b_status = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		prevState = state;
        state = GamePad.GetState(playerIndex);

		if (state.Buttons.X == ButtonState.Pressed) {
			x_status = true;
			// SoundManager.instance.PlaySingle ("Horse-nay");
			return;
        } else {
        	if (x_status) {
        		x_status = false;
        		if (main_menu) {
        			main_menu = false;
        			sn.goMainMenu();
        		} else {
        			main_menu = true;
        			resume_game = false;
        		}
        		return;
        	}
        }

        if (state.Buttons.B == ButtonState.Pressed) {
			b_status = true;
			// SoundManager.instance.PlaySingle ("Horse-nay");
			return;
        } else {
        	if (b_status) {
        		b_status = false;
        		if (resume_game) {
        			resume_game = false;
        			sn.startLoadedGame();
        		} else {
        			resume_game = true;
        			main_menu = false;
        		}
        		return;
        	}
        }
	}
}
