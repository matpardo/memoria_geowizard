using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using XInputDotNetPure;
using System;

public class Battle : MonoBehaviour {

	private int lastSelectedFace;

	private bool wasDeathSfxPlayed;
	private bool wasActionSfxPlayed;
	private bool wasVictoryMusicPlayed;
	private bool extraPlayerSoundsNeeded;
	private bool wasEnemySfxPlayed;
	private bool wasWinMsgPlayed;

	private string atkSuffix;
	private string enemyAtkSfx;
	private string enemyAtkSpeech;

	private TurnPhase currentPhase;
	private TurnPhase enemyPhase;
	private Rotation currentRotation;
	
	private BattleState currentState;
    private AbilityState currentAbility;
	private AbilityState[] abilities;

	private EnemyReturn ret;

	private MonsterEntity enemy;
    public double enemyMaxHp;
	private Player player;
	private SoundMap soundMap;
	private SceneLoader loader;

    public Text winText;
    public Text hpText;
    public Text enemyHpText;
    public Text abilityText;

    PlayerIndex playerIndex = 0;
    GamePadState state;
    GamePadState prevState;

    private int[] secuencia;
    private int contador_secuencia;
    private int element_numbers;

    void Start(){

		lastSelectedFace = 0;

		initiar_secuencias();

		wasDeathSfxPlayed = false;
		wasActionSfxPlayed = false;
		wasVictoryMusicPlayed = false;
		extraPlayerSoundsNeeded = false;
		wasWinMsgPlayed = false;

		atkSuffix = "";
		enemyAtkSfx = "";
		enemyAtkSpeech = "";

		currentPhase = TurnPhase.BEGINNING;
		enemyPhase = TurnPhase.BEGINNING;

        player = Game.GetInstance ().player;
		enemy = Game.GetInstance ().enemy;
        enemyMaxHp = enemy.getHP();
        GamePad.SetVibration(playerIndex, 0.0f, 0.0f); //hardcode

        currentState = BattleState.START;
		abilities = player.CurrentAbilityStates;
		currentAbility = abilities[0];
		ret = new EnemyReturn(EnemyAction.BASIC_ATTACK, new object[]{0, "miss"});

		soundMap = new SoundMap();
		loader = SceneLoader.GetInstance();
		SoundManager.instance.PlaySingle("Monster");

        if (ApplicationData.getLastLevel() == 2)
        {
            SoundManager.instance.PlaySingle("instrucciones_batalla");

        }
    }
	
	void Update(){

        prevState = state;
        state = GamePad.GetState(playerIndex);

        hpText.text = "HP: " + player.getHP().ToString();
        enemyHpText.text = "HP: " + enemy.getHP().ToString();
        abilityText.text = "Habilidad: " + currentAbility.ToString();

        switch (currentState) {

			case(BattleState.START):
				wasDeathSfxPlayed = false;
				wasVictoryMusicPlayed = false;
				currentState = BattleState.PLAYER_TURN;
				break;

			case(BattleState.PLAYER_TURN):
				playerTurn();
				break;

			case(BattleState.ENEMY_TURN):
				enemyTurn();
				break;

			case(BattleState.VICTORY):
			    Room.GetInstance().removeMonster(enemy.getPosition());
                Game.GetInstance ().enemy = null;
                
                winText.text = "¡Ganaste!";
				player.CurrentAbilityStates = abilities;

				if(!wasDeathSfxPlayed){
					SoundManager.instance.PlaySingle("monster_death");
					wasDeathSfxPlayed = true;
				}
				
				if(!wasVictoryMusicPlayed && !SoundManager.instance.isEfxPlaying()){
					SoundManager.instance.StopMusic();
					SoundManager.instance.PlaySingle("battle_victory_music");
					wasVictoryMusicPlayed = true;
				}

				if(!wasWinMsgPlayed && !SoundManager.instance.isEfxPlaying()){
					SoundManager.instance.StopMusic();
					SoundManager.instance.PlaySingle("msg-win-batalla");
					wasWinMsgPlayed = true;
				}
				
				if(!SoundManager.instance.isEfxPlaying() && wasWinMsgPlayed){
					currentState = BattleState.END;
					loader.load(loader.persistentScenes[0]);
					//SoundManager.instance.PlaySingle("cuarto-principal");
					SoundManager.instance.PlayMusic("Hidden Agenda");
				}
				break;

			case(BattleState.DEFEAT):
	            winText.text = "Has perdido";
	            currentState = BattleState.END;
				loader.cleanLoad("HallState");
				break;
		}
		
	}


