using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// The GameManager script is gonna just be a spaghetti mess where it handles the program loop and the gameplay loop.
/// The game is simple enough that it shouldn't matter.
/// </summary>

public class GameManager : MonoBehaviour
{


	#region ChildObjects and their Scripts
	[SerializeField] GameObject GameManger;	
	[SerializeField] GameObject GameCamera; //Added in inspector
	[SerializeField] GameObject UIManager;  //Added in inspector
											//[SerializeField] GameObject MapManager; Handled in this script
	[SerializeField] UIManager UIManagerScript;
	#endregion

	#region Timer Items
	[SerializeField] int startingTimeMinutes;   //time at the start of the game in seconds	
	public int MinutesRemaining;
	public int SecondsRemaining;
	[SerializeField] float passedSeconds;
	[SerializeField] bool crunchTime;			//Added this. Figured I can up the player speed in the last 30 seconds without telling the player.
												//I just want a slight bump for those last second moments.
	#endregion

	#region Map Items
	[SerializeField] GameObject gameMap;    //object that holds the map tiles
	public GameObject[,] MapTiles = new GameObject[0, 0];
	

	//Tracked Items
	[SerializeField] List<GameObject> graves= new List<GameObject>();
	[SerializeField] List<GameObject> spawns= new List<GameObject>();
	[SerializeField] List<GameObject> possibleSpawns = new List<GameObject>();
	[SerializeField] GameObject activeSummonSpawn;
	[SerializeField] GameObject player;
	[SerializeField] PlayerController playerController;


	//Tiles
	[SerializeField] GameObject GrassTile;
	[SerializeField] GameObject GraveTile;
	[SerializeField] GameObject WalkwayTile;
	[SerializeField] GameObject BorderTile;
	[SerializeField] GameObject SpawnTile;

	//other prefabs
	[SerializeField] GameObject PlayerPrefab;
		
	
	bool MapGenerated;
	[SerializeField] Texture2D mapPNG;  //The map texture
	#endregion

	#region NPCs
	[SerializeField] GameObject NPCPrefab;
	[SerializeField] GameObject ActiveNPC;
	[SerializeField] NPCScript NPCScript;
	[SerializeField] List<GameObject> PossibleNPCs = new List<GameObject>();
	#endregion

	#region Body Parts
	[SerializeField] GameObject BodyPartPrefab;
	#endregion

	[SerializeField] enum programState {Title, MainGame}
	[SerializeField] programState currentProgramState;

