using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenScript : MonoBehaviour
{
	[SerializeField] GameObject GameManagerPrefab;

	private void Awake()
	{
		if(GameObject.FindGameObjectWithTag("GameManager") == null)
		{
			Instantiate(GameManagerPrefab);
		}

	}
}
