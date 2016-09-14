using UnityEngine;
using System.Collections;

public class TreasureEntity : SignalEntity
{
	protected void Awake(){
		source  = "item_pickup";
	}

	public override void handleCollision(){
		base.handleCollision ();
		Game.GetInstance ().player.wait (1.5f);
		Invoke ("generateTreasureRandomLootBoost", 1);

	}

	public override bool destroyable(){
		return true;
	}

	public override void ask ()
	{
		SoundManager.instance.PlaySingle ("tomar-tesoro");
	}

	public override void touch () {
		SoundManager.instance.PlaySingle ("item_pickup");
	}

	public void generateTreasureRandomLootBoost() {
		AbilityState[] listAbilityPool = new AbilityState[]{
			AbilityState.AGUA,
			AbilityState.TIERRA,
			AbilityState.FUEGO,
			AbilityState.VIENTO,
			AbilityState.NATURALEZA,
			AbilityState.ARCANO
		};
		int randomBoost = Random.Range(0,6); // A number between 0 and 5
		Game.GetInstance ().player.addBoost (listAbilityPool[randomBoost], 4);
		ApplicationData.addBoost (4, listAbilityPool[randomBoost]);

		switch (listAbilityPool [randomBoost]) {
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
}

