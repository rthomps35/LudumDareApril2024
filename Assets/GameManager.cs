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
	public int TimeRemaining;	

	#region ChildObjects and their Scripts
	[SerializeField] GameObject GameCamera; //Added in inspector
	[SerializeField] GameObject UIManager;  //Added in inspector
	[SerializeField] GameObject MapManager; //Added in inspector
	//Below code was copied from an old project
	//[SerializeField] CameraManager CameraManager;   //These are the core scripts of the above objects.
	//[SerializeField] UIManager UIManagerScript;     //These scripts arn't attached to make the GameManager Object cleaner.
	//[SerializeField] MapManager MapManagerScript;
	#endregion

	//Awake contains the Don't Destroy on Load command. It'll keep this script running the whole time.
	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	//Timer
	void TimerSystem()
	{
		//Time DeltaTimeShit
	}

	#region TitleScreen Specific Code
	void TitlePageScreen()
	{
		//Start the timer
		//Move to next screen after
	}
	#endregion
	#region MainGame Specific Code
	
	#endregion


	#region Scene Management
	//Scene Managment
	public void NewGame()
	{
		Action load = delegate () { NewGameSetUp(); };  //Okay so this actually runs
		StartCoroutine(AsyncLoader("MainGame", load));

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
