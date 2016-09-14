using UnityEngine;
using System;
using System.Collections.Generic;

public class MonsterEntity : SignalEntity
{

	private int hp = 10;
	private int minDamage = 0;
	private int maxDamage = 11;
	private double damageMultiplier = 1.0;
	private double attackThreshold = 0.7;
	private double chanceToHit = 0.8;
	private double[] multipliers = new double[]{0.5, 1, 2};
	private System.Random random = new System.Random ();
	private Dictionary<AbilityState, double> stats;
	private Dictionary<EnemyAction, Func<System.Object[], System.Object[]>> attackPool;

	protected void Awake(){
		source  = "Monster";
	}

	public override void handleCollision(){
		SceneLoader loader = SceneLoader.GetInstance();
		Game.GetInstance ().enemy = this;
		initializeStats();
		initializeAttacks();
		SoundManager.instance.PlayMusic("Mistake the Getaway");
		loader.load("BattleState");
	}

	private void initializeStats(){
		stats = new Dictionary<AbilityState, double>();
		int i = 0;

		foreach(AbilityState AS in System.Enum.GetValues(typeof(AbilityState))){
			//int rand = UnityEngine.Random.Range(0, 3);
			//stats.Add(AS, multipliers[rand]);
			stats.Add (AS, Constants.baseStats[i]);
			i++;
		}
	}

	/*FUNCIONES QUE DEFINEN ATAQUES*/
	private System.Object[] basicAttack(System.Object[] args){
		string sfx = "";
		int baseDamage = UnityEngine.Random.Range(minDamage, maxDamage);
		int finalDamage = (int)Math.Ceiling(baseDamage * damageMultiplier);
		double damageRatio = ((double)finalDamage / (maxDamage - 1));

		double hitRoll = new System.Random().NextDouble();

		if(hitRoll <= chanceToHit){

			if(damageRatio == 0)
				sfx = "no_damage";

			if(0 < damageRatio && damageRatio <= attackThreshold)
				sfx = "light_hit";

			if(attackThreshold < damageRatio)
				sfx = "heavy_hit";
		}
		else{
			finalDamage = 0;
			sfx = "miss";
		}

		return new System.Object[]{finalDamage, sfx};
	}

	private System.Object[] confusePlayer(System.Object[] args){
		Array values = Enum.GetValues(typeof(Rotation));
		//System.Random random = new System.Random();
		Rotation rotation = (Rotation)values.GetValue(random.Next(values.Length));

		Debug.Log(rotation.ToString());

		return new System.Object[]{rotation};
	}

	private System.Object[] powerUp(System.Object[] args){
		this.damageMultiplier += 0.25;

		return new System.Object[]{};
	}

	private System.Object[] raiseDefense(System.Object[] args){
		AbilityState abilityToBuff = (AbilityState)args[0];
		AbilityState abilityToDebuff = abilityToBuff;

		while(abilityToDebuff.Equals(abilityToBuff) || stats[abilityToDebuff] != multipliers[0]){
			Array values = Enum.GetValues(typeof(AbilityState));
			//System.Random random = new System.Random();
			abilityToDebuff = (AbilityState)values.GetValue(random.Next(values.Length));
		}

		stats[abilityToDebuff] = stats[abilityToBuff];
		stats[abilityToBuff] = multipliers[0];

		Debug.Log ("turno");
		foreach(KeyValuePair<AbilityState, double> stat in stats){
			Debug.Log(stat.Key.ToString() + ":" + stat.Value.ToString());
		}

		return new System.Object[]{};
	}
	/*FUNCIONES QUE DEFINEN ATAQUES*/

	/*DICCIONARIO DE ATAQUES*/
	private void initializeAttacks(){
		attackPool = new Dictionary<EnemyAction, Func<System.Object[], System.Object[]>>();

		attackPool.Add(EnemyAction.BASIC_ATTACK, basicAttack);
		attackPool.Add(EnemyAction.CONFUSE_PLAYER, confusePlayer);
		attackPool.Add(EnemyAction.POWER_UP, powerUp);
		attackPool.Add(EnemyAction.RAISE_DEFENSE, raiseDefense);
	}

	/*FUNCION QUE DECIDE LA ACCION A SEGUIR
	 * POR AHORA ES ALEATORIA*/
	public EnemyReturn decide(System.Object[] args){

		bool flag = true;
		EnemyAction action = EnemyAction.BASIC_ATTACK;

		while (flag) {
			Array enum_values = Enum.GetValues (typeof(EnemyAction));
			//System.Random random = new System.Random ();
			action = (EnemyAction)enum_values.GetValue (random.Next (enum_values.Length));

			if(!action.Equals(EnemyAction.RAISE_DEFENSE) || stats[(AbilityState)args[0]] != multipliers[0]){
				flag = false;
			}
		}

		System.Object[] values = attackPool[action](args);

		return new EnemyReturn(action, values);
	}

    public void setHP(int hp){
        this.hp = hp;
    }

	public int getHP(){
		return this.hp;
	}

	public void removeHP(int dmg){
		this.hp -= dmg;
        if (this.hp < 0){
            this.hp = 0;
        }
	}

	public double getMultiplier(AbilityState ability){
		return stats[ability];
	}

	public override bool destroyable(){
		return true;
	}

	public override void ask(){
		SoundManager.instance.PlaySingle ("un-enemigo");
	}

	public override void touch() {
		SoundManager.instance.PlaySingle ("Monster");
	}
}

