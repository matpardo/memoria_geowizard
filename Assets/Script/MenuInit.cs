using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// using XInputDotNetPure;

public class MenuInit : MonoBehaviour {

	// GamePadState state;
	// GamePadState prevState;

	// Use this for initialization
	void Start () {
		Button button = GameObject.Find ("LoadButton").GetComponent<Button>();
		if(!ApplicationData.existSavedGame())
			button.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
		// prevState = state;
  //       state = GamePad.GetState(0);
  //       if (state.Buttons.X == ButtonState.Pressed) {
  //       	// MenuEvent.startNewGame();
  //       	SoundManager.instance.PlaySingle ("Horse-nay");
  //       } else if (state.Buttons.B == ButtonState.Pressed) {
  //       	// MenuEvent.goLoadMenu();
  //       	SoundManager.instance.PlaySingle ("Horse-nay");
  //       }

	}
}
