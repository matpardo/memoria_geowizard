using UnityEngine;
using System.Collections;

public class SignalEntity : RoomEntity
{
	public string source;

	protected void Awake(){
		source = "correct";
	}

	public override void handleCollision(){
		SoundManager.instance.PlaySingle (source);
	}
	
}
