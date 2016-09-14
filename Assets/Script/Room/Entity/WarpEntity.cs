using UnityEngine;
using System.Collections;

public class WarpEntity : SignalEntity
{

	protected void Awake(){
		source  = "warp";
	}
	
	private void nextScene(){
		SceneLoader.GetInstance ().load("GeometricState");
	}
	
	public override void handleCollision(){
		base.handleCollision ();
		Game.GetInstance ().player.wait (3);
		Invoke ("nextScene", 4);
	}

	public override void ask ()
	{
		SoundManager.instance.PlaySingle ("energia-portal");
	}

	public override void ask () {
		SoundManager.instance.PlaySingle ("warp");
	}
}

