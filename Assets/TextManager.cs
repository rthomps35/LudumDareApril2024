using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Text manager is called by the engine for the specific dialog that needs to be printed.
/// It contains the data to read CSVs
/// </summary>


public class TextManager : MonoBehaviour
{

	[SerializeField] TextAsset dialogFile;
	public string CurrentText;
	public Sprite CurrentImage;



	public void DialogLookUp(string IDName)
	{
		dialogFile = Resources.Load<TextAsset>("Text/Dialog");
		string rawData = dialogFile.text.ToString();                                              //read the csv
		string[] splitData;
		
		splitData = rawData.Split('\n');    //splits the CSV by lines


		for (int i = 1; i < splitData.Length - 1; i++)  //This is minus 1 because its reading a blink line at the end that should be ignored
		{                                               //There needs to be a less hacked way of doing this
														//this starts after the first line instead of the header line and is why i != 0
			var data = splitData[i].Split(",");
			for (int d = 0; d < data.Length; d++)
			{
				if (data[d] == IDName)
				{
					CurrentText = data[d+1];
					break;
				}

			}
		}
	}
}
