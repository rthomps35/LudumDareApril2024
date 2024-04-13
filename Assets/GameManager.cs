using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// The GameManager script is gonna just be a spaghetti mess where it handles the program loop and the gameplay loop.
/// The game is simple enough that it shouldn't matter.
/// </summary>

public class GameManager : MonoBehaviour
{
	

	#region ChildObjects and their Scripts
	[SerializeField] GameObject GameCamera; //Added in inspector
	[SerializeField] GameObject UIManager;  //Added in inspector
	//[SerializeField] GameObject MapManager; Handled in this script
	#endregion

	#region Timer Items
	public int TimeRemaining;
	[SerializeField] float passedSeconds;
	#endregion

	#region Map Items
	[SerializeField] int mapWidth;	//may mpt be meeded
	[SerializeField] int mapHeight;	//may not be needed
	[SerializeField] GameObject gameMap;	//object that holds the map tiles
	//Tiles
	[SerializeField] GameObject GrassTile;
	[SerializeField] GameObject GraveTile;
	[SerializeField] GameObject WalkwayTile;
	[SerializeField] GameObject BorderTile;
	[SerializeField] GameObject SpawnTile;

	[SerializeField] Texture2D mapPNG;  //The map texture
	#endregion

	//Awake contains the Don't Destroy on Load command. It'll keep this script running the whole time.
	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}


	// Start is called before the first frame update
	void Start()
	{
		//For Testing
		MapGenerator();
	}

	// Update is called once per frame
	void Update()
	{
		TimerSystem();
	}

	//Timer
	void TimerSystem()
	{
		passedSeconds += Time.deltaTime;
		if (passedSeconds >= 1)
		{
			passedSeconds =0;
			TimeRemaining -= 1;
		}
	}

	#region TitleScreen Specific Code
	void TitlePageScreen()
	{
		//Start the timer
		//Move to next screen after
	}
	#endregion
	#region MainGame Specific Code
	void MainGameSetUp()
	{
		//Set the timer
		//Spawn the bodies
	}
	#endregion

	#region MapGenerator
	void MapGenerator()
	{
		mapPNG = Resources.Load<Texture2D>("Maps/testMap");	//Grab the provincemap
		
		Debug.Log($"Max X:{mapWidth},{mapHeight}");

		for (int y = 0; y < mapPNG.height;y++)
		{
			for (int x = 0; x < mapPNG.width; x++)
			{
				Color thisPixel = mapPNG.GetPixel(x, y);
				instantiater(thisPixel,x,y);	//make the tile based on the RGB value

			}
		}
		
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
		}
		else if (r == 255 && g == 0 && b == 0)
		{
			newGameObject = Instantiate(SpawnTile, new Vector3(x, y), Quaternion.identity, gameMap.transform);	//Spwan
		}
		else
		{
			newGameObject = Instantiate(GrassTile, new Vector3(x, y), Quaternion.identity, gameMap.transform);	//The default is just grass
		}
		
		newGameObject.name = $"Tile{x}.{y}";
	}
	#endregion


	#region Scene Management
	//Scene Managment
	public void NewGame()
	{
		Action load = delegate () { NewGameSetUp(); };
		StartCoroutine(AsyncLoader("MainGame", load));
		//Map and everything should be set up in the coroutine right?
	}

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
		//UIManagerScript.FindSceneCanvas(GameCamera);
	}



	void NewGameSetUp()
	{
		Debug.Log(SceneManager.GetActiveScene().name);
	}
	#endregion
}
