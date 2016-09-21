using UnityEngine;
using System.Collections;

public class PlayerMovementRestriction : MonoBehaviour
{
	private LevelData data;
	private AudioSource source;

	private bool toggle = true;
	// public AudioClip choque;

	private float lastPress = 1f;
	private float delay = 1f;
	
	void Start(){
		this.source = GetComponent<AudioSource>();
	}

	public bool canMove(Vector2 pos){
		return Room.GetInstance (). walkableCell(pos);
	}


	public void playCrashSound(Vector2 pos){
		if (Time.time - lastPress > delay) {
			lastPress = Time.time;
            /*RoomEntity go = Room.GetInstance().isInside(pos)?Room.GetInstance ().getEntityAt (pos):null;
			if (go == null) {
				source.PlayOneShot(choque);
			} else {
				go.GetComponent<AudioSource> ().Play ();
            }*/
            // source.PlayOneShot(choque);
            SoundManager.instance.PlaySingle ("error");

        }
	}


}

