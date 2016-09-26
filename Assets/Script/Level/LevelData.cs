using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class LevelData
{

	private string[] hallData;
	private Vector2 startPosition;
	private Dictionary<string, Orientation> orientations;
	private Dictionary<string, string> sounds;
	private Dictionary<string, float> soundsDuration;
	private Vector2 door;
    private ArrayList monsters;
	private ArrayList traps;

	public LevelData(){
		orientations = new Dictionary<string, Orientation> ();
        monsters = new ArrayList();
		traps = new ArrayList();
		sounds = new Dictionary<string, string> ();
		soundsDuration = new Dictionary<string, float> ();
	}

	public Vector2 getStartPosition(){
		return startPosition;
	}

	public int getRoomWidth(){
		return hallData [0].Length;
	}

	public int getRoomHeight(){
		return hallData.Length;
	}

	public bool wallAt(int i, int j){
		return hallData[j][i].Equals('#');
	}

	public bool walkableCell(int i, int j){
		return !hallData [j] [i].Equals ('#');
	}

	public void addOrientation(int i, int j, Orientation o){
		orientations.Add (i + "," + j, o);
	}

	public void addSound(int i, int j, string sound, float duration){
		sounds.Add (i + "," + j, sound);
		soundsDuration.Add (i + "," + j, duration);
	}

	public string getSound(int i, int j){
		return sounds [i + "," + j];
	}

	public float getSoundDuration(int i, int j){
		return soundsDuration [i + "," + j];
	}

	public Orientation getOrientationAt(int i, int j){
		return orientations [i + "," + j];
	}

	public void addDoor(int i, int j)
	{
		door = new Vector2 (i, j);
	}

    public void addMonster(int i, int j)
    {
        monsters.Add(new Vector2(i, j));
    }

	public void addTrap(int i, int j)
	{
		traps.Add(new Vector2(i, j));
	}

	public void removeTrap(Vector2 pos)
	{
		foreach (Vector2 v in traps)
		{
			if (v.Equals(pos))
			{
				traps.Remove(v);
				break;
			}
		}
	}
	
	public void removeTrap(int i, int j)
	{
		removeTrap(new Vector2(i, j)); 
	}
	
	public void removeMonster(Vector2 pos)
	{
		foreach (Vector2 v in monsters)
		{
            if (v.Equals(pos))
            {
                monsters.Remove(v);
                break;
            }
        }
    }

    public void removeMonster(int i, int j)
    {
        removeMonster(new Vector2(i, j)); 
    }

	public Entity getEntityAt(int i, int j){
		switch (hallData[j][i]){
		case '#':
			return Entity.WALL;
		case ' ':
			return Entity.FLOOR;
		case 'T':
			return Entity.TREASURE;
		case 'X':
			return Entity.TRAP;
		case 'D':
			return Entity.DOOR;
		case 'M':
			return Entity.MONSTER;
		case '-':
			return Entity.SIGNAL_MULTI;
		case 'u':
			return Entity.SIGNAL_UP;
		case 'd':
			return Entity.SIGNAL_DOWN;
		case 'l':
			return Entity.SIGNAL_LEFT;
		case 'r':
			return Entity.SIGNAL_RIGHT;
		case 'W':
			return Entity.GEOMETRIC;
		case 'h':
			return Entity.HELP;
		default:
			return Entity.WALL;
		}
	}

	public static LevelData getLevel(int n){
		LevelData levelData = new LevelData ();

		switch (n) {
			case 1:
				levelData.hallData = new string[]{
					"#D#############",
					"# #############",
					"# #############",
					"# #############",
					"# #############",
					"# #############",
					"#h#############",
					"# #############",
					"# #############",
				};
				levelData.startPosition = new Vector2(1, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(1, 0, Orientation.SOUTH);
				levelData.addDoor(1, 0);
				// help
				levelData.addSound(1, 6, "ayuda-brujula", 16);
	            break;
	        case 2:
				levelData.hallData = new string[]{
					"###############",
					"###############",
					"###############",
					"########D######",
					"######## ######",
					"#   h    ######",
					"# #############",
					"#h#############",
					"# #############",
				};
				levelData.startPosition = new Vector2(1, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(8, 3, Orientation.SOUTH);
				levelData.addDoor(8, 3);
				// help
				levelData.addSound(1, 7, "ayuda-first-rotation", 16);
				levelData.addSound(4, 5, "ayuda-touch-and-ask", 16);
	            break;
	        case 3:
				levelData.hallData = new string[]{
					"######D########",
					"###### ########",
					"###### ########",
					"######      ###",
					"########### ###",
					"########### ###",
					"######      ###",
					"######h########",
					"###### ########",
				};
				levelData.startPosition = new Vector2(6, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(6, 0, Orientation.SOUTH);
				levelData.addDoor(6, 0);
				//help
				levelData.addSound(6, 7, "continue", 6);
	            break;

	        case 4:
				levelData.hallData = new string[]{
					"#D#############",
					"# #############",
					"# #############",
					"#       hlh ###",
					"########### ###",
					"########### ###",
					"########### ###",
					"########### ###",
					"########### ###",
				};
				levelData.startPosition = new Vector2(11, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(1, 0, Orientation.SOUTH);
				levelData.addDoor(1, 0);
				// Help
				levelData.addSound(10, 3, "sonido-positivo", 5);
				levelData.addSound(8, 3, "sonido-negativo", 5);
	            break;

	        case 5:
				levelData.hallData = new string[]{
					"#######D#######",
					"####### #######",
					"#######u#######",
					"###    -    ###",
					"###u#######u###",
					"### ####### ###",
					"###    -    ###",
					"####### #######",
					"####### #######",
				};
				levelData.startPosition = new Vector2(7, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(7, 0, Orientation.SOUTH);
				levelData.addDoor(7, 0);
				//help
				levelData.addSound(1, 7, "ayuda-first-cubo", 17);
	            break;
	                    
	        case 6:
				levelData.hallData = new string[]{
					"############D##",
					"#  -   r     ##",
					"# # ###########",
					"# #h###########",
					"#u# ###########",
					"# # ###########",
					"#h#T###########",
					"# #############",
					"# #############",
				};
				levelData.startPosition = new Vector2(1, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(12, 0, Orientation.SOUTH);
				levelData.addDoor(12, 0);
				//Treasures
				levelData.addOrientation(3, 6, Orientation.WEST);
				//helpsounds
				levelData.addSound(1, 6, "ayuda_tesoro", 4);
				levelData.addSound(3, 3, "tesoro-cerca", 6);
				levelData.addSound(2, 1, "sonidos-lados", 10);
				// levelData.addSound(6, 1, "sonido-positivo", 5);
				// levelData.addSound(8, 1, "sonido-negativo", 5);
	            break;

	        case 7:
				levelData.hallData = new string[]{
					"###D###########",
					"### ###########",
					"### ###########",
					"### #  -    ###",
					"###W#d# ###u###",
					"###h# # ### ###",
					"### # # ###   #",
					"###   #T##### #",
					"############# #",
				};
				levelData.startPosition = new Vector2(13, 8);
				/*Orientations*/
				//Treasures
				levelData.addOrientation(7, 7, Orientation.WEST);
				//Doors
				levelData.addOrientation(3, 0, Orientation.SOUTH);
				levelData.addDoor(3, 0);
				//Warps 
				levelData.addOrientation(3, 4, Orientation.SOUTH);
				levelData.addSound(3, 5, "energia-portal", 4);
	            break;
			case 8:
				levelData.hallData = new string[]{
					"############D##",
					"#X  h- r     ##",
					"##### #########",
					"###   #########",
					"###u###########",
					"##T-  #########",
					"# ###M#########",
					"#-    #########",
					"# #############",
				};
				levelData.startPosition = new Vector2(1, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(12, 0, Orientation.SOUTH);
				levelData.addDoor(12, 0);
				//Treasures
				levelData.addOrientation(2, 5, Orientation.EAST);
				//Traps
				levelData.addOrientation(1, 1, Orientation.EAST);
				levelData.addTrap(1,1);
				//Monsters
				levelData.addOrientation(5, 6, Orientation.SOUTH);
	            levelData.addMonster(5, 6);
	            // levelData.addSound(3, 7, "advertencia_monster", 8);   
				levelData.addSound(4, 1, "advertencia-trampa", 4); 
				// levelData.addSound(4, 5, "tesoro-cerca", 4); 
				break;
			case 9:
				levelData.hallData = new string[]{
					"############D##",
					"############ ##",
					"############ ##",
					"#  -     T##W##",
					"#u# ######## ##",
					"# # r ###    ##",
					"# ###M###u#####",
					"#- r - r  #####",
					"# #############",
				};
				levelData.startPosition = new Vector2(1, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(12, 0, Orientation.SOUTH);
				levelData.addDoor(12, 0);
				//Treasures
				levelData.addOrientation(9, 3, Orientation.WEST);
				//Monsters
				levelData.addOrientation(5, 6, Orientation.SOUTH);
	            levelData.addMonster(5, 6);

	            //Warps
				// levelData.addSound(4, 3, "tesoro-cerca", 4); 
				levelData.addOrientation(12, 3, Orientation.SOUTH);
				// levelData.addSound(12, 4, "energia-portal", 4);
				break;
			case 10:
				levelData.hallData = new string[]{
					"#   r      D###",
					"#u#############",
					"#W  ###  M  T##",
					"### ### #####T#",
					"### l  -   ## #",
					"####### ##    #",
					"###T### #######",
					"###-   -  l   #",
					"X h ######### #",
				};
				levelData.startPosition = new Vector2(13, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(11, 0, Orientation.WEST);
				levelData.addDoor(11, 0);
				//Treasures
				levelData.addOrientation(12, 2, Orientation.WEST);
				levelData.addOrientation(13, 3, Orientation.SOUTH);
				levelData.addOrientation(3, 6, Orientation.SOUTH);
				//Monsters
				levelData.addOrientation(9, 2, Orientation.WEST);
				levelData.addMonster(9, 2);

				//Trap
				levelData.addOrientation(0, 8, Orientation.EAST);

				//Warps 
				levelData.addOrientation(1, 2, Orientation.EAST);
				// levelData.addSound(2, 2, "energia-portal", 4);
				levelData.addSound(2, 8, "advertencia-trampa", 4); 
				break;
			case 11:
				levelData.hallData = new string[]{
					"#######D#######",
					"####### #######",
					"####### #######",
					"####### #######",
					"#######M#######",
					"####### #######",
					"####### #######",
					"####### #######",
					"####### #######",
				};
				levelData.startPosition = new Vector2(7, 8);
				/*Orientations*/
				//Monsters
				levelData.addOrientation(7, 4, Orientation.WEST);
				levelData.addMonster(7, 4);
				//Doors
				levelData.addOrientation(1, 0, Orientation.SOUTH);
				levelData.addDoor(1, 0);
	            break;
            default:
				break;
		}
		return levelData;


		// switch (n) {
		// case 1:
		// 	levelData.hallData = new string[]{
		// 		"############D##",
		// 		"# h-  hrh    ##",
		// 		"# # ###########",
		// 		"# #h###########",
		// 		"#u# ###########",
		// 		"# # ###########",
		// 		"#h#T###########",
		// 		"# #############",
		// 		"# #############",
		// 	};
		// 	levelData.startPosition = new Vector2(1, 8);
		// 	/*Orientations*/
		// 	//Doors
		// 	levelData.addOrientation(12, 0, Orientation.SOUTH);
		// 	levelData.addDoor(12, 0);
		// 	//Treasures
		// 	levelData.addOrientation(3, 6, Orientation.WEST);
		// 	//helpsounds
		// 	levelData.addSound(1, 6, "ayuda_tesoro", 4);
		// 	levelData.addSound(3, 3, "tesoro-cerca", 6);
		// 	levelData.addSound(2, 1, "sonidos-lados", 10);
		// 	levelData.addSound(6, 1, "sonido-positivo", 5);
		// 	levelData.addSound(8, 1, "sonido-negativo", 5);
  //           break;
		// case 2:
		// 	levelData.hallData = new string[]{
		// 		"############D##",
		// 		"#Xh-   r     ##",
		// 		"### ###########",
		// 		"### ###########",
		// 		"###u###########",
		// 		"##T-h #########",
		// 		"# ###M#########",
		// 		"#- h  #########",
		// 		"# #############",
		// 	};
		// 	levelData.startPosition = new Vector2(1, 8);
		// 	/*Orientations*/
		// 	//Doors
		// 	levelData.addOrientation(12, 0, Orientation.SOUTH);
		// 	levelData.addDoor(12, 0);
		// 	//Treasures
		// 	levelData.addOrientation(2, 5, Orientation.EAST);
		// 	//Traps
		// 	levelData.addOrientation(1, 1, Orientation.EAST);
		// 	levelData.addTrap(1,1);
		// 	//Monsters
		// 	levelData.addOrientation(5, 6, Orientation.SOUTH);
  //           levelData.addMonster(5, 6);
  //           levelData.addSound(3, 7, "advertencia_monster", 8);   
		// 	levelData.addSound(2, 1, "advertencia-trampa", 4); 
		// 	levelData.addSound(4, 5, "tesoro-cerca", 4); 
		// 	break;
		// case 3:
		// 	levelData.hallData = new string[]{
		// 		"############D##",
		// 		"############ ##",
		// 		"############u##",
		// 		"#  -h    T##W##",
		// 		"#u# ########h##",
		// 		"# # r ###    ##",
		// 		"# ###M###u#####",
		// 		"#- r -    #####",
		// 		"# #############",
		// 	};
		// 	levelData.startPosition = new Vector2(1, 8);
		// 	/*Orientations*/
		// 	//Doors
		// 	levelData.addOrientation(12, 0, Orientation.SOUTH);
		// 	levelData.addDoor(12, 0);
		// 	//Treasures
		// 	levelData.addOrientation(9, 3, Orientation.WEST);
		// 	//Monsters
		// 	levelData.addOrientation(5, 6, Orientation.SOUTH);
  //           levelData.addMonster(5, 6);

  //           //Warps
		// 	levelData.addSound(4, 3, "tesoro-cerca", 4); 
		// 	levelData.addOrientation(12, 3, Orientation.SOUTH);
		// 	levelData.addSound(12, 4, "energia-portal", 4);
		// 	break;
		// case 4:
		// 	levelData.hallData = new string[]{
		// 		"#   r      D###",
		// 		"#u#############",
		// 		"#Wh ###  M  T##",
		// 		"### ### #####T#",
		// 		"### l  -   ## #",
		// 		"####### ##    #",
		// 		"###T### #######",
		// 		"###-   -  l   #",
		// 		"X h ######### #",
		// 	};
		// 	levelData.startPosition = new Vector2(13, 8);
		// 	/*Orientations*/
		// 	//Doors
		// 	levelData.addOrientation(11, 0, Orientation.WEST);
		// 	levelData.addDoor(11, 0);
		// 	//Treasures
		// 	levelData.addOrientation(12, 2, Orientation.WEST);
		// 	levelData.addOrientation(13, 3, Orientation.SOUTH);
		// 	levelData.addOrientation(3, 6, Orientation.SOUTH);
		// 	//Monsters
		// 	levelData.addOrientation(9, 2, Orientation.WEST);
		// 	levelData.addMonster(9, 2);

		// 	//Trap
		// 	levelData.addOrientation(0, 8, Orientation.EAST);

		// 	//Warps 
		// 	levelData.addOrientation(1, 2, Orientation.EAST);
		// 	levelData.addSound(2, 2, "energia-portal", 4);
		// 	levelData.addSound(2, 8, "advertencia-trampa", 4); 
		// 	break;
		// default:
		// 	break;
		// }
		return levelData;
	}

    public void removeEntity(int i, int j)
    {
        StringBuilder sb = new StringBuilder(hallData[j]);
        sb[i] = ' ';
        hallData[j]= sb.ToString();
    }
    
    public float getMinDistanceMonster(Vector2 pos)
    {
        float minDistance = 10000000.0f;
        foreach(Vector2 v in monsters)
        {
            float aux = Vector2.Distance(v, pos);
            if (aux<minDistance)
            {
                minDistance = aux;
            }
        }

        return minDistance;
    } 

	public float getMinDistanceTrap(Vector2 pos)
	{
		float minDistance = 10000000.0f;
		foreach(Vector2 v in traps)
		{
			float aux = Vector2.Distance(v, pos);
			if (aux<minDistance)
			{
				minDistance = aux;
			}
		}
		
		return minDistance;
	} 
	
	public float getDoorDistance(Vector2 pos)
	{
		return Vector2.Distance (door, pos);
	}
}

