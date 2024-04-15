using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
/// UI manager contains the methods to update the UI. They are called by the game manager though.
/// </summary>
/// 
public class UIManager : MonoBehaviour
{
	[SerializeField] TMP_Text timerText;
	[SerializeField] TMP_Text textBox;
	[SerializeField] UnityEngine.UI.Image textImage;
	[SerializeField] GameObject TextImageObject;
	[SerializeField] string CurrentText;
	//bool longformText = false;

	[SerializeField] int textEndSec;
	bool textSkippable = false;
	bool textPresent = false;

	[SerializeField] GameManager gameManager;

	public List<string> textQueue = new List<string>();

	GameObject CurrentSpeaker;
	Sprite CurrentSprite;

	[SerializeField] int TextLinger;
	float coroutineSpeed;
	bool coroutineTimer;

	[SerializeField] GameObject TitleScreenItems;
	[SerializeField] GameObject MainGameItems;


	private void Awake()
	{
		TextImageObject.SetActive(false);
	}

	public void UIMode()
	{
		string SceneName = SceneManager.GetActiveScene().name;

		switch (SceneName)
		{
			case("TitleScreen"):
				TitleScreenItems.SetActive(true);
				MainGameItems.SetActive(false);
				break;
			case("MainGame"):
				TitleScreenItems.SetActive(false);
				MainGameItems.SetActive(true);
				break;
		}
	}

	public void UIMode(string SceneName)
	{
		switch (SceneName)
		{
			case ("TitleScreen"):
				TitleScreenItems.SetActive(true);
				MainGameItems.SetActive(false);
				break;
			case ("MainGame"):
				TitleScreenItems.SetActive(false);
				MainGameItems.SetActive(true);
				break;
		}
	}

	//new text system
	//bool freezes frame()
	//initialize the queue in one method



	private IEnumerator WriteLine(string s)
	{
		//typing = true;
		foreach (char c in s.ToCharArray())
		{
			textBox.text += c;
			yield return new WaitForSeconds(coroutineSpeed);
		}
		Debug.Log("FINISHED");
		//typing = false;
	}

	//Text routine couroutine that spookifies the text is needed


	public void BasicTextWriter(GameObject Speaker, Sprite SpeakerSprite, List<string> LinesToWrite)
	{
		CurrentSpeaker= Speaker;
		CurrentSprite = SpeakerSprite;
		textImage.sprite = SpeakerSprite;
		TextImageObject.SetActive(true);
		textQueue.Clear();
		textEndSec= gameManager.TotalGameSeconds + TextLinger;
		textQueue = LinesToWrite;
		textBox.text = textQueue[0];
	}
	public void BasicTextWriter(GameObject Speaker, Sprite SpeakerSprite, string LineToWrite)
	{
		CurrentSpeaker = Speaker;
		CurrentSprite = SpeakerSprite;
		textImage.sprite = Speaker.GetComponent<Sprite>();   //Set the image
		TextImageObject.SetActive(true);
		textQueue.Clear();
		textEndSec = gameManager.TotalGameSeconds + TextLinger;
		textQueue.Add(LineToWrite);
		textBox.text = LineToWrite;
	}
	public void BasicTextWriter( string LineToWrite)
	{
		textEndSec = gameManager.TotalGameSeconds + TextLinger;
		textBox.text = LineToWrite;
	}


	public void UpdateUIEachSecond(int Minutes, int Seconds)
	{
		//Timer
		UpdateTimer(Minutes,Seconds);
		
		//Text
		if (gameManager.TotalGameSeconds >= textEndSec && textQueue.Count > 0)
		{
			Debug.Log(textQueue[0]);
			textQueue.Remove(textQueue[0]);
			if(textQueue.Count > 0)
			{
				BasicTextWriter(textQueue[0]);
			}
			else
			{
				TextImageObject.SetActive(false);//deactivate image
				textBox.text = "";  //blank text
			}
		}
	}


	//Tic Actions
	void UpdateTimer(int Minutes, int Seconds)
	{
		if (Seconds < 10 && Seconds > 0)
		{
			timerText.text = $"{Minutes}:0{Seconds}";
		}
		else if (Seconds == 0)
		{
			timerText.text = $"{Minutes}:0{Seconds}";//May not be needed. We will see in a moment
		}
		else
		{
			timerText.text = $"{Minutes}:{Seconds}";
		}
	}
}
