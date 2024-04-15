using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
	public Sprite NPCSprite;
	public List<string> IntroductionLines= new List<string>();
	public List<string> GoodAcceptance = new List<string>();
	public string NPCName;
	public bool IsSpeaking;
	public bool IsAccepted;
	public bool IsIntroduced;
}
