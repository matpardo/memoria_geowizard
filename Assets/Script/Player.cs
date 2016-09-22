using UnityEngine;
using System.Collections.Generic;
using XInputDotNetPure;

/// <summary>
/// Player.
/// 
/// This class (for now) offer all we need to move the character through the room.
/// </summary>
public class Player : MonoBehaviour
{
    private int maxHp;
	private int hp;
	private int boostsToLevelUp;
	private bool ableToMove;
	private TreasureEntity actual_treasure;

	/// <summary>
	/// The actual geometric room level
	/// </summary>
	private int geomLevel;

	private AbilityState[] currentAbilityState;
	public AbilityState[] CurrentAbilityStates{
		get{return currentAbilityState;}
		set{currentAbilityState = value;}
	}

	private Dictionary<AbilityState, int> abilitiesBaseDamage;
	private Dictionary<AbilityState, int> boosts;

	/// <summary>
	/// The movement restriction of the player.
	/// </summary>
	public PlayerMovementRestriction movementRestriction;

	/// <summary>
	/// The movement speed of the character.
	/// </summary>
	public float speed = 2f;

	/// <summary>
	/// The coord position relative to the room.
	/// </summary>
	private Vector2 position;

	/// <summary>
	/// The current direction where the character is facing.
	/// </summary>
	private int currentDir;

	/// <summary>
	/// The posible directions of the player.
	/// </summary>
	private Vector2[] directions;

	/// <summary>
	/// The current state of the player.
	/// </summary>
	public PlayerState state;


	/// <summary>
	/// The last state of the player before him started to wait.
	/// </summary>
	private PlayerState lastState;

	/// <summary>
	/// The time the player should wait in the wait state.
	/// </summary>
	private float waitTime = 0f;

	public Vector2 direction{
		get{return directions[currentDir];}
	}

	/// <summary>
	/// The countdown for smooth movement.
	/// </summary>
	private float countdown;

	/// <summary>
	/// The start position before moving the player.
	/// </summary>
	private Vector3 startPosition;

	/// <summary>
	/// The destination position where we want to move the player.
	/// </summary>
	private Vector3 destPosition;

	/// <summary>
	/// The start rotation before rotating  the player.
	/// </summary>
	private Quaternion startRotation;

	/// <summary>
	/// The destination rotation the player should have after rotating.
	/// </summary>
	private Quaternion destRotation;

	void Awake(){
		DontDestroyOnLoad (transform.gameObject);
	}

	void Start(){
		directions = new Vector2[]{
			new Vector2(0, -1), //north
			new Vector2(1, 0), //east
			new Vector2(0, 1), //south
			new Vector2(-1, 0) //west
		};
		geomLevel = 2;
		currentDir = 0;
		startRotation = Quaternion.identity;
		destRotation = Quaternion.identity;
		state = PlayerState.NO_STAFF;

		source = GetComponent<AudioSource>();

		//faces order: front (initial): 0, left: 1, back: 2, right 3, up 4, down 5
		//always starts with water ability unlocked
		currentAbilityState = new AbilityState[]{
			AbilityState.ARCANO,
			AbilityState.VIENTO,
			AbilityState.NATURALEZA,
			AbilityState.AGUA,
			AbilityState.TIERRA,
			AbilityState.FUEGO
		};

		actual_treasure = null;

		boostsToLevelUp = 3;

		ableToMove = true;

		//base damage for each ability starts at 1
		abilitiesBaseDamage = new Dictionary<AbilityState, int>();

		//boosts for each ability starts at 0
		//every 3 boosts for some ability, the base damage for said ability increases by 1
		boosts = new Dictionary<AbilityState, int>();

		foreach(AbilityState AS in System.Enum.GetValues(typeof(AbilityState))){
			boosts.Add(AS, 0);
			abilitiesBaseDamage.Add(AS, 1);
			addBoost(AS, ApplicationData.getBoost (AS));
		}
	}

