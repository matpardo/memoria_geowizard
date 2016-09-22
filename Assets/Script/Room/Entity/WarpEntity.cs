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
		riddle_number = Random.Range(0,4); // A number between 0 and 4
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
				SoundManager.instance.PlaySingle ("riddle_traslacion_1");
				break;
			case 3:
				SoundManager.instance.PlaySingle ("riddle_traslacion_2");
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
				correct = (answer == 0);
				break;
			case 3:
				correct = (answer == 1);
				break;
			default:
				return false;
				break;
		}

		if (correct) {
			SoundManager.instance.PlaySingle ("correct");
		} else {
			SoundManager.instance.PlaySingle ("error");
		}

		return correct;
	}
}

