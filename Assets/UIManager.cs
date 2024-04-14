using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI manager contains the methods to update the UI. They are called by the game manager though.
/// </summary>
/// 
public class UIManager : MonoBehaviour
{
	[SerializeField] TMP_Text timerText;
	[SerializeField] TMP_Text textBox;

	public void UpdateTimer(int Minutes, int Seconds)
	{
		if (Seconds < 10 && Seconds > 0)
		{
			timerText.text = $"{Minutes}:0{Seconds}";
		}
		else if(Seconds == 0)
		{
			timerText.text = $"{Minutes}:0{Seconds}";//May not be needed. We will see in a moment
		}
		else
		{
			timerText.text = $"{Minutes}:{Seconds}";
		}
		
	}

	//Text routine couroutine that spookifies the text is needed

	public void UpdateTextBox()
	{

	}
}