	void FixedUpdate ()
	{
		if (state == PlayerState.STOPPED) {
			return;
		}
		if (state == PlayerState.MOVING) {
			updatePosition ();
		}
		else if (state == PlayerState.TURNING) {
			updateRotation ();
		} else if (state == PlayerState.WAITING) {
			waitTime -= Time.deltaTime;
			if(waitTime <= 0)
				state = lastState;
		}

	}

	/// <summary>
	/// Updates the position of the player.
	/// </summary>
	private void updatePosition(){
		if (Vector3.Distance (transform.position, destPosition) <= 0.025f) {
			transform.position = destPosition;
			state = PlayerState.STOPPED;
			CollisionManager.collide(position);
			countdown = 0f;
			return;
		}
		countdown -= Time.deltaTime * speed;
		transform.position = Vector3.Lerp (startPosition, destPosition, 1f - countdown);
	}

	/// <summary>
	/// Updates the rotation of the player.
	/// </summary>
	private void updateRotation(){
		if (Quaternion.Angle (transform.rotation, destRotation) <= 0.1f) {
			transform.rotation = destRotation;
			state = PlayerState.STOPPED;
			countdown = 0f;
			wait(0.5f);
			return;
		}
		countdown -= Time.deltaTime * speed;
		transform.rotation = Quaternion.Lerp (startRotation, destRotation, 1f - countdown);
	}

	/// <summary>
	/// Gets the current position.
	/// </summary>
	/// <returns>The position.</returns>
	public Vector2 getPosition(){
		return position;
	}

	/// <summary>
	/// Sets the position.
	/// Don't use this to move the character. This method just set the coord position
	/// of the player in the room without moving it.
	/// </summary>
	/// <param name="pos">Position.</param>
	public void setPosition(Vector2 pos){
		position = pos;
	}

	/// <summary>
	/// Sets the position.
	/// Don't use this to move the character. This method just set the coord position
	/// of the player in the room without moving it..
	/// </summary>
	/// <param name="i">X Coord.</param>
	/// <param name="j">Y Coord</param>
	public void setPosition(int i, int j){
		position.x = i;
		position.y = j;
	}

	/// <summary>
	/// Move this instance one coord ahead smoothly.
	/// </summary>

	private bool toggle = true;
	private AudioSource source;
	public AudioClip paso1, paso2;

	public void move(){
		if (state == PlayerState.NO_STAFF) {
			noStaffAlert();
			return;
		}
		if (state != PlayerState.STOPPED || !ableToMove)
			return;

		Vector2 dest = position + direction;
		//CollisionManager.collide(dest);
		if (!canMove(dest)) {

            GamePad.SetVibration((PlayerIndex)0, 0.2f, 0.2f);
            movementRestriction.playCrashSound(dest);

            GamePad.SetVibration((PlayerIndex)0, 0.0f, 0.0f);
            return;
		}


		position += direction;
		startPosition = transform.position;
		destPosition = Room.GetInstance ().getWorldPosition (getPosition());
		countdown = 1f;
		state = PlayerState.MOVING;

		if (toggle)
			source.PlayOneShot (paso1);
		else
			source.PlayOneShot (paso2);

		toggle = !toggle;
	}

	/// <summary>
	/// Turns to the right smoothly.
	/// </summary>
	public void turnRight(){
		if (state == PlayerState.NO_STAFF) {
			noStaffAlert();
		}
		if (state != PlayerState.STOPPED || !ableToMove) {
			return;
		}
		makeTurningRightSound ();
		currentDir = (currentDir + 1) % 4;
		rotate (90f);
	}

	/// <summary>
	/// Turns to the left smoothly.
	/// </summary>
	public void turnLeft(){
		if (state == PlayerState.NO_STAFF) {
			noStaffAlert();
		}
		if (state != PlayerState.STOPPED || !ableToMove) {
			return;
		}
		makeTurningLeftSound ();
		currentDir--;
		currentDir = currentDir >= 0 ? currentDir : 3;
		rotate (-90f);
	}

