using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class MagicBlock : MonoBehaviour {

	public bool active;
	public int x_answer;
	public int y_answer;

	private int max_x = 11;
	private int max_z = 5;

	private bool waiting_release;

	public GameObject block;

	PlayerIndex playerIndex = 0;
	GamePadState state;
	GamePadState prevState;

	// Use this for initialization
	void Start () {
		waiting_release = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		prevState = state;
        state = GamePad.GetState(playerIndex);

        if (leftEvent()) {
        	 if (!waiting_release) {
        	 	moveLeft();
        	 }
        	 waiting_release = true;
        } else if (rightEvent()) {
        	 if (!waiting_release) {
        	 	moveRight();
        	 }
        	 waiting_release = true;
        } else if (upEvent()) {
        	 if (!waiting_release) {
        	 	moveUp();
        	 }
        	 waiting_release = true;
        } else if (downEvent()) {
        	 if (!waiting_release) {
        	 	moveDown();
        	 }
        	 waiting_release = true;
        }
       	else {
        	waiting_release = false;
        }
	}

	private void moveLeft() {
		float new_x = transform.position.x - 1;
		if (new_x < (-1 * max_x)) {
			new_x = (-1 * max_x);
		}
		block.transform.position = new Vector3(new_x, transform.position.y, transform.position.z);
	}

	private void moveRight() {
		float new_x = transform.position.x + 1;
		if (new_x > (max_x)) {
			new_x = (max_x);
		}
		block.transform.position = new Vector3(new_x, transform.position.y, transform.position.z);
	}

	private void moveUp() {
		float new_z = transform.position.z + 1;
		if (new_z > (max_z)) {
			new_z = (max_z);
		}
		block.transform.position = new Vector3(transform.position.x, transform.position.y, new_z);
	}

	private void moveDown() {
		float new_z = transform.position.z - 1;
		if (new_z < (-1 * max_z)) {
			new_z = (-1 * max_z);
		}
		block.transform.position = new Vector3(transform.position.x, transform.position.y, new_z);
	}

	private bool leftEvent(){
		float x = state.ThumbSticks.Left.X;
		float y = state.ThumbSticks.Left.Y;
		return  x < 0 && Mathf.Abs(x)>Mathf.Abs(y);
	}
	
	private bool rightEvent(){
		float x = state.ThumbSticks.Left.X;
		float y = state.ThumbSticks.Left.Y;
		return  x > 0 && Mathf.Abs(x)>Mathf.Abs(y);
	}
	
	private bool upEvent(){
		float x = state.ThumbSticks.Left.X;
		float y = state.ThumbSticks.Left.Y;
		return  y > 0 && Mathf.Abs(y)>Mathf.Abs(x);
	}
	
	private bool downEvent(){
		float x = state.ThumbSticks.Left.X;
		float y = state.ThumbSticks.Left.Y;
		return  y < 0 && Mathf.Abs(y)>Mathf.Abs(x);
	}

	private bool topEvent(){
		return state.Buttons.LeftShoulder == ButtonState.Pressed;
	}

	private bool bottomEvent() {
		return state.Triggers.Left > 0 ;
	}
}