    public int calculateDamage(){

        int baseDamage, finalDamage;
		double multiplier;
		int actual_element_number;
		switch (currentAbility) {
			case AbilityState.AGUA:
				actual_element_number = 3;
				break;
			case AbilityState.TIERRA:
				actual_element_number = 4;
				break;
			case AbilityState.FUEGO:
				actual_element_number = 5;
				break;
			case AbilityState.VIENTO:
				actual_element_number = 1;
				break;
			case AbilityState.NATURALEZA:
				actual_element_number = 2;
				break;
			case AbilityState.ARCANO:
				actual_element_number = 0;
				break;
			default:
				actual_element_number = 6;
				break;
		}

		baseDamage = player.getBaseDamage(currentAbility);

		multiplier = enemy.getMultiplier(currentAbility);

		if (!perform_attack(actual_element_number)){
			return -1;
		}

		finalDamage = (int)System.Math.Floor(baseDamage * multiplier);

		if (finalDamage <= 0) {
			finalDamage = 1;
		}

        return finalDamage;
    }


	public void rotateCube(Rotation direction){
		//orden: adelante 0, izq 1, atras 2, der 3, arriba 4, abajo 5

		AbilityState aux = abilities[0];

		switch(direction){

			case(Rotation.LEFT):
				abilities[0] = abilities[3];
				abilities[3] = abilities[2];
				abilities[2] = abilities[1];
				abilities[1] = aux;
				break;

			case(Rotation.RIGHT):
				abilities[0] = abilities[1];
				abilities[1] = abilities[2];
				abilities[2] = abilities[3];
				abilities[3] = aux;
				break;

			case(Rotation.UP):
				abilities[0] = abilities[5];
				abilities[5] = abilities[2];
				abilities[2] = abilities[4];
				abilities[4] = aux;
				break;

			case(Rotation.DOWN):
				abilities[0] = abilities[4];
				abilities[4] = abilities[2];
				abilities[2] = abilities[5];
				abilities[5] = aux;
				break;
		}

		currentAbility = abilities[lastSelectedFace];
	}

	private void selection(){
		if(askSecuence()) {
			// currentPhase = TurnPhase.ENDING;
			// TODO : Ask Caras del cubo
		}
		else if(playerAttack()) {
			currentPhase = TurnPhase.CONFIRMATION;
		} else if (askFaces()) {
			SoundManager.instance.PlaySingle("Cubo_pelea");
		} else if (askEnemy()) {
			// SoundManager.instance.PlaySingle("tu_enemigo_es");
			SoundManager.instance.PlaySingle("un_goblin");
		}
	}

	private void selectFace(){
		bool wasButtonPressed = false;

		if(topEvent()){
			currentAbility = abilities[0];
			wasButtonPressed = true;
			lastSelectedFace = 0;
		}
		if(bottomEvent()){
			currentAbility = abilities[2];
			wasButtonPressed = true;
			lastSelectedFace = 2;
		}
		if(leftEvent()){
			currentAbility = abilities[1];
			wasButtonPressed = true;
			lastSelectedFace = 1;
		}
		if(rightEvent()){
			currentAbility = abilities[3];
			wasButtonPressed = true;
			lastSelectedFace = 3;
		}
		if(frontEvent()){
			currentAbility = abilities[4];
			wasButtonPressed = true;
			lastSelectedFace = 4;
		}
		if(behindEvent()){
			currentAbility = abilities[5];
			wasButtonPressed = true;
			lastSelectedFace = 5;
		}

		if(wasButtonPressed){
			string clip = soundMap.getSelectionClip(currentAbility);
			SoundManager.instance.PlaySingle(clip);
		}
	}

