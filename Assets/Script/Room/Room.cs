using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Room.
/// 
/// Contiene la estructura e informacion de un nivel especifico.
/// </summary>
public class Room
{
	private static Room instance;

	public static Room GetInstance(){
		return instance;
	}

	/// <summary>
	/// The level information.
	/// </summary>
	private LevelData data;

	private RoomEntity[][] entities;

	/// <summary>
	/// The hall structure.
	/// </summary>
	private Hall hall;

	/// <summary>
	/// The scale of the room.
	/// </summary>
	private RoomScale scale;

	public Room(RoomScale scale){
		data = LevelData.getLevel (ApplicationData.getLastLevel());
		int width = data.getRoomWidth ();
		int height = data.getRoomHeight ();

		entities = new RoomEntity[height][];
		for (int i=0; i < height; i++) {
			entities[i] = new RoomEntity[width];
		}

		instance = this;
		HallBuilder hallBuilder = new HallBuilder ();

		this.hall = hallBuilder.build (data);
		this.scale = scale;
		HallMeshBuilder meshBuilder = new HallMeshBuilder ();
		meshBuilder.setHall (hall);
		meshBuilder.setScale (scale);
		meshBuilder.process ();
		RoomObjectGenerator generator = new RoomObjectGenerator (data);

		
		for (int i=0; i<width; i++) {
			for (int j=0; j<height; j++) {
				entities[j][i] = generator.instantiateRoomObject(i, j);
			}
		}

		Game.GetInstance ().player.wait (6);
		if (ApplicationData.getLastLevel () == 1) {
			SoundManager.instance.PlaySingle ("introduccion");
		} else {
			SoundManager.instance.PlaySingle ("entrada-laberinto");
		}
	}

   

    /// <summary>
    /// Gets the room scale.
    /// </summary>
    /// <returns>The room scale.</returns>
    public float getRoomScale(){
		return scale.getHallWidth ();
	}

	/// <summary>
	/// Gets the player start position.
	/// </summary>
	/// <returns>The start position.</returns>
	public Vector2 getStartPosition(){
		return data.getStartPosition ();
	}

	/// <summary>
	/// Gets the world position of a given position inside the room.
	/// </summary>
	/// <returns>The world position.</returns>
	/// <param name="pos">Position.</param>
	public Vector3 getWorldPosition(Vector2 pos){
		return new Vector3 (
			pos.x * getRoomScale(),
			1f,
			- pos.y * getRoomScale());
	}

	/// <summary>
	/// Gets the world position of a given position inside the room.
	/// </summary>
	/// <returns>The world position.</returns>
	/// <param name="pos">Position.</param>
	public Vector3 getWorldPosition(Vector3 pos){
		return new Vector3 (
			pos.x * getRoomScale(),
			pos.y,
			- pos.z * getRoomScale());
	}

	/// <summary>
	/// Returns true if the specified position of the room is walkable.
	/// </summary>
	/// <returns>The <see cref="System.Boolean"/>.</returns>
	/// <param name="pos">Position.</param>
	public bool walkableCell(Vector2 pos){
		if (!isInside (pos)) {
			return false;
		}
		return data.walkableCell ((int)pos.x, (int) pos.y);
	}

	/// <summary>
	/// Returns true if the given position is inside the limits of the room.
	/// </summary>
	/// <returns><c>true</c>, if inside was ised, <c>false</c> otherwise.</returns>
	/// <param name="pos">Position.</param>
	public bool isInside(Vector2 pos){
		return 0 <= pos.x && pos.x < hall.getWidth () &&
			0 <= pos.y && pos.y < hall.getHeight ();
	}

	public RoomEntity getEntityAt(Vector2 pos){
		if (!isInside (pos))
			return null;
		return entities [(int)pos.y] [(int)pos.x];
	}

    internal void removeEntity(Vector2 pos)
    {
		entities [(int)pos.y] [(int)pos.x] = null;
        data.removeEntity((int)pos.x, (int)pos.y);
    }

    public void removeMonster(Vector2 pos)
    {
        data.removeMonster(pos);
    }

    public float getMinDistanceMonster(Vector2 pos)
    {
        return data.getMinDistanceMonster(pos);
    }

	public float getMinDistanceTrap(Vector2 pos)
	{
		return data.getMinDistanceTrap(pos);
	}
	
	public float getDoorDistance(Vector2 pos){
		return data.getDoorDistance(pos);
	}

	public void ask(Vector2 pos){
		if (!isInside (pos) || (isInside (pos) && !walkableCell(pos)))
			SoundManager.instance.PlaySingle ("una-muralla");
		else if (getEntityAt (pos) != null) {
			getEntityAt (pos).ask ();
		}
	}

	public void touch(Vector2 pos) {
		if (!isInside (pos) || (isInside (pos) && !walkableCell(pos)))
			SoundManager.instance.PlaySingle ("crashWall");
		else if (getEntityAt (pos) != null) {
			getEntityAt (pos).touch ();
		}
	}

}

