using UnityEngine;
using System.Collections;

public class TreasureEntity : SignalEntity
{
	private int loot_position;
	private bool is_destroyable;

	protected void Awake(){
		source  = "item_pickup";
	}

	public override void handleCollision(){
		// base.handleCollision ();
		// Game.GetInstance ().player.wait (1.5f);
		// Invoke ("generateTreasureRandomLootBoost", 1);
		if (!is_destroyable) {
			is_destroyable = false;
			Game.GetInstance ().player.wait (1.0f);
			generateTreasureLoot();
			soundLoot();
		}
	}

	public override bool destroyable(){
		return is_destroyable;
	}

	public override void ask ()
	{
		SoundManager.instance.PlaySingle ("tomar-tesoro");
	}

	public override void touch () {
		SoundManager.instance.PlaySingle ("item_pickup");
	}

	// public void generateTreasureRandomLootBoost() {
	// 	AbilityState[] listAbilityPool = new AbilityState[]{
	// 		AbilityState.AGUA,
	// 		AbilityState.TIERRA,
	// 		AbilityState.FUEGO,
	// 		AbilityState.VIENTO,
	// 		AbilityState.NATURALEZA,
	// 		AbilityState.ARCANO
	// 	};
	// 	int randomBoost = Random.Range(0,6); // A number between 0 and 5
	// 	Game.GetInstance ().player.addBoost (listAbilityPool[randomBoost], 4);
	// 	ApplicationData.addBoost (4, listAbilityPool[randomBoost]);

	// 	switch (listAbilityPool [randomBoost]) {
	// 	case AbilityState.AGUA:
	// 		SoundManager.instance.PlaySingle ("tesoro-agua");
	// 		break;
	// 	case AbilityState.TIERRA:
	// 		SoundManager.instance.PlaySingle ("tesoro-tierra");
	// 		break;
	// 	case AbilityState.FUEGO:
	// 		SoundManager.instance.PlaySingle ("tesoro-fuego");
	// 		break;
	// 	case AbilityState.VIENTO:
	// 		SoundManager.instance.PlaySingle ("tesoro-viento");
	// 		break;
	// 	case AbilityState.NATURALEZA:
	// 		SoundManager.instance.PlaySingle ("tesoro-naturaleza");
	// 		break;
	// 	case AbilityState.ARCANO:
	// 		SoundManager.instance.PlaySingle ("tesoro-arcano");
	// 		break;
	// 	default:
	// 		break;
	// 	}
	// }

	public void generateTreasureLoot(){
		loot_position = Random.Range(0,6); // A number between 0 and 5
		Game.GetInstance ().player.setTreasure(this);
	}

	public bool isPositionTreasure(int position) {
		if (position == loot_position) {
			return true;
		}
		soundLoot();
		return false;
	}

	public void getLoot() {
		Game.GetInstance ().player.wait (1.5f);

		AbilityState[] listAbilityPool = new AbilityState[]{
			AbilityState.ARCANO,
			AbilityState.VIENTO,
			AbilityState.NATURALEZA,
			AbilityState.AGUA,
			AbilityState.TIERRA,
			AbilityState.FUEGO
		};

		Game.GetInstance ().player.addBoost (listAbilityPool[loot_position], 4);
		ApplicationData.addBoost (4, listAbilityPool[loot_position]);

		switch (listAbilityPool [loot_position]) {
		case AbilityState.AGUA:
			SoundManager.instance.PlaySingle ("tesoro-agua");
			break;
		case AbilityState.TIERRA:
			SoundManager.instance.PlaySingle ("tesoro-tierra");
			break;
		case AbilityState.FUEGO:
			SoundManager.instance.PlaySingle ("tesoro-fuego");
			break;
		case AbilityState.VIENTO:
			SoundManager.instance.PlaySingle ("tesoro-viento");
			break;
		case AbilityState.NATURALEZA:
			SoundManager.instance.PlaySingle ("tesoro-naturaleza");
			break;
		case AbilityState.ARCANO:
			SoundManager.instance.PlaySingle ("tesoro-arcano");
			break;
		default:
			break;
		}
	}

	public void soundLoot() {
		switch (loot_position) {
			case 0:
				SoundManager.instance.PlaySingle ("arcane_select");
				break;
			case 1:
				SoundManager.instance.PlaySingle ("wind_select");
				break;
			case 2:
				SoundManager.instance.PlaySingle ("nature_select");
				break;
			case 3:
				SoundManager.instance.PlaySingle ("water_select");
				break;
			case 4:
				SoundManager.instance.PlaySingle ("earth_select");
				break;
			case 5:
				SoundManager.instance.PlaySingle ("fire_select");
				break;
			default:
				// SoundManager.instance.PlaySingle ("Horse-nay");
				break;
		}
	}

	public void makeDestroyable() {
		is_destroyable = true;
	}
}

