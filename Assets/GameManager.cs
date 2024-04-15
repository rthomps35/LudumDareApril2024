using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The GameManager script is gonna just be a spaghetti mess where it handles the program loop and the gameplay loop.
/// The game is simple enough that it shouldn't matter.
/// </summary>

public class GameManager : MonoBehaviour
{
	bool lostGame = false;

	#region ChildObjects and their Scripts
	[SerializeField] GameObject GameManger;	
	[SerializeField] GameObject GameCamera; //Added in inspector
	[SerializeField] GameObject UIManager;  //Added in inspector

	[SerializeField] UIManager UIManagerScript;
	
	#endregion

	#region Timer Items
	[SerializeField] int startingTimeMinutes;   //time at the start of the game in seconds	
	public int MinutesRemaining;
	public int SecondsRemaining;
	public float passedSeconds;
	[SerializeField] bool crunchTime;           //Added this. Figured I can up the player speed in the last 30 seconds without telling the player.
												//I just want a slight bump for those last second moments.
	public int TotalGameSeconds;
	bool timerActive = false;
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
	[SerializeField] GameObject Tile;
	[SerializeField] GameObject GraveObject;
	[SerializeField] GameObject WalkwayObject;
	[SerializeField] GameObject BorderObject;
	[SerializeField] GameObject SpawnObject;
	[SerializeField] GameObject TombstoneObject;

	//other prefabs
	[SerializeField] GameObject PlayerPrefab;
		
	
	bool MapGenerated;
	[SerializeField] Texture2D mapPNG;  //The map texture
	#endregion

	#region NPCs
	[SerializeField] GameObject NPCPrefab;
	[SerializeField] GameObject ActiveNPC;
	[SerializeField] NPCScript NPCScript;
	[SerializeField] List<GameObject> NPCsQueue = new List<GameObject>();
	//I need a queue for the 
	//NPC index

	//IndividualNPCS
	public GameObject CurrentNPC;
	[SerializeField] GameObject TutorialMan;
	[SerializeField] GameObject Romeo;
	//
	//
	//
	//

	#endregion

	#region Body Parts
	[SerializeField] GameObject BodyPartPrefab;
	public int BodyScore;

	[SerializeField] int Type1PartScore;
	[SerializeField] int Type2PartScore;
	[SerializeField] int Type3PartScore;

	public enum BodyPart { Head, Arms, Torso, Legs}
	public BodyPart SelectedPart;

	//Sprites
	[SerializeField] Sprite Type1Tombstone;
	[SerializeField] Sprite Type2Tombstone; 
	[SerializeField] Sprite Type3Tombstone;
	public Sprite Type1Head;
	public Sprite Type1Torso;
	public Sprite Type1RightArm;
	public Sprite Type1LeftArm;
	public Sprite Type1RightLeg;
	public Sprite Type1LeftLeg;
	public Sprite Type2Head;
	public Sprite Type2Torso;
	public Sprite Type2RightArm;
	public Sprite Type2LeftArm;
	public Sprite Type2RightLeg;
	public Sprite Type2LeftLeg;
	public Sprite Type3Head;
	public Sprite Type3Torso;
	public Sprite Type3RightArm;
	public Sprite Type3LeftArm;
	public Sprite Type3RightLeg;
	public Sprite Type3LeftLeg;

	bool hasHead;
	bool hasTorso;
	bool hasArms;
	bool hasLegs;

	//Lists
	List<GameObject> AllGraves = new List<GameObject>();
	List<GameObject> Type1Graves = new List<GameObject>();
	List<GameObject> Type2Graves = new List<GameObject>();
	List<GameObject> Type3Graves = new List<GameObject>();
	#endregion

	[SerializeField] enum programState {Title, MainGame}
	[SerializeField] programState currentProgramState;

	#region Audio
	//This is the point where i've accepted my italian heritage and I'm cooking spaghetti
	public AudioSource AudioManager;
	

	//sounds
	[SerializeField] AudioClip MainMusic;
	public AudioClip DigNoise;
	#endregion

