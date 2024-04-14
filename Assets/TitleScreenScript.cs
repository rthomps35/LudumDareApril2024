using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenScript : MonoBehaviour
{
	[SerializeField] GameObject GameManagerPrefab;
	[SerializeField] GameObject GameManager;

	private void Start()
	{
		GameManager = GameObject.FindGameObjectWithTag("GameManager");
		if (GameManager == null)
		{
			GameManager = Instantiate(GameManagerPrefab);
		}

	}
}
