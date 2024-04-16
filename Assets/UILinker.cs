using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILinker : MonoBehaviour
{
	public GameManager gameManager;

	public UnityEngine.UI.Image headImage;
	public GameObject headImageObject;
	public UnityEngine.UI.Image torsoImage;
	public GameObject torsoImageObject;
	public UnityEngine.UI.Image armLeftImage;
	public GameObject armLeftImageObject;
	public UnityEngine.UI.Image armRightImage;
	public GameObject armRightImageObject;
	public UnityEngine.UI.Image legLeftImage;
	public GameObject legLeftImageObject;
	public UnityEngine.UI.Image legRightImage;
	public GameObject legRightImageObject;
	public TMP_Text timerText;
	public TMP_Text textBox;
	public UnityEngine.UI.Image textImage;
	public GameObject TextImageObject;


	// Start is called before the first frame update
	void Start()
    {
		GameObject Manager = GameObject.FindGameObjectWithTag("GameManager");
		gameManager = Manager.GetComponent<GameManager>();
		UIManager UIS = gameManager.GetComponent<UIManager>();
	
		UIS.headImage = headImage;
		UIS.headImageObject = headImageObject;
		UIS.torsoImage = torsoImage;
		UIS.torsoImageObject = torsoImageObject;
		UIS.armLeftImage = armLeftImage;
		UIS.armLeftImageObject = armLeftImageObject;
		UIS.armRightImage = armRightImage;
		UIS.armRightImageObject = armRightImageObject;
		UIS.legLeftImage = legLeftImage;
		UIS.legLeftImageObject = legLeftImageObject;
		UIS.legRightImage = legRightImage;
		UIS.legRightImageObject = legRightImageObject;
		UIS.timerText = timerText;
		UIS.textBox = textBox;
		UIS.textImage = textImage;
		UIS.TextImageObject = TextImageObject;

}

}
