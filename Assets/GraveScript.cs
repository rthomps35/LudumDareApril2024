using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveScript : MonoBehaviour
{
	//Body parts present
	public bool isDiggable = true;
	[SerializeField] Sprite DugSprite;
	[SerializeField] SpriteRenderer SR;
	public GameObject BodyPart;
	public GameObject Tombstone;
	public enum GraveType {Type1, Type2, Type3}
	public GraveType Type;

	public void iveBeenDug()
	{
		isDiggable= false;
		SR.sprite = DugSprite;
		//body parts destroyed?
	}

}
