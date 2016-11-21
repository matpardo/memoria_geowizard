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
	private bool flying;

	public GameObject block;
	public MagicBlock next_block;
	public Material new_material;

	PlayerIndex playerIndex = 0;
	GamePadState state;
	GamePadState prevState;

	// Use this for initialization
	void Start () {
		waiting_release = false;
		flying = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		if (active) {
			prevState = state;
        	state = GamePad.GetState(playerIndex);

	        if (leftEvent()) {
	        	 if (flying) {
	        	 	if (!waiting_release) {
	        	 		moveLeft();
	        	 	}
	        	 	waiting_release = true;
	        	 } else {
	        	 	GamePad.SetVibration((PlayerIndex)0, 0.2f, 0.2f);
	        	 }
	        } else if (rightEvent()) {
	        	 if (flying) {
	        	 	if (!waiting_release) {
	        	 		moveRight();
	        	 	}
	        	 	waiting_release = true;
	        	 } else {
	        	 	GamePad.SetVibration((PlayerIndex)0, 0.2f, 0.2f);
	        	 }
	        } else if (upEvent()) {
	        	 if (flying) {
	        	 	if (!waiting_release) {
	        	 		moveUp();
	        	 	}
	        	 	waiting_release = true;
	        	 } else {
	        	 	GamePad.SetVibration((PlayerIndex)0, 0.2f, 0.2f);
	        	 }
	        } else if (downEvent()) {
	        	 if (flying) {
	        	 	if (!waiting_release) {
	        	 		moveDown();
	        	 	}
	        	 	waiting_release = true;
	        	 } else {
	        	 	GamePad.SetVibration((PlayerIndex)0, 0.2f, 0.2f);
	        	 }
	        } else if (bottomEvent()) {
	        	 if (flying) {
	        	 	if (!waiting_release) {
	        	 		moveBottom();
	        	 	}
	        	 	waiting_release = true;
	        	 } else {
	        	 	GamePad.SetVibration((PlayerIndex)0, 0.2f, 0.2f);
	        	 }
	        } else if (topEvent()) {
	        	 if (!flying) {
	        	 	if (!waiting_release) {
	        	 		moveTop();
	        	 	}
	        	 	waiting_release = true;
	        	 } else {
	        	 	GamePad.SetVibration((PlayerIndex)0, 0.2f, 0.2f);
	        	 }
	        } else {
	        	GamePad.SetVibration((PlayerIndex)0, 0.0f, 0.0f);
	        	waiting_release = false;
	        }
		}
	}

	private void moveLeft() {
		float new_x = transform.position.x - 1;
		if (new_x < (-1 * max_x)) {
			new_x = (-1 * max_x);
			GamePad.SetVibration((PlayerIndex)0, 0.2f, 0.2f);
		}
		block.transform.position = new Vector3(new_x, transform.position.y, transform.position.z);
	}

	private void moveRight() {
		float new_x = transform.position.x + 1;
		if (new_x > (max_x)) {
			new_x = (max_x);
			GamePad.SetVibration((PlayerIndex)0, 0.2f, 0.2f);
		}
		block.transform.position = new Vector3(new_x, transform.position.y, transform.position.z);
	}

	private void moveUp() {
		float new_z = transform.position.z + 1;
		if (new_z > (max_z)) {
			new_z = (max_z);
			GamePad.SetVibration((PlayerIndex)0, 0.2f, 0.2f);
		}
		block.transform.position = new Vector3(transform.position.x, transform.position.y, new_z);
	}

	private void moveDown() {
		float new_z = transform.position.z - 1;
		if (new_z < (-1 * max_z)) {
			new_z = (-1 * max_z);
			GamePad.SetVibration((PlayerIndex)0, 0.2f, 0.2f);
		}
		block.transform.position = new Vector3(transform.position.x, transform.position.y, new_z);
	}

	private void moveBottom() {
		block.transform.position = new Vector3(transform.position.x, 0,transform.position.z);
		flying = false;
		check_is_correct();
	}

	private void moveTop() {
		block.transform.position = new Vector3(transform.position.x, 1,transform.position.z);
		flying = true;
	}

	private void check_is_correct() {
		if (transform.position.x == x_answer && transform.position.z == y_answer) {
			deactivate();
			block.GetComponent<Renderer>().material = new_material;
			if (next_block) {
				next_block.activate();
			}
		}
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

	public void activate() {
		this.active = true;
		this.waiting_release = true;
	}

	public void deactivate() {
		this.active = false;
	}
}
