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


	public RoomEntity instantiateRoomObject(int i, int j){
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
		case Entity.SIGNAL_DOWN:
			return instantiateDownSignal(i, j);
		case Entity.SIGNAL_UP:
			return instantiateUpSignal(i, j);
		case Entity.SIGNAL_LEFT:
			return instantiateLeftSignal(i, j);
		case Entity.SIGNAL_RIGHT:
			return instantiateRightSignal(i, j);
		case Entity.SIGNAL_MULTI:
			return instantiateMultiSignal(i, j);
		case Entity.HELP:
			return instantiateHelp(i, j);
		case Entity.CARTESIAN:
			return instantiateCartesian(i, j);
		default:
			return null;
		}
	}

	private RoomEntity instantiateTreasure(int i, int j){
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		go.GetComponent<MeshRenderer>().material = Resources.Load("YellowMat") as Material;
		updateObjectTransform (go, i, j);
		//go.AddComponent<AudioSource> ();
		//go.GetComponent<AudioSource> ().clip = Resources.Load ("crashTreasure") as AudioClip;
		return go.AddComponent<TreasureEntity> () as TreasureEntity;

	}

	private RoomEntity instantiateDoor(int i, int j){
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		go.GetComponent<MeshRenderer>().material = Resources.Load("BrownMat") as Material;
		go.transform.parent = parent;
		go.transform.localScale = new Vector3 (0.8f, 2.3f, 0.1f);
		
		Orientation o = data.getOrientationAt (i, j);
		go.transform.Rotate (o.getRotation());
		go.transform.position = Room.GetInstance ().getWorldPosition (new Vector3(i, 0.15f, j));
		//AudioSource source = go.AddComponent<AudioSource> ();
		//source.playOnAwake = false;
		//source.clip = Resources.Load ("crashDoor") as AudioClip;
		return go.AddComponent<DoorEntity> () as DoorEntity;
	}

	private RoomEntity instantiateMonster(int i, int j){
		GameObject go = Object.Instantiate(Resources.Load("monster1")) as GameObject;
		go.transform.parent = parent;
		Orientation o = data.getOrientationAt (i, j);
		go.transform.Rotate (o.getRotation());
		go.transform.position = Room.GetInstance ().getWorldPosition (new Vector3(i, 0f, j));
		//go.AddComponent<AudioSource> ();
		//go.GetComponent<AudioSource> ().clip = Resources.Load ("crashMonster") as AudioClip;
        MonsterEntity m = go.AddComponent<MonsterEntity>() as MonsterEntity;
        m.setPosition(i, j);
        return m;
	}

	private RoomEntity instantiateTrap(int i, int j){
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		go.GetComponent<MeshRenderer>().material = Resources.Load("BlackMat") as Material;
		updateObjectTransform (go, i, j);
		//go.AddComponent<AudioSource> ();
		//go.GetComponent<AudioSource> ().clip = Resources.Load ("crashTrap") as AudioClip;
		return go.AddComponent<TrapEntity> () as TrapEntity;
	}

	private RoomEntity instantiateGeometricRoom(int i, int j){
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		go.GetComponent<MeshRenderer>().material = Resources.Load("BlueMat") as Material;
		updateObjectTransform (go, i, j);
		int init = data.getInitAt(i,j);
		WarpEntity we = go.AddComponent<WarpEntity> () as WarpEntity;
		we.selectRiddle(init);
		return we;
	}

	private RoomEntity instantiateCartesian(int i, int j){
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		go.GetComponent<MeshRenderer>().material = Resources.Load("BrownMat") as Material;
		updateObjectTransform (go, i, j);
		return go.AddComponent<CartesianEntity> () as CartesianEntity;;
	}

	private RoomEntity instantiateDownSignal(int i, int j){
		GameObject go = new GameObject("DownSignal");
		go.transform.parent = parent;
		return go.AddComponent<DownConditionalSignalEntity> ();
	}

	private RoomEntity instantiateUpSignal(int i, int j){
		GameObject go = new GameObject("UpSignal");
		go.transform.parent = parent;
		return go.AddComponent<UpConditionalSignalEntity> ();
	}

	private RoomEntity instantiateLeftSignal(int i, int j){
		GameObject go = new GameObject("LeftSignal");
		go.transform.parent = parent;
		return go.AddComponent<LeftConditionalSignalEntity> ();
	}

	private RoomEntity instantiateRightSignal(int i, int j){
		GameObject go = new GameObject("RightSignal");
		go.transform.parent = parent;
		return go.AddComponent<RightConditionalSignalEntity> ();
	}


	private RoomEntity instantiateMultiSignal(int i, int j){
		GameObject go = new GameObject("MultiSignal");
		go.transform.parent = parent;
		RoomEntity entity = go.AddComponent<MultiSignalEntity> ();
		return entity;
	}


	private RoomEntity instantiateHelp(int i, int j){
		GameObject go = new GameObject("Help");
		go.transform.parent = parent;
		WaitEntity signal = go.AddComponent<WaitEntity> ();
		signal.setDuration (data.getSoundDuration(i,j));
		signal.source = data.getSound(i,j);
		return signal;
	}

	private void updateObjectTransform(GameObject go, int i, int j){
		go.transform.parent = parent;
		go.transform.localScale = new Vector3 (0.8f, 0.3f, 0.5f);
		
		Orientation o = data.getOrientationAt (i, j);
		go.transform.Rotate (o.getRotation());
		go.transform.position = Room.GetInstance ().getWorldPosition (new Vector3(i, 0.15f, j));
	}
}