	/// <summary>
	/// Rotate the character the specified degrees
	/// </summary>
	/// <param name="degrees">Degrees.</param>
	private void rotate(float degrees){
		
		startRotation = transform.rotation;
		destRotation = transform.rotation * Quaternion.Euler (Vector3.up * degrees);
		countdown = 1f;
		state = PlayerState.TURNING;
	}

	/// <summary>
	/// True if the player can move one coord ahead.
	/// </summary>
	/// <returns><c>true</c>, if the movement could be done, <c>false</c> otherwise.</returns>
	private bool canMove(Vector2 pos){
		return movementRestriction.canMove (pos);
	}

	public void removeHP(int dmg){
		this.hp -= dmg;
        if (this.hp < 0)
        {
            this.setHP(0);
        }
	}

    public void setHP(int hp)
    {
        this.hp = hp;
    }

	public int getHP(){
		return this.hp;
	}

    public void setMaxHP(int maxHp)
    {
        this.maxHp = maxHp;
    }

    public int getMaxHP()
    {
        return this.maxHp;
    }

    public void toggleMovementBlock(){
		ableToMove = !ableToMove;
	}

	public void enableMovement(){
		ableToMove = true;
	}

	public void disableMovement(){
		ableToMove = false;
	}

	public int getBaseDamage(AbilityState ability){
		return abilitiesBaseDamage[ability];
	}

	public void setBaseDamage(AbilityState ability, int dmg){
		abilitiesBaseDamage[ability] = dmg;
	}

	public int getBoost(AbilityState ability){
		return boosts[ability];
	}

	public void addBoost(AbilityState ability, int amount = 1){
		boosts[ability] += amount;
		abilitiesBaseDamage[ability] = boosts[ability] / boostsToLevelUp + 1;
		//abilitiesBaseDamage[ability] += boosts[ability] / boostsToLevelUp;
		//boosts[ability] = boosts[ability] % boostsToLevelUp;
	}
	
	private void makeTurningSound(){
		SoundManager.instance.PlaySingle("turning");
	}

	private void makeTurningLeftSound(){
		SoundManager.instance.PlayDirectionalSingle("giro-izquierda", -1.0f);
	}
	
	private void makeTurningRightSound(){
		SoundManager.instance.PlayDirectionalSingle("giro-derecha", 1.0f);
	}

	private void noStaffAlert(){
		wait (8);
		SoundManager.instance.PlaySingle ("sostener-baston");
		Invoke ("ayudaBaston", 4);
	}

	private void ayudaBaston (){SoundManager.instance.PlaySingle ("ayuda-baston");}

	public void pickStaff(){
		state = PlayerState.STOPPED;
		wait (1);
		SoundManager.instance.PlaySingle("baston_despliegue_interior_n");
		Debug.Log("Staff picked");
	}

	public Orientation getOrientation(){
		switch (currentDir) {
		case 0:
			return Orientation.NORTH;
		case 1:
			return Orientation.EAST;
		case 2:
			return Orientation.SOUTH;
		default:
			return Orientation.WEST;
		}
	}

	public void wait(float seconds){
		waitTime = seconds;
		if (state == PlayerState.WAITING)
			return;
		lastState = state;
		state = PlayerState.WAITING;

	}

	public void askAhead(){
		Room.GetInstance ().ask (frontPosition());
		wait (0.5f);
	}

	public void askRight(){
		Vector2 dest = rightPosition ();
		Room.GetInstance ().ask (dest);
		wait (0.5f);
	}

	public void askLeft(){
		Vector2 dest = leftPosition ();
		Room.GetInstance ().ask (dest);
		wait (0.5f);
	}

	public void askBehind() {
		Room.GetInstance ().ask (backPosition());
		wait (0.5f);
	}

	public Vector2 rightPosition(){
		int rightDir = (currentDir + 1) % 4;
		return position + directions [rightDir];
	}
	public Vector2 leftPosition(){
		int leftDir = (currentDir - 1) >= 0 ? (currentDir-1) : 3;
		return position + directions [leftDir];
	}
	public Vector2 frontPosition(){
		return position + direction;
	}

