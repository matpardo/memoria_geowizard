using UnityEngine;
using System.Collections;

public class Orientation
{
	private int orientation;

	private Orientation(){

	}

	public static Orientation NORTH {
		get {
			Orientation o = new Orientation ();
			o.orientation = 0;
			return o;
		}
		private set{}
	}

	public static Orientation SOUTH {
		get {
			Orientation o = new Orientation ();
			o.orientation = 2;
			return o;
		}
		private set{}
	}

	public static Orientation EAST {
		get {
			Orientation o = new Orientation ();
			o.orientation = 1;
			return o;
		}
		private set{}
	}

	public static Orientation WEST {
		get {
			Orientation o = new Orientation ();
			o.orientation = -1;
			return o;
		}
		private set{}
	}

	public Vector3 getRotation(){
		return  new Vector3(0f, orientation * 90f, 0f);
	}
}