	//summon trigger
	bool summoningActive;
	[SerializeField] Sprite activeSummoningSprite;
	[SerializeField] Sprite inactiveSummoningSprite;
	

	[SerializeField]Sprite PHsprite;


	public List<GameObject> NPCS= new List<GameObject>();



	//Scene Set Ups
	bool NPCToIntroduce = true;
	//bool mainGameSet = false;
	bool TextSent = false;	//This is so the introductions work

	//Awake contains the Don't Destroy on Load command. It'll keep this script running the whole time.
	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		MapGenerated= false;
		UIManagerScript.UIMode();
		Time.timeScale = 1;
	}


	// Start is called before the first frame update
	void Start()
	{
		Debug.Log(Time.timeScale);
		currentProgramState = programState.Title;
		Debug.Log(SceneManager.GetActiveScene().name);
	}

	// Update is called once per frame
	void Update()
	{	
		ProgramLoop();
		TicLoop();
		//Loss Check
	}

	void ProgramLoop()
	{
		//The first section is run regardless of tic
		switch (currentProgramState)
		{
			case programState.Title:
				TitlePageScreen();
				break;
			case programState.MainGame:
				if(NPCToIntroduce == true)	//new NPC?
				{
					timerActive = false;
					playerController.CanMove = false;
					if (UIManagerScript.textQueue.Count == 0 && TextSent == true)
					{
						NPCToIntroduce = false;
						playerController.CanMove = true;
						timerActive = true;
					}

				}
				//MainGameLoop
				cameraUpdate(); //Move the camera to follow the player

				//player
				playerController.PokemonController();
				
				if(player.transform.position == activeSummonSpawn.transform.position && summoningActive == true)
				{
					//Circle Checck
					//Deliver body
					//BodyScore = 0;
					//Add Time
					//UIManagerScript.UpdateUIEachSecond(MinutesRemaining,SecondsRemaining);	//Update Clock
					//Introduce next
					//Wait for circle text
					Debug.Log("You are in an active circle");
				}


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
	void TicLoop()
	{
		//This is my strategy mental illness coming out
		//Its a time/tic based game loop similar to Paradox Games
		//Items that last x amount of time will be here.
		//You could use a coroutine but this is easier to control
		//All once per second items should be below
		passedSeconds += Time.deltaTime;
		if (passedSeconds >= 1)
		{
			passedSeconds = 0;
			TotalGameSeconds += 1;
			
			//Timer
			if(timerActive==true)
			{
				GameTimer();
			}
			
			//Tic Specific Action
			//PlayerTic Actions
			if (player != null)
			{
				playerController.PlayerTicActions();
			}
			
			//Summoning Animation
			if(BodyScore > 0 && summoningActive == false)
			{
				summoningActive = true; //summoning active
				SpriteRenderer aSSSR =  activeSummonSpawn.GetComponent<SpriteRenderer>();
				aSSSR.sprite = activeSummoningSprite;
			}

			//UI
			UIManagerScript.UpdateUIEachSecond(MinutesRemaining,SecondsRemaining);
			//TextUpdate


			//The below is crunchtime, this should be a method
			if (MinutesRemaining == 0 && SecondsRemaining <= 30 && crunchTime == false)
			{
				crunchTime = true;
			}
			else if (MinutesRemaining >= 0 && SecondsRemaining > 45 && crunchTime == true)
			{
				crunchTime = false;
			}
		}
	}

	void GameTimer()
	{
		SecondsRemaining -= 1;
		if (SecondsRemaining < 0 && MinutesRemaining >= 1)
		{
			SecondsRemaining = 59;
			MinutesRemaining -= 1;
		}
		else
		{
			lostGame = true;
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
		
		MinutesRemaining = startingTimeMinutes; //set timer
		SecondsRemaining = 0;
		UIManagerScript = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
		UIManagerScript.UpdateUIEachSecond(MinutesRemaining, SecondsRemaining);	//update the timer
		gameMap.SetActive(true);
		//player = Instantiate(PlayerPrefab, new Vector3(activeSummonSpawn.transform.position.x, activeSummonSpawn.transform.position.y, -1), Quaternion.identity);
		player.transform.position = new Vector2(activeSummonSpawn.transform.position.x, activeSummonSpawn.transform.position.y);
		player.SetActive(true);
		//playerController = player.GetComponent<PlayerController>();
		playerController.GameManager = this;
		GameCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y,-3); //Move camera to active spawn
		UIManagerScript.UIMode("MainGame");
		currentProgramState = programState.MainGame;
		NPCToIntroduce = true;
		CurrentNPC = TutorialMan;
		MainGameStart();
		Debug.Log(SceneManager.GetActiveScene().name);
	}

	void MainGameStart()
	{
		//animate the spawn
		CurrentNPC = TutorialMan;
		IntroduceAnNPC(TutorialMan); //Introduce Tutorial
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
				GameObject newTile = Instantiate(Tile, new Vector2(x, y), Quaternion.identity, gameMap.transform);
				objectInstantiater(thisPixel,x,y, newTile);    //make the tile based on the RGB value
				MapTiles[x, y] = newTile;
				newTile.name = $"Tile{x}.{y}";
			}
		}
		activeSummonSpawn = spawns[UnityEngine.Random.Range(0, spawns.Count)];	//pick the first spawn loaction
		GraveGeneration();														//Set up the body locations
		Debug.Log($"Map Initialization ended at:{DateTime.Now}");
		gameMap.SetActive(false);       //set the map to inactive
	}

	//The below reads the item and with the rgb value, spawns the necessary object ontop of the tile
	void objectInstantiater(Color pixelColor, int x, int y, GameObject tile)
	{
		int r = (int)(pixelColor.r * 255);	//needs to be convereted to 255 color
		int g = (int)(pixelColor.g * 255);	
		int b = (int)(pixelColor.b * 255);

		GameObject newGameObject;
		TileScript tileScript = tile.GetComponent<TileScript>();

		if(r == 38 && g == 127 && b == 0)
		{
			newGameObject = Instantiate(BorderObject, new Vector3(x, y), Quaternion.identity, gameMap.transform);	//Border
			tileScript.Walkable = false;
			tileScript.ObjectOnTile = newGameObject;
		}
		else if(r == 64 && g == 64 && b == 64)
		{
			newGameObject = Instantiate(GraveObject, new Vector3(x, y), Quaternion.identity, gameMap.transform);	//Graves
			AllGraves.Add(newGameObject);
			tileScript.Walkable = true;
			tileScript.ObjectOnTile = newGameObject;
		}
		else if (r == 255 && g == 0 && b == 0)
		{
			newGameObject = Instantiate(SpawnObject, new Vector3(x, y), Quaternion.identity, gameMap.transform);	//Spwan
			spawns.Add(newGameObject);
			tileScript.Walkable = true;
			tileScript.ObjectOnTile = newGameObject;
		}
		else if(r == 0 && g == 0 && b == 0)
		{
			newGameObject = Instantiate(TombstoneObject, new Vector3(x, y), Quaternion.identity, gameMap.transform);    //Graves
			tileScript.Walkable = false;
			tileScript.ObjectOnTile = newGameObject;
			TileScript ts = MapTiles[x,y-1].GetComponent<TileScript>();    //link to nearby grave
			GraveScript gs = ts.ObjectOnTile.GetComponent<GraveScript>();
			gs.Tombstone = newGameObject;
		}
		else
		{
			tileScript.Walkable = true;
		}
		
		
		
	}

	void GraveGeneration()
	{
		int totalGraves = AllGraves.Count;
		Debug.Log($"Total Graves:{totalGraves}");
		//Body randomization
		while(Type1Graves.Count < (int)(totalGraves * .5))
		{
			int index = UnityEngine.Random.Range(0, AllGraves.Count - 1);
			GameObject grave = AllGraves[index];
			Type1Graves.Add(grave);
			AllGraves.Remove(grave);
			GraveScript gs = grave.GetComponent<GraveScript>();
			SpriteRenderer tombstone = gs.Tombstone.GetComponent<SpriteRenderer>();
			tombstone.sprite = Type1Tombstone;
			gs.Type = GraveScript.GraveType.Type1; //body instantiation
			
		}
		Debug.Log($"Type1 Graves:{Type1Graves.Count}");
		while (Type2Graves.Count < (int)(totalGraves*.35))
		{
			int index = UnityEngine.Random.Range(0, AllGraves.Count - 1);
			GameObject grave = AllGraves[index];
			Type2Graves.Add(grave);
			AllGraves.Remove(grave);
			GraveScript gs = grave.GetComponent<GraveScript>();
			SpriteRenderer tombstone = gs.Tombstone.GetComponent<SpriteRenderer>();
			tombstone.sprite = Type2Tombstone;
			gs.Type = GraveScript.GraveType.Type2; //body instantiation
		}
		Debug.Log($"Type2 Graves:{Type2Graves.Count}");
		int remainingGraves = AllGraves.Count;
		Debug.Log(remainingGraves);
		while (Type3Graves.Count < remainingGraves)
		{
			int index = 0;
			GameObject grave = AllGraves[index];
			Type3Graves.Add(grave);
			AllGraves.Remove(grave);
			GraveScript gs = grave.GetComponent<GraveScript>();
			SpriteRenderer tombstone = gs.Tombstone.GetComponent<SpriteRenderer>();
			tombstone.sprite = Type3Tombstone;
			gs.Type = GraveScript.GraveType.Type3; //body instantiation
			index ++;	
		}
		Debug.Log($"Type3 Graves:{Type3Graves.Count}");
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
	
	#region Text Specific
	void IntroduceAnNPC(GameObject NPC)
	{

		NPCScript NPCS = NPC.GetComponent<NPCScript>();
		
		if (UIManagerScript.textQueue.Count == 0 && TextSent == true)
		{
			NPCS.Introduced = true;
			playerController.CanMove = true;
			timerActive = true;
		}
		else if(NPCS.Introduced == false)
		{
			UIManagerScript.BasicTextWriter(NPC, NPCS.NPCSprite, NPCS.IntroductionLines);
			TextSent= true;
		}
		
	}

	void CheckIfIntroduced(GameObject NPC)
	{

	}

	#endregion
	#region BodySpecific
	void ClearBody()
	{
		hasHead= false;
		hasTorso= false;
		hasLegs= false;
		hasArms= false;
		BodyScore = 0;
		UIManagerScript.UIBodyClear();
	}

	public void PullBody(GraveScript gs)
	{
		bool partReceived = false;
		//Check
		while (partReceived == false)
		{
			int dieRoll = UnityEngine.Random.Range(1,4);
			Debug.Log(dieRoll);
			if (hasHead == false && dieRoll == 1)
			{
				hasHead = true;
				UIManagerScript.UIAddPart(BodyPart.Head, gs.Type);
				partReceived = true;
				Debug.Log("Got a head!");
			}
			else if (hasTorso == false && dieRoll == 2)
			{
				hasTorso = true;
				UIManagerScript.UIAddPart(BodyPart.Torso, gs.Type);
				partReceived = true;
				Debug.Log("Got a torso!");
			}
			else if(hasArms == false && dieRoll == 3)
			{
				hasArms = true;
				UIManagerScript.UIAddPart(BodyPart.Arms, gs.Type);
				partReceived = true;
				Debug.Log("Got arms");
			}
			else if(hasLegs == false && dieRoll == 3)
			{
				//legs
				hasLegs = true;
				UIManagerScript.UIAddPart(BodyPart.Legs, gs.Type);
				partReceived = true;
				Debug.Log("Got Legs!");
			}
		}
		CalculatePartScore(gs.Type);	//calculate score
		

	}

	void CalculatePartScore(GraveScript.GraveType Type)
	{
		switch(Type)
		{
			case(GraveScript.GraveType.Type1):
				BodyScore += Type1PartScore;
				break;
			case (GraveScript.GraveType.Type2):
				BodyScore += Type2PartScore;
				break;
			case (GraveScript.GraveType.Type3):
				BodyScore += Type3PartScore;
				break;
		}
	}

	#endregion 
}
