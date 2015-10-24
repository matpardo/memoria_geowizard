using UnityEngine;
using System.Collections;

public class RoomObjectGenerator
{
	private LevelData data;
	private Transform parent;

	public RoomObjectGenerator(LevelData data){
		this.data = data;
		parent = GameObject.Find ("RoomObject").transform;
	}


	public GameObject instantiateRoomObject(int i, int j){
		switch (data.getEntityAt (i, j)) {
		case Entity.TREASURE:
			return instantiateTreasure(i , j);
		case Entity.MONSTER:
			return instantiateMonster(i, j);
		case Entity.TRAP:
			return instantiateTrap(i, j);
		case Entity.GEOMETRIC:
			return instantiateGeometricRoom(i, j);
		case Entity.DOOR:
			return instantiateDoor(i, j);
		default:
			return null;
		}
	}

	private GameObject instantiateTreasure(int i, int j){
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		go.GetComponent<MeshRenderer>().material = Resources.Load("YellowMat") as Material;
		updateObjectTransform (go, i, j);
		go.AddComponent<AudioSource> ();
		go.GetComponent<AudioSource> ().clip = Resources.Load ("crashTreasure") as AudioClip;
		return go;

	}

	private GameObject instantiateDoor(int i, int j){
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		go.GetComponent<MeshRenderer>().material = Resources.Load("BrownMat") as Material;
		go.transform.parent = parent;
		go.transform.localScale = new Vector3 (0.8f, 2.3f, 0.1f);
		
		Orientation o = data.getOrientationAt (i, j);
		go.transform.Rotate (o.getRotation());
		go.transform.position = Room.GetInstance ().getWorldPosition (new Vector3(i, 0.15f, j));
		go.AddComponent<AudioSource> ();
		go.GetComponent<AudioSource> ().clip = Resources.Load ("crashDoor") as AudioClip;
		return go;
	}

	private GameObject instantiateMonster(int i, int j){
		GameObject go = Object.Instantiate(Resources.Load("monster1")) as GameObject;
		go.transform.parent = parent;
		Orientation o = data.getOrientationAt (i, j);
		go.transform.Rotate (o.getRotation());
		go.transform.position = Room.GetInstance ().getWorldPosition (new Vector3(i, 0f, j));
		go.AddComponent<AudioSource> ();
		go.GetComponent<AudioSource> ().clip = Resources.Load ("crashMonster") as AudioClip;
		return go;
	}

	private GameObject instantiateTrap(int i, int j){
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		go.GetComponent<MeshRenderer>().material = Resources.Load("BlackMat") as Material;
		updateObjectTransform (go, i, j);
		go.AddComponent<AudioSource> ();
		go.GetComponent<AudioSource> ().clip = Resources.Load ("crashTrap") as AudioClip;
		return go;
	}

	private GameObject instantiateGeometricRoom(int i, int j){
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		go.GetComponent<MeshRenderer>().material = Resources.Load("BlueMat") as Material;
		updateObjectTransform (go, i, j);
		return go;
	}

	private void updateObjectTransform(GameObject go, int i, int j){
		go.transform.parent = parent;
		go.transform.localScale = new Vector3 (0.8f, 0.3f, 0.5f);
		
		Orientation o = data.getOrientationAt (i, j);
		go.transform.Rotate (o.getRotation());
		go.transform.position = Room.GetInstance ().getWorldPosition (new Vector3(i, 0.15f, j));
	}
}
