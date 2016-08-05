using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using System;

/// <summary>
/// A keyboard movement controller.
/// 
/// This class allows to control the movement of the player using the keyboard.
/// </summary>
public class JoystickMovementController : PlayerMovementController
{
    PlayerIndex playerIndex = 0;
    GamePadState state;
    GamePadState prevState;


    protected override void getMovement()
    {
        prevState = state;
        state = GamePad.GetState(playerIndex);
        /*
        if ((prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed)|| (prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Pressed))
        {
            player.turnLeft();
        }
        if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed)
        {
            player.turnRight();
        }
        if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed)
        {
            player.move();
        }
        */
        // AL presionar A
		if (state.Buttons.A == ButtonState.Pressed) {
			if(player.state == PlayerState.NO_STAFF){
				player.pickStaff();
				return;
			}
			if(player.state != PlayerState.STOPPED)
				return;
			if(upEvent())
				player.askAhead();
			else if(rightEvent())
				player.askRight();
			else if(leftEvent())
				player.askLeft();
			return;
		}
		// Al presionar B
		else if (state.Buttons.B == ButtonState.Pressed) {
			// Direccion de burjula
			// TODO : Relacionar con state del player
			player.askOrientation();
		}
		// Al presionar Y
		else if (state.Buttons.Y == ButtonState.Pressed) {
			// TODO : Tocar, si en tesoro obtener dependiendo de tesoro
		} 
		// Al presionar X
		else if (state.Buttons.X == ButtonState.Pressed) {
			// TODO : Reproducir ayuda cubo
		}
		// Moverse en direcciones
        else if (rightEvent())
        {
            player.turnRight();
        }
        else if (leftEvent())
        {
            player.turnLeft();
        }
        else if (upEvent())
        {
            player.move();
        }

        Vector2 pos = Game.GetInstance().player.getPosition();
        float distance = Room.GetInstance().getMinDistanceMonster(pos);
        if (distance<3.0f)
        {
            float coef = 0.1f + ((3.0f - distance) * 0.2f / 3.0f);
			float coef2 = (3.0f - distance)/ 3.0f;
            GamePad.SetVibration(playerIndex, coef, coef);
			if(!SoundManager.instance.isEfxPlaying())
				SoundManager.instance.PlaySingleWithVolume("breathing2", coef2);
        }
        else
        {
            GamePad.SetVibration(playerIndex, 0.0f, 0.0f);
        }

		float doorDistance = Room.GetInstance().getDoorDistance(pos);
		if (doorDistance<3.0f)
		{
			float coef = (3.0f - doorDistance)/ 3.0f;
			GamePad.SetVibration(playerIndex, coef, coef);
			if(!SoundManager.instance.isEfxPlaying())
				SoundManager.instance.PlaySingleWithVolume("shooting-star", coef);
		}
		else
		{
			GamePad.SetVibration(playerIndex, 0.0f, 0.0f);
		}

		float trapDistance = Room.GetInstance().getMinDistanceTrap(pos);
		if (trapDistance<3.0f)
		{
			float coef = (3.0f - trapDistance)/ 3.0f;
			GamePad.SetVibration(playerIndex, coef, coef);
			if(!SoundManager.instance.isEfxPlaying())
				SoundManager.instance.PlaySingleWithVolume("death-ray", coef);
		}
		else
		{
			GamePad.SetVibration(playerIndex, 0.0f, 0.0f);
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
}