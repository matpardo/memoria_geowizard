using UnityEngine;
using System.Collections;

public class TrapEntity : SignalEntity
{
	protected void Awake(){
		source  = "player_falling_trap";
	}

	public void nextScene(){
		SceneLoader.GetInstance ().cleanLoad ("HallState");
	}

	public override void handleCollision(){
		base.handleCollision ();
		Game.GetInstance().player.disableMovement();
		Invoke ("nextScene", 5);
	}

	public override void ask ()
	{
		SoundManager.instance.PlaySingle ("advertencia-trampa");
	}

	public override void touch () {
		SoundManager.instance.PlaySingle ("crashTrap");
	}
}

