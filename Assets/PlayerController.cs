using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] Rigidbody2D rigidbody2D;
	public float speed; //This should be the max speed probably? Right and just work up to it
	//interaction field?

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMovementAndControl();
    }

	void playerMovementAndControl()
	{

		//It really needs a ramp up to the proper speed
		//Its way too floaty right now but it can be fixed later. It needs to slowly scale the speed up just like it slows down

		if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
		{
			rigidbody2D.velocity = new Vector2(-speed, speed);   //left up
		}
		else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
		{
			rigidbody2D.velocity = new Vector2(speed, speed);   //right up
		}
		else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
		{
			rigidbody2D.velocity = new Vector2(-speed, -speed);   //left down
		}
		else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
		{
			rigidbody2D.velocity = new Vector2(speed, -speed);   //right down
		}
		else if (Input.GetKey(KeyCode.W))
		{
			//face up
			rigidbody2D.velocity = new Vector2(0.0f, speed);	//up
		}
		else if (Input.GetKey(KeyCode.S))
		{
			rigidbody2D.velocity = new Vector2(0.0f, -speed);	//down
		}
		else if (Input.GetKey(KeyCode.A))
		{
			rigidbody2D.velocity = new Vector2(-speed, 0.0f);	//left
		}
		else if (Input.GetKey(KeyCode.D))
		{
			rigidbody2D.velocity = new Vector2(speed, 0.0f);	//right
		}

		else if (Input.GetKey(KeyCode.W) != true && Input.GetKey(KeyCode.S)!=true && Input.GetKey(KeyCode.A) != true && Input.GetKey(KeyCode.D) != true)
		{
			rigidbody2D.velocity = rigidbody2D.velocity / 1.005f;   //Grabbed thos line from stack overflow and i'm shocked it works. I need to find a way to reverse this without exceeding speed.
		}
		Debug.Log(rigidbody2D.velocity);
	}

	//The dig thing
	void InteractWithGrave()
	{
		//Should take a specific amount of time to up the tension
		//throw a interaction thing in front and a progress bar in front of the player?
	}

	
}
