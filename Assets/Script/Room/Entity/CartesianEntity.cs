using UnityEngine;
using System.Collections;

public class CartesianEntity : SignalEntity
{

	protected void Awake(){
		source  = "warp";
	}
	
	private void nextScene(){
		SceneLoader.GetInstance ().load("CartesianState");
	}

	public override bool destroyable(){
		return true;
	}
	
	public override void handleCollision(){
		base.handleCollision ();
		Game.GetInstance ().player.wait (3);
		Invoke ("nextScene", 2);
	}

	public override void ask ()
	{
		SoundManager.instance.PlaySingle ("energia-portal");
	}

	public override void touch () {
		SoundManager.instance.PlaySingle ("warp");
	}
}