	private void rotate(){
		if(leftEvent() || rightEvent() || topEvent() || bottomEvent()){
			rotateCube(currentRotation);
			string clip = soundMap.getSelectionClip(currentAbility);
			SoundManager.instance.PlaySingle(clip);
		}
	}

	private void feedBack(){
		string audioClip = "";

		if (askMyHP()){
			audioClip = "playerhp"+ System.Math.Ceiling((double)player.getHP()*10 / player.getMaxHP()) * 10;
		}
		else if (askEnemyHP()){
			audioClip = "enemyhp" + System.Math.Ceiling(10 * enemy.getHP() / enemyMaxHp) * 10;
		}
		else if(askCurrentAbility()){
			audioClip = "current_ability_" + currentAbility.ToString().ToLower();
		} else if (askSecuence()) {
			elemento_secuencia();
		}

		if(!audioClip.Equals(""))
			SoundManager.instance.PlaySingle(audioClip);
	}

	private void activateCheatCodes(){
		if(instakill()){
			enemy.setHP(0);
			currentPhase = TurnPhase.ENDING;
		}
	}

	public void playerTurn(){

		switch(currentPhase){
			case(TurnPhase.BEGINNING):
				wasActionSfxPlayed = false;
				currentPhase = TurnPhase.SELECTION;
				break;
			case(TurnPhase.SELECTION):
				//CAMBIAR rotate() POR selectFace() PARA CAMBIAR DE ROTACION DEL CUBO A SELECCION FIJA
				//rotate();
				selectFace();
				feedBack();
				selection();
				activateCheatCodes();
				break;
			case(TurnPhase.CONFIRMATION):
				currentPhase = TurnPhase.CALCULATIONS;
				break;
			case(TurnPhase.CALCULATIONS):
				int dmg = calculateDamage();
				enemy.removeHP (dmg);

				if (dmg < 0) {
					extraPlayerSoundsNeeded = true;
					atkSuffix = "error";
				}
				else if(dmg == 0){
					extraPlayerSoundsNeeded = true;
					atkSuffix = "immune";
				}
				else if(enemy.getMultiplier(currentAbility) == 1){
					extraPlayerSoundsNeeded = true;
					atkSuffix = "regular";
				}
				else if(enemy.getMultiplier(currentAbility) == 0.5 && dmg > 0){
					extraPlayerSoundsNeeded = true;
					atkSuffix = "ineffective";
				}
				else if(enemy.getMultiplier(currentAbility) == 2){
					extraPlayerSoundsNeeded = true;
					atkSuffix = "very_effective";
				}

				if(enemy.getHP() <= 0)
					extraPlayerSoundsNeeded = false;

				currentPhase = TurnPhase.VISIBLE_ACTIONS;
				break;
			case(TurnPhase.VISIBLE_ACTIONS):
				string clip = soundMap.getAttackClip(currentAbility);

				if(!wasActionSfxPlayed){
					SoundManager.instance.PlaySingle(clip);
					wasActionSfxPlayed = true;
				}

				if(extraPlayerSoundsNeeded){
					if(!SoundManager.instance.isEfxPlaying()){
						SoundManager.instance.PlaySingle("attack_" + atkSuffix);
						extraPlayerSoundsNeeded = false;
					}
					//por si se quieren saltar el feedback del ataque
					else if(Input.GetKeyUp (KeyCode.H)){
						extraPlayerSoundsNeeded = false;
						SoundManager.instance.StopSingle();
						currentPhase = TurnPhase.ENDING;
					}
				}
				else
					currentPhase = TurnPhase.ENDING;

				break;
			case(TurnPhase.ENDING):
				if (enemy.getHP () <= 0) {
					Game.GetInstance().playerHP.text = "HP: "+ player.getHP();
					currentState = BattleState.VICTORY;
				} else {
					currentState = BattleState.ENEMY_TURN;
				}
				currentPhase = TurnPhase.BEGINNING;
				break;
		}
	}


