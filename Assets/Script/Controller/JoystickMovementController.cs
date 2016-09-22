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

    protected override void tryToGetLoot() {
    	prevState = state;
        state = GamePad.GetState(playerIndex);
        
    	if (player.state == PlayerState.ON_TREASURE) {
        	if (state.Buttons.Y == ButtonState.Pressed) {
				if(upEvent()) {
					player.getTreasure(4);
				}
				else if(rightEvent()) {
					player.getTreasure(3);
				}
				else if(leftEvent()) {
					player.getTreasure(1);
				} 
				else if(downEvent()) {
					player.getTreasure(5);
				} 
				else if (topEvent()) {
					player.getTreasure(0);
				} 
				else if(bottomEvent()) {
					player.getTreasure(2);
				}
				return;
				}
        }
    }

    protected override void getMovement()
    {
        prevState = state;
        state = GamePad.GetState(playerIndex);

        if (player.state == PlayerState.ON_TREASURE) {
        	tryToGetLoot();
        	return;
        }
        // AL presionar A
		if (state.Buttons.A == ButtonState.Pressed) {

			if(player.state == PlayerState.NO_STAFF){
				player.pickStaff();
				return;
			}

			if(player.state != PlayerState.STOPPED) {
				return;
			}

			if(upEvent()) {
				player.askAhead();
			}
			else if(rightEvent()) {
				player.askRight();
			}
			else if(leftEvent()) {
				player.askLeft();
			} 
			else if(downEvent()) {
				player.askBehind();
			}
			else if (topEvent()) {
				player.askTop();
			} 
			else if(bottomEvent()) {
				player.askBottom();
			}
			return;
		}
		// Al presionar B
		else if (state.Buttons.B == ButtonState.Pressed) {
			// Direccion de brujula
			// TODO : Relacionar con state del player
			// if(player.state != PlayerState.STOPPED) {
			// 	return;
			// }
			player.askOrientation();
			return;
		}
		// Al presionar Y
		else if (state.Buttons.Y == ButtonState.Pressed) {
			if(player.state != PlayerState.STOPPED) {
				return;
			}
			// Toques normales
			if(upEvent()) {
				player.touchAhead();
			}
			else if(rightEvent()) {
				player.touchRight();
			}
			else if(leftEvent()) {
				player.touchLeft();
			} 
			else if(downEvent()) {
				player.touchBehind();
			} 
			else if (topEvent()) {
				player.touchOver();
			} 
			else if(bottomEvent()) {
				player.touchBottom();
			}
			return;
		} 
		// Al presionar X
		else if (state.Buttons.X == ButtonState.Pressed) {
			// Reproducir ayuda cubo
			player.wait(11.5f);
			SoundManager.instance.PlaySingle ("Cubo_laberinto");
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

	private bool topEvent(){
		return state.Buttons.LeftShoulder == ButtonState.Pressed;
	}

	private bool bottomEvent() {
		return state.Triggers.Left > 0 ;
	}
}