	public Vector2 backPosition() {
		return position - direction;
	}

	// public Vector2 overPosition() {

	// }

	// public Vector2 bottomPosition() {

	// }

	public void touchAhead() {
		Room.GetInstance ().touch (frontPosition());
		wait (0.5f);
	}

	public void touchRight() {
		Vector2 dest = rightPosition ();
		Room.GetInstance ().touch (dest);
		wait (0.5f);
	}

	public void touchLeft() {
		Vector2 dest = leftPosition ();
		Room.GetInstance ().touch (dest);
		wait (0.5f);
	}

	public void touchBehind() {
		Room.GetInstance ().touch (backPosition());
		wait (0.5f);
	}

	public void touchOver() {
		SoundManager.instance.PlaySingle ("Horse-nay");
	}

	public void touchBottom()  {
		SoundManager.instance.PlaySingle ("Horse-nay");
	}

	public int getGeomLevel(){
		return geomLevel;
	}

	public void setGeomLevel(int newLevel){
		geomLevel = newLevel;
	}

	public void increaseOneGeomLevel(){
		geomLevel = geomLevel + 1;
	}

	// TODO : Documentar
	public void askOrientation(){
		wait (1);
		switch (currentDir) {
		case 0:
			{SoundManager.instance.PlaySingle ("norte");
			return;}
		case 1:
			{SoundManager.instance.PlaySingle ("este");
			return;}
		case 2:
			{SoundManager.instance.PlaySingle ("sur");
			return;}
		default:
			{SoundManager.instance.PlaySingle ("oeste");
			return;}
		}
	}

	public void setTreasure(TreasureEntity treasure) {
		lastState = state;
		state = PlayerState.ON_TREASURE;
		actual_treasure = treasure;
	}

	public void getTreasure(int position) {
		if (actual_treasure) {
			wait (0.5f);
			if (actual_treasure.isPositionTreasure(position)) {
				actual_treasure.getLoot();
				actual_treasure.makeDestroyable();
				Destroy(actual_treasure.gameObject);
				actual_treasure = null;
				state = PlayerState.STOPPED;
				wait (5);
			}
		} else {
			SoundManager.instance.PlaySingle ("Horse-nay");
		}
	}

	public void doLeftSound() {
		// SoundManager.instance.PlayDirectionalSingle ("pew-left", -1f);
		switch (currentDir) {
		case 0:
			{SoundManager.instance.PlayDirectionalSingle ("wind_select", -1f);
			return;}
		case 1:
			{SoundManager.instance.PlayDirectionalSingle ("earth_select", -1f);
			return;}
		case 2:
			{SoundManager.instance.PlayDirectionalSingle ("water_select", -1f);
			return;}
		default:
			{SoundManager.instance.PlayDirectionalSingle ("fire_select", -1f);
			return;}
		}
	}

	public void doRightSound() {
		// SoundManager.instance.PlayDirectionalSingle ("pew-right", 1f);
		switch (currentDir) {
		case 0:
			{SoundManager.instance.PlayDirectionalSingle ("water_select", 1f);
			return;}
		case 1:
			{SoundManager.instance.PlayDirectionalSingle ("fire_select", 1f);
			return;}
		case 2:
			{SoundManager.instance.PlayDirectionalSingle ("wind_select", 1f);
			return;}
		default:
			{SoundManager.instance.PlayDirectionalSingle ("earth_select", 1f);
			return;}
		}
	}

	public void doFrontSound() {
		// SoundManager.instance.PlayDirectionalSingle ("pew", 0f);
		switch (currentDir) {
		case 0:
			{SoundManager.instance.PlayDirectionalSingle ("earth_select", 0f);
			return;}
		case 1:
			{SoundManager.instance.PlayDirectionalSingle ("water_select", 0f);
			return;}
		case 2:
			{SoundManager.instance.PlayDirectionalSingle ("fire_select", 0f);
			return;}
		default:
			{SoundManager.instance.PlayDirectionalSingle ("wind_select", 0f);
			return;}
		}
	}
}