	public void enemyTurn(){

		switch(enemyPhase){
			case(TurnPhase.BEGINNING):
				wasEnemySfxPlayed = false;
				enemyPhase = TurnPhase.SELECTION;
				break;
			case(TurnPhase.SELECTION):
				ret = enemy.decide(new System.Object[]{currentAbility});
				enemyPhase = TurnPhase.CONFIRMATION;
				break;
			case(TurnPhase.CONFIRMATION):
				enemyPhase = TurnPhase.CALCULATIONS;
				break;
			case(TurnPhase.CALCULATIONS):
			//esta feo pero funciona por mientras D:
				switch(ret.action){
					case(EnemyAction.BASIC_ATTACK):
						Debug.Log("basic attack");
						player.removeHP((int)ret.values[0]);
						enemyAtkSfx = "enemy_attack_" + (string)ret.values[1] + "_sfx";
						enemyAtkSpeech = "enemy_attack_" + (string)ret.values[1] + "_speech";
						break;
					case(EnemyAction.CONFUSE_PLAYER):
						Debug.Log("confuse player");
						// rotateCube((Rotation)ret.values[0]);
						enemyAtkSfx = "";
						enemyAtkSpeech = "enemy_rotate_" + ret.values[0].ToString().ToLower();
						break;
					case(EnemyAction.POWER_UP):
						Debug.Log("power up");
						enemyAtkSfx = "";
						enemyAtkSpeech = "enemy_power_up";
						break;
					case(EnemyAction.RAISE_DEFENSE):
						Debug.Log("raise defense");
						enemyAtkSfx = "";
						enemyAtkSpeech = "enemy_raise_defense";
						break;
				}
				/*int dmg = UnityEngine.Random.Range(0, 11);
				player.removeHP (dmg);

				//ACA VAN LAS WEAS DE ATAQUE

				if(dmg == 0){
					enemyAtkSpeech = "enemy_attack_miss_speech";
					enemyAtkSfx = "enemy_attack_miss_sfx";
				}

				if(0 < dmg && dmg <= 7){
					enemyAtkSpeech = "enemy_attack_light_hit_speech";
					enemyAtkSfx = "enemy_attack_light_hit_sfx";
				}

				if(7 < dmg && dmg <= 10){
					enemyAtkSpeech = "enemy_attack_heavy_hit_speech";
					enemyAtkSfx = "enemy_attack_heavy_hit_sfx";
				}*/

				enemyPhase = TurnPhase.VISIBLE_ACTIONS;
				break;
			case(TurnPhase.VISIBLE_ACTIONS):
				if(!SoundManager.instance.isEfxPlaying() && !extraPlayerSoundsNeeded && !wasEnemySfxPlayed){
					SoundManager.instance.PlaySingle(enemyAtkSfx);
					wasEnemySfxPlayed = true;
				}
				else if(!SoundManager.instance.isEfxPlaying() && wasEnemySfxPlayed){
					SoundManager.instance.PlaySingle(enemyAtkSpeech);
					enemyPhase = TurnPhase.ENDING;
				}
				else if(Input.GetKeyUp (KeyCode.H)){
					SoundManager.instance.StopSingle();
					enemyPhase = TurnPhase.ENDING;
				}
				break;
			case(TurnPhase.ENDING):
				if (player.getHP () > 0)
				currentState = BattleState.PLAYER_TURN;
				else
				currentState = BattleState.DEFEAT;

				enemyPhase = TurnPhase.BEGINNING;
				break;
		}
	}
	
	protected bool leftEvent(){
		currentRotation = Rotation.LEFT;
		return Input.GetKeyUp (KeyCode.LeftArrow) || (prevState.DPad.Left == ButtonState.Released && state.DPad.Left == ButtonState.Pressed);
	}
	
