using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveScript : MonoBehaviour
{
	//Body parts present
	public bool isDiggable = true;
	[SerializeField] Sprite DugSprite;
	[SerializeField] SpriteRenderer SR;

	public void iveBeenDug()
	{
		isDiggable= false;
		SR.sprite = DugSprite;
		//body parts destroyed?
	}

}
