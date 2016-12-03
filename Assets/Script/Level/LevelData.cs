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
	private Dictionary<string, int> inits;
	private Vector2 door;
    private ArrayList monsters;
	private ArrayList traps;

	public LevelData(){
		orientations = new Dictionary<string, Orientation> ();
        monsters = new ArrayList();
		traps = new ArrayList();
		sounds = new Dictionary<string, string> ();
		soundsDuration = new Dictionary<string, float> ();
		inits = new Dictionary<string, int> ();
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

	public void addInit(int i, int j, int n) {
		inits.Add (i + "," + j, n);
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

	public int getInitAt(int i, int j) {
		return inits [i + "," + j];
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
		case 'C':
			return Entity.CARTESIAN;
		default:
			return Entity.WALL;
		}
	}

	public static LevelData getLevel(int n){
		LevelData levelData = new LevelData ();

		// Express
		switch (n) {
			case 1:
				levelData.hallData = new string[]{
					"#D#############",
					"#     l-     ##",
					"#######u####u##",
					"#######  -   ##",
					"#########h#####",
					"#   hr  h #####",
					"#h#############",
					"#h#############",
					"# #############",
				};
				levelData.startPosition = new Vector2(1, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(1, 0, Orientation.SOUTH);
				levelData.addDoor(1, 0);
				// help
				levelData.addSound(1, 6, "ayuda-first-rotation", 16);
				levelData.addSound(1, 7, "ayuda-brujula", 16);
				levelData.addSound(4, 5, "sonido-positivo", 4);
				levelData.addSound(8, 5, "ayuda-touch-and-ask", 16);
				levelData.addSound(9, 4, "ayuda-first-cubo", 17);
	            break;
	        case 2:
				levelData.hallData = new string[]{
					"#D#############",
					"# ###########X#",
					"#-l  W####### #",
					"# ### #######h#",
					"# W l-  l-    #",
					"######### #####",
					"######### #####",
					"#  h W  r #####",
					"# #############",
				};
				levelData.startPosition = new Vector2(1, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(1, 0, Orientation.SOUTH);
				levelData.addDoor(1, 0);
				//Traps
				levelData.addOrientation(13, 1, Orientation.SOUTH);
				levelData.addTrap(13,1);
				// help
				levelData.addSound(13, 3, "advertencia-trampa", 4);
				levelData.addSound(3, 7, "energia-portal", 4);
				//Warps 
				// 1
				levelData.addOrientation(5, 7, Orientation.WEST);
				levelData.addInit(5,7,0);
				// 2
				levelData.addOrientation(2, 4, Orientation.EAST);
				levelData.addInit(2,4,1);
				// 3
				levelData.addOrientation(5, 2, Orientation.SOUTH);
				levelData.addInit(5,2,2);
	            break;
	       case 3:
				levelData.hallData = new string[]{
					"###D###########",
					"### ###########",
					"### ###########",
					"### C lW - h T#",
					"######### #####",
					"#########W#####",
					"######  r #####",
					"###### ########",
					"###### ########",
				};
				levelData.startPosition = new Vector2(6, 8);
				/*Orientations*/
				//Cartesian
				levelData.addOrientation(4, 3, Orientation.EAST);
				//Doors
				levelData.addOrientation(3, 0, Orientation.SOUTH);
				levelData.addDoor(3, 0);
				//help
				levelData.addSound(11, 3, "ayuda_tesoro", 4);
				//Treasures
				levelData.addOrientation(13, 3, Orientation.WEST);
				// Warps
				// 4
				levelData.addOrientation(9, 5, Orientation.SOUTH);
				levelData.addInit(9,5,3);
				// 5
				levelData.addOrientation(7, 3, Orientation.EAST);
				levelData.addInit(7,3,4);
	            break;

	        case 4:
				levelData.hallData = new string[]{
					"#D#############",
					"#   W l-  - l #",
					"####### ## ## #",
					"######  ##T##W#",
					"######u###### #",
					"#T####  W - W #",
					"# ######## ####",
					"#-  W   r  ####",
					"# #############",
				};
				levelData.startPosition = new Vector2(1, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(1, 0, Orientation.SOUTH);
				levelData.addDoor(1, 0);
				//Treasures
				levelData.addOrientation(10, 3, Orientation.NORTH);
				levelData.addOrientation(1, 5, Orientation.SOUTH);

				// 6
				levelData.addOrientation(4, 7, Orientation.WEST);
				levelData.addInit(4,7,5);
				// 7
				levelData.addOrientation(12, 5, Orientation.EAST);
				levelData.addInit(12,5,6);
				// 8
				levelData.addOrientation(8, 5, Orientation.WEST);
				levelData.addInit(8,5,7);
				// 9
				// levelData.addOrientation(10, 3, Orientation.NORTH);
				// levelData.addInit(10,3,8);
				// 10
				levelData.addOrientation(13, 3, Orientation.SOUTH);
				levelData.addInit(13,3,9);
				// 11
				levelData.addOrientation(4, 1, Orientation.EAST);
				levelData.addInit(4,1,10);
	            break;

	        case 5:
				levelData.hallData = new string[]{
					"#D     W  T   #",
					"############# #",
					"#   W  - r  # #",
					"#u##### ### # #",
					"# #####M### #W#",
					"#W#####  r -# #",
					"# ######### # #",
					"# W  ######d#u#",
					"#### ######   #",
				};
				levelData.startPosition = new Vector2(4, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(1, 0, Orientation.EAST);
				levelData.addDoor(1, 0);
				// Treasures
				levelData.addOrientation(10, 0, Orientation.EAST);
				//Monsters
				levelData.addOrientation(7, 4, Orientation.NORTH);
				levelData.addMonster(7, 4);
				// Warps
				// 12
				levelData.addOrientation(2, 7, Orientation.EAST);
				levelData.addInit(2,7,11);
				// 13
				levelData.addOrientation(1, 5, Orientation.SOUTH);
				levelData.addInit(1,5,12);
				// 14
				levelData.addOrientation(4, 2, Orientation.WEST);
				levelData.addInit(4,2,13);
				// 15
				levelData.addOrientation(7, 0, Orientation.EAST);
				levelData.addInit(7,0,14);
				// 16
				levelData.addOrientation(13, 4, Orientation.SOUTH);
				levelData.addInit(13,4,15);
	            break;

	   //      case 6:
	   //      	levelData.hallData = new string[]{
				// 	"#######D#######",
				// 	"####### #######",
				// 	"####### #######",
				// 	"##      #######",
				// 	"## ############",
				// 	"##T############",
				// 	"## ############",
				// 	"##      #######",
				// 	"####### #######",
				// };
				// levelData.startPosition = new Vector2(7, 8);
				// /*Orientations*/
				// //Doors
				// levelData.addOrientation(7, 0, Orientation.SOUTH);
				// levelData.addDoor(7, 0);
				// // Treasures
				// levelData.addOrientation(2, 5, Orientation.SOUTH);
	   //          break;

	   //      case 7:
	   //      	levelData.hallData = new string[]{
				// 	"#######D#######",
				// 	"####### #######",
				// 	"####### #######",
				// 	"#######      ##",
				// 	"############ ##",
				// 	"############T##",
				// 	"############ ##",
				// 	"#######      ##",
				// 	"####### #######",
				// };
				// levelData.startPosition = new Vector2(7, 8);
				// /*Orientations*/
				// //Doors
				// levelData.addOrientation(7, 0, Orientation.SOUTH);
				// levelData.addDoor(7, 0);
				// // Treasures
				// levelData.addOrientation(12, 5, Orientation.SOUTH);
	   //          break;

			case 6:
				levelData.hallData = new string[]{
					"##D############",
					"## ############",
					"## ############",
					"## ############",
					"## hM     hW###",
					"###########W###",
					"###########W###",
					"########### ###",
					"########### ###",
				};
				levelData.startPosition = new Vector2(11, 8);
				/*Orientations*/
				//Monsters
				levelData.addOrientation(4, 4, Orientation.EAST);
				levelData.addMonster(4, 4);
				//Doors
				levelData.addOrientation(2, 0, Orientation.SOUTH);
				levelData.addDoor(2, 0);
				//help
				levelData.addSound(10, 4, "last_battle_inc", 11);
				levelData.addSound(3, 4, "final", 6);
				// Warps
				// 17
				levelData.addOrientation(11, 6, Orientation.SOUTH);
				levelData.addInit(11,6,16);
				// 18
				levelData.addOrientation(11, 5, Orientation.SOUTH);
				levelData.addInit(11,5,17);
				// 19
				levelData.addOrientation(11, 4, Orientation.SOUTH);
				levelData.addInit(11,4,18);
	            break;
            default:
				break;
		}
		return levelData;


		// Normal
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
					"######      ###",
					"######h########",
					"###### ########",
					"###### ########",
				};
				levelData.startPosition = new Vector2(6, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(6, 0, Orientation.SOUTH);
				levelData.addDoor(6, 0);
				//help
				levelData.addSound(6, 6, "continue", 7);
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
				levelData.addSound(10, 3, "sonido-positivo", 4);
				levelData.addSound(8, 3, "sonido-negativo", 4);
	            break;

	        case 5:
				levelData.hallData = new string[]{
					"#######D#######",
					"####### #######",
					"#######u#######",
					"###    -    ###",
					"###u#######u###",
					"###     -   ###",
					"########h######",
					"########   ####",
					"########## ####",
				};
				levelData.startPosition = new Vector2(10, 8);
				/*Orientations*/
				//Doors
				levelData.addOrientation(7, 0, Orientation.SOUTH);
				levelData.addDoor(7, 0);
				//help
				levelData.addSound(8, 6, "ayuda-first-cubo", 17);
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
					"#######h#######",
					"#######M#######",
					"####### #######",
					"####### #######",
					"#######h#######",
					"####### #######",
				};
				levelData.startPosition = new Vector2(7, 8);
				/*Orientations*/
				//Monsters
				levelData.addOrientation(7, 4, Orientation.SOUTH);
				levelData.addMonster(7, 4);
				//Doors
				levelData.addOrientation(7, 0, Orientation.SOUTH);
				levelData.addDoor(7, 0);
				//help
				levelData.addSound(7, 7, "last_battle_inc", 11);
				levelData.addSound(7, 3, "final", 6);
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

