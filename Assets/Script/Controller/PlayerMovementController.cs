using UnityEngine;
using System.Collections;

using XInputDotNetPure;

/// <summary>
/// A player movement controller.
/// 
/// This class allows to control the movement of the player.
/// </summary>
public abstract class PlayerMovementController : PlayerController
{
    
    public override void Update ()
	{
        getMovement();
	}

	protected abstract void getMovement();

	protected abstract void tryToGetLoot();
}

