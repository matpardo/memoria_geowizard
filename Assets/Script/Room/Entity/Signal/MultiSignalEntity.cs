using UnityEngine;
using System.Collections;

public class MultiSignalEntity : RoomEntity
{

	private float delta = 0.6f;
	protected float waitTime = 2;

	public override void handleCollision(){
		makeSounds ();
		Game.GetInstance ().player.wait (waitTime);
	}

	public override void ask ()
	{

	}

	public override void touch() {
		
	}
	
	private void makeSounds(){
		Player p = Game.GetInstance ().player;
		bool left = Room.GetInstance ().walkableCell (p.leftPosition ());
		bool right = Room.GetInstance ().walkableCell (p.rightPosition ());
		bool front = Room.GetInstance ().walkableCell (p.frontPosition ());
		if (left && right && front)
			lrfSound ();
		else if (left && right)
			lrSound ();
		else if (left && front)
			lfSound ();
		else if (front && right)
			rfSound ();
	}
	
	protected void lfSound(){
		Invoke ("leftSound", 0);
		Invoke ("bothSound", delta);
	}
	protected void rfSound(){
		Invoke ("rightSound", 0);
		Invoke ("bothSound", delta);
	}
	protected void lrfSound(){
		Invoke ("leftSound", 0);
		Invoke ("rightSound", delta);
		Invoke ("bothSound", 2*delta);
	}
	protected void lrSound(){
		Invoke ("leftSound", 0);
		Invoke ("rightSound", delta);
	}
	
	private void leftSound(){SoundManager.instance.PlayDirectionalSingle ("pew-left", -1f);}
	private void rightSound(){SoundManager.instance.PlayDirectionalSingle ("pew-right", 1f);}
	private void bothSound(){SoundManager.instance.PlayDirectionalSingle ("pew", 0f);}
}