	protected bool rightEvent(){
		currentRotation = Rotation.RIGHT;
		return Input.GetKeyUp (KeyCode.RightArrow) || (prevState.DPad.Right == ButtonState.Released && state.DPad.Right == ButtonState.Pressed);
	}
	
	protected bool topEvent(){
		// currentRotation = Rotation.UP;
		// return Input.GetKeyUp (KeyCode.UpArrow) || (prevState.DPad.Up == ButtonState.Released && state.DPad.Up == ButtonState.Pressed);
		return (prevState.Buttons.LeftShoulder== ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed);
	}

	protected bool bottomEvent(){
		// currentRotation = Rotation.DOWN;
		// return Input.GetKeyUp (KeyCode.DownArrow) || (prevState.DPad.Down == ButtonState.Released && state.DPad.Down == ButtonState.Pressed);
		return Input.GetKeyUp(KeyCode.U) || (prevState.Triggers.Left >0);
	}

	protected bool frontEvent(){
		// return Input.GetKeyUp (KeyCode.RightControl) || (prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed);
		currentRotation = Rotation.UP;
		return Input.GetKeyUp (KeyCode.UpArrow) || (prevState.DPad.Up == ButtonState.Released && state.DPad.Up == ButtonState.Pressed);
	}

	protected bool behindEvent(){
		// return Input.GetKeyUp (KeyCode.RightShift) || (prevState.Buttons.LeftShoulder== ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed);
		currentRotation = Rotation.DOWN;
		return Input.GetKeyUp (KeyCode.DownArrow) || (prevState.DPad.Down == ButtonState.Released && state.DPad.Down == ButtonState.Pressed);
	}

	protected bool instakill(){
		// return Input.GetKeyUp(KeyCode.Q) || (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed);
		return Input.GetKeyUp(KeyCode.Q);
	}

    protected bool askMyHP(){
        // return Input.GetKeyUp(KeyCode.U) || (prevState.Triggers.Left >0);
        return (prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed);
    }

    protected bool askEnemyHP(){
        return Input.GetKeyUp(KeyCode.I) || (prevState.Triggers.Right > 0);
    }

	protected bool askCurrentAbility(){
		// return Input.GetKeyUp (KeyCode.O) || (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed);
		return false;
	}

	protected bool askFaces() {
		return Input.GetKeyUp (KeyCode.O) || (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed);
	}

    protected bool playerAttack(){
		return Input.GetKeyUp (KeyCode.J) || (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed);
	}
	
	protected bool askSecuence(){
		return Input.GetKeyUp (KeyCode.K) || (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed);
	}

	protected bool askEnemy() {
		return (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed);
	}

	protected void initiar_secuencias() {
		element_numbers = 4;
		secuencia = new int[element_numbers];
    	contador_secuencia = 0;

    	for (int i = 0; i < element_numbers; i++) {
    		secuencia[i] = UnityEngine.Random.Range(0,7); // numeros aleatorios del 0 al 6, 7 es any
    	}
	}

	protected void elemento_secuencia() {
		switch (secuencia[contador_secuencia]) {
			case 0:
				SoundManager.instance.PlaySingle ("face_arcane");
				break;
			case 1:
				SoundManager.instance.PlaySingle ("face_wind");
				break;
			case 2:
				SoundManager.instance.PlaySingle ("face_nature");
				break;
			case 3:
				SoundManager.instance.PlaySingle ("face_water");
				break;
			case 4:
				SoundManager.instance.PlaySingle ("face_earth");
				break;
			case 5:
				SoundManager.instance.PlaySingle ("face_fire");
				break;
			case 6:
				SoundManager.instance.PlaySingle ("face_any");
				break;
			default:
				SoundManager.instance.PlaySingle ("Horse-nay");
				break;
		}
	}

	protected bool perform_attack(int face_number) {
		if (face_number == secuencia[contador_secuencia] || (secuencia[contador_secuencia] == 6)) {
			contador_secuencia++;
			if (contador_secuencia == element_numbers) {
				contador_secuencia = 0;
			}
			return true;
		} else {
			return  false;
		}
	}
}