	//Awake contains the Don't Destroy on Load command. It'll keep this script running the whole time.
	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		MapGenerated= false;
	}


	// Start is called before the first frame update
	void Start()
	{
		currentProgramState = programState.Title;
		Debug.Log(SceneManager.GetActiveScene().name);
		
	}

	// Update is called once per frame
	void Update()
	{		
		switch(currentProgramState)
		{
			case programState.Title:
				TitlePageScreen();
				break;
			case programState.MainGame:
				//Start Sequence

				//MainGameLoop
				cameraUpdate();
				playerController.PokemonController();
				
				TimerSystem();
				UIManagerScript.UpdateTimer(MinutesRemaining,SecondsRemaining);
				//Win/Fail

				//testing
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					ReturnToTitle();
					//invoke the failed state
					//load the title
				}
				break;
		}
	}

	//Timer
	void TimerSystem()
	{
		passedSeconds += Time.deltaTime;
		if (passedSeconds >= 1)
		{
			passedSeconds =0;
			SecondsRemaining -= 1;
			if (SecondsRemaining < 0 && MinutesRemaining >= 1)
			{
				SecondsRemaining = 59;
				MinutesRemaining -= 1;
			}
			else
			{
				//Game Over!
			}
		}
		if(MinutesRemaining == 0 && SecondsRemaining <= 30 && crunchTime == false)
		{
			crunchTime = true;
		}
		else if(MinutesRemaining >= 0 && SecondsRemaining > 45 && crunchTime == true)
		{
			crunchTime= false;
		}
	}

	#region TitleScreen Specific Code
	void TitlePageScreen()
	{
		if (MapGenerated == false)
		{
			MapGenerator(); //Generate the Map. This map will follow to the next screen
			MapGenerated = true;
		}
		
		//for the start it will linger until the player presses "enter" or "return"
		if(Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			NewGame();	//Load the next scene
		}
	}

	void titleSetUp()
	{
		MapGenerated= false;

		//testing deleting the map
		for (int y = 0; y < MapTiles.GetLength(1); y++)
		{
			for (int x = 0; x < MapTiles.GetLength(0); x++)
			{
				Destroy(MapTiles[x,y]);
			}
		}
		//Start playing title music?

	}

	#endregion

	#region MainGame Specific Code
	void NewGameSetUp()
	{
		Time.timeScale = 0; //Stop the clock
		MinutesRemaining = startingTimeMinutes; //set timer
		SecondsRemaining = 0;
		UIManagerScript = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
		UIManagerScript.UpdateTimer(MinutesRemaining, SecondsRemaining);	//update the timer
		gameMap.SetActive(true);
		player = Instantiate(PlayerPrefab, new Vector3(activeSummonSpawn.transform.position.x, activeSummonSpawn.transform.position.y, -1), Quaternion.identity);
		playerController = player.GetComponent<PlayerController>();
		playerController.GameManager = this;
		GameCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y,-3); //Move camera to active spawn
		//run dialogue from client
		//TEST
		Time.timeScale = 1;
		//TEST
		Debug.Log(SceneManager.GetActiveScene().name);
	}
	void nextSummonPositionSpawn()
	{
		//I'd like to make sure the spawns don't repeat themselves
	}

	//Follows the player
	void cameraUpdate()
	{
		GameCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y,-3);
	}
	
	#endregion

	#region Map Generation
	void MapGenerator()
	{
		mapPNG = Resources.Load<Texture2D>("Maps/testMap");	//Grab the provincemap
		MapTiles = new GameObject[mapPNG.width,mapPNG.height];

		for (int y = 0; y < mapPNG.height;y++)
		{
			for (int x = 0; x < mapPNG.width; x++)
			{
				Color thisPixel = mapPNG.GetPixel(x, y);
				instantiater(thisPixel,x,y);	//make the tile based on the RGB value
			}
		}
		activeSummonSpawn = spawns[UnityEngine.Random.Range(0, spawns.Count)];//pick the first spawn loaction
															  //Set up the body locations
		Debug.Log($"Map Initialization ended at:{DateTime.Now}");
		gameMap.SetActive(false);       //set the map to inactive
	}

	//The below reads the item and with the rgb value, spawns the necessary item
	void instantiater(Color pixelColor, int x, int y)
	{
		int r = (int)(pixelColor.r * 255);	//needs to be convereted to 255 color
		int g = (int)(pixelColor.g * 255);	
		int b = (int)(pixelColor.b * 255);

		GameObject newGameObject;

		if(r==255 && g==255 && b==255)
		{
			newGameObject = Instantiate(GrassTile, new Vector3(x, y), Quaternion.identity, gameMap.transform);
		}
		else if(r == 38 && g == 127 && b == 0)
		{
			newGameObject = Instantiate(BorderTile, new Vector3(x, y), Quaternion.identity, gameMap.transform);	//Border
		}
		else if(r == 64 && g == 64 && b == 64)
		{
			newGameObject = Instantiate(GraveTile, new Vector3(x, y), Quaternion.identity, gameMap.transform);	//Graves
			graves.Add(newGameObject);
		}
		else if (r == 255 && g == 0 && b == 0)
		{
			newGameObject = Instantiate(SpawnTile, new Vector3(x, y), Quaternion.identity, gameMap.transform);	//Spwan
			spawns.Add(newGameObject);
		}
		else
		{
			newGameObject = Instantiate(GrassTile, new Vector3(x, y), Quaternion.identity, gameMap.transform);	//The default is just grass
		}
		
		MapTiles[x, y] = newGameObject;
		newGameObject.name = $"Tile{x}.{y}";
	}
	#endregion


	#region Scene Management
	public void NewGame()
	{
		Action load = delegate () { NewGameSetUp(); };
		StartCoroutine(AsyncLoader("MainGame", load));
		currentProgramState = programState.MainGame;
	}

	public void ReturnToTitle()
	{
		Action load = delegate () { titleSetUp(); };
		StartCoroutine(AsyncLoader("TitleScreen", load));
		currentProgramState = programState.Title;
	}	
	

	//Asynchronous loaded I think I got in the documentation or stack overflow
	IEnumerator AsyncLoader(string SceneName, Action SetUpMethod)
	{
		// The Application loads the Scene in the background as the current Scene runs.
		// This is particularly good for creating loading screens.
		// You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
		// a sceneBuildIndex of 1 as shown in Build Settings.

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);


		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
		//Everything below here loads after the Coroutine
		SetUpMethod();
	}
	#endregion
}
