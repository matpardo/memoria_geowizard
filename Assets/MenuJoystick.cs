using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using System;

public class MenuJoystick : MonoBehaviour {

	GamePadState state;
	GamePadState prevState;
	bool new_game;
	bool resume_game;
	PlayerIndex playerIndex = 0;
	MenuEvent sn;

	// Use this for initialization
	void Start () {
		new_game = false;
		resume_game = false;
		sn = gameObject.GetComponent<MenuEvent>();
		prevState = GamePad.GetState(playerIndex);
		state = prevState;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		prevState = state;
        state = GamePad.GetState(playerIndex);

		if (state.Buttons.X == ButtonState.Pressed) {
			sn.startNewGame();
			// SoundManager.instance.PlaySingle ("Horse-nay");
			return;
        }

        if (state.Buttons.B == ButtonState.Pressed) {
			sn.goLoadMenu();
			// SoundManager.instance.PlaySingle ("Horse-nay");
			return;
        }
	}
}
