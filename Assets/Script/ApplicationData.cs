using UnityEngine;
using System.Collections;

/// <summary>
/// Application data.
/// 
/// Contains all the game information about the state of the player,
/// like score, money, last level played, etc.
/// </summary>
public class ApplicationData
{

	private static readonly string LAST_LEVEL_KEY = "0";
	private static readonly string MONEY_KEY = "1";
	private static readonly string SCORE_KEY = "2";
	private static readonly string GAME_SAVED = "3";
	private static readonly string PLAYER_BOOST = "4";
	private static readonly int MAX_LEVEL_INT = 9;

	public static void setLastLevel(int level){
		PlayerPrefs.SetInt (LAST_LEVEL_KEY, level);
	}

	public static void setMoney(int money){
		PlayerPrefs.SetInt (MONEY_KEY, money);
	}

	public static void setScore(int score){
		PlayerPrefs.SetInt (SCORE_KEY, score);
	}

	public static void setBoost(int score, AbilityState ability){
		PlayerPrefs.SetInt (PLAYER_BOOST+ability, score);
	}

	public static void addMoney(int money){
		PlayerPrefs.SetInt (MONEY_KEY, getMoney() + money);
	}

	public static void addScore(int score){
		PlayerPrefs.SetInt (SCORE_KEY, getScore() + score);
	}

	public static void addBoost(int boost, AbilityState ability ){
		PlayerPrefs.SetInt (PLAYER_BOOST+ability, getBoost(ability) + boost);
	}

	public static int getBoost(AbilityState ability){
		return PlayerPrefs.GetInt (PLAYER_BOOST+ability);
	}

	public static int getLastLevel(){
		if (!PlayerPrefs.HasKey (LAST_LEVEL_KEY)) {
			PlayerPrefs.SetInt (LAST_LEVEL_KEY, 1);
			PlayerPrefs.Save();
		}
		return PlayerPrefs.GetInt (LAST_LEVEL_KEY);

	}

	public static int getMoney(){
		if (!PlayerPrefs.HasKey (MONEY_KEY)) {
			PlayerPrefs.SetInt (MONEY_KEY, 0);
			PlayerPrefs.Save();
		}
		return PlayerPrefs.GetInt (MONEY_KEY);
	}

	public static int getScore(){
		if (!PlayerPrefs.HasKey (SCORE_KEY)) {
			PlayerPrefs.SetInt (SCORE_KEY, 0);
			PlayerPrefs.Save();
		}
		return PlayerPrefs.GetInt (SCORE_KEY);
	}

	public static void setSaved(){
		PlayerPrefs.SetInt (GAME_SAVED, 1);
	}

	public static bool existSavedGame(){
		return PlayerPrefs.HasKey (GAME_SAVED);
	}

	public static void save(){
		PlayerPrefs.Save ();
	}

	public static void reset(){
		AbilityState[] listAbilityPool = new AbilityState[]{
			AbilityState.ARCANO,
			AbilityState.VIENTO,
			AbilityState.NATURALEZA,
			AbilityState.AGUA,
			AbilityState.TIERRA,
			AbilityState.FUEGO
		};

		foreach (AbilityState state in listAbilityPool)
			setBoost (0, state);
		setLastLevel(1);
		setMoney(0);
		setScore(0);
		setSaved ();
	}

	public static int getMaxLevel(){
		return MAX_LEVEL_INT;
	}

}

