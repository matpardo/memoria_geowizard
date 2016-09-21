using UnityEngine;
using System.Collections;

public class GeometricEntity : RoomEntity
{

	public override void handleCollision(){
		Debug.Log("geometric collision");
	}

	public override bool destroyable(){
		return true;
	}

	public override void ask ()
	{
		Debug.Log("Cuarto geometrico");
		SoundManager.instance.PlaySingle ("Horse-nay");
	}

	public override void touch()
    {
        SoundManager.instance.PlaySingle ("Horse-nay");
        return;
    }
}

