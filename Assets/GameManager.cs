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
	[SerializeField] GameObject MapManager; //Added in inspector
	#endregion

	#region Timer Items
	public int TimeRemaining;
	[SerializeField] float passedSeconds;
	
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
	
	#endregion


	#region Scene Management
	//Scene Managment
	public void NewGame()
	{
		Action load = delegate () { NewGameSetUp(); };
		StartCoroutine(AsyncLoader("MainGame", load));
		//Map and everything shoujld be set up in the coroutine right?
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
