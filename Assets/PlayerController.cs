using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] Rigidbody2D rigidbody2D;
	public float speed;	//I'm setting this public if we want to modify it later like a last miute thing.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	//The dig thing
	void InteractWithGrave()
	{
		//Should take a specific amount of time to up the tension
	}
}
