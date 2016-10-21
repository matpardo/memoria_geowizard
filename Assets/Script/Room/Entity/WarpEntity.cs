using UnityEngine;
using System.Collections;

public class WarpEntity : SignalEntity
{
	private bool is_destroyable;
	private int riddle_number;

	protected void Awake(){
		source  = "warp";
	}
	
	private void nextScene(){
		SceneLoader.GetInstance ().load("GeometricState");
	}

	public override bool destroyable(){
		return is_destroyable;
	}
	
	public override void handleCollision(){
		// base.handleCollision ();
		// Game.GetInstance ().player.wait (3);
		// Invoke ("nextScene", 4);

		if (!is_destroyable) {
			is_destroyable = false;
			Game.GetInstance ().player.wait (1);
			generateRiddleQuestion();
			Game.GetInstance ().player.wait (4);
			SoundManager.instance.PlaySingle ("riddle");
		}
	}

	public override void ask ()
	{
		SoundManager.instance.PlaySingle ("energia-portal");
	}

	public override void touch () {
		SoundManager.instance.PlaySingle ("warp");
	}

	public void generateRiddleQuestion() {
		riddle_number = Random.Range(0,19); // A number between 0 and 4
		Game.GetInstance ().player.setRiddle(this);
	}

	public void makeDestroyable() {
		is_destroyable = true;
	}

	public void sayRiddle() {
		switch (riddle_number) {
			case 0:
				SoundManager.instance.PlaySingle ("riddle_rotacion_1");
				break;
			case 1:
				SoundManager.instance.PlaySingle ("riddle_rotacion_2");
				break;
			case 2:
				SoundManager.instance.PlaySingle ("riddle_rot_3");
				break;
			case 3:
				SoundManager.instance.PlaySingle ("riddle_rot_4");
				break;
			case 4:
				SoundManager.instance.PlaySingle ("riddle_rot_5");
				break;
			case 5:
				SoundManager.instance.PlaySingle ("riddle_traslacion_1");
				break;
			case 6:
				SoundManager.instance.PlaySingle ("riddle_traslacion_2");
				break;
			case 7:
				SoundManager.instance.PlaySingle ("riddle_trans_3");
				break;
			case 8:
				SoundManager.instance.PlaySingle ("riddle_trans_4");
				break;
			case 9:
				SoundManager.instance.PlaySingle ("riddle_trans_5");
				break;
			case 10:
				SoundManager.instance.PlaySingle ("riddle_trans_6");
				break;
			case 11:
				SoundManager.instance.PlaySingle ("riddle_ref_1");
				break;
			case 12:
				SoundManager.instance.PlaySingle ("riddle_ref_2");
				break;
			case 13:
				SoundManager.instance.PlaySingle ("riddle_ref_3");
				break;
			case 14:
				SoundManager.instance.PlaySingle ("riddle_ref_4");
				break;
			case 15:
				SoundManager.instance.PlaySingle ("riddle_ref_5");
				break;
			case 16:
				SoundManager.instance.PlaySingle ("riddle_rot_x");
				break;
			case 17:
				SoundManager.instance.PlaySingle ("riddle_trans_x");
				break;
			case 18:
				SoundManager.instance.PlaySingle ("riddle_ref_x");
				break;
			default:
				SoundManager.instance.PlaySingle ("Horse-nay");
				break;
		}
	}

	public bool isAnswerToRiddle(int answer) {
		bool correct = false;
		switch (riddle_number) {
			case 0:
				correct = (answer == 5);
				break;
			case 1:
				correct = (answer == 1);
				break;
			case 2:
				correct = (answer == 4);
				break;
			case 3:
				correct = (answer == 1);
				break;
			case 4:
				correct = (answer == 2);
				break;
			case 5:
				correct = (answer == 0);
				break;
			case 6:
				correct = (answer == 1);
				break;
			case 7:
				correct = (answer == 1);
				break;
			case 8:
				correct = (answer == 2);
				break;
			case 9:
				correct = (answer == 4);
				break;
			case 10:
				correct = (answer == 3);
				break;
			case 11:
				correct = (answer == 4);
				break;
			case 12:
				correct = (answer == 3);
				break;
			case 13:
				correct = (answer == 1);
				break;
			case 14:
				correct = (answer == 5);
				break;
			case 15:
				correct = (answer == 4);
				break;
			case 16:
				correct = (answer == 5);
				break;
			case 17:
				correct = (answer == 3);
				break;
			case 18:
				correct = (answer == 5);
				break;	
			default:
				return false;
				break;
		}

		if (correct) {
			SoundManager.instance.PlaySingle ("riddle_right");
		} else {
			SoundManager.instance.PlaySingle ("riddle_wrong");
		}

		return correct;
	}
}

