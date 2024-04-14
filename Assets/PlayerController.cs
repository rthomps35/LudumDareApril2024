using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public GameManager GameManager;
	[SerializeField] Rigidbody2D rigidbody2D;
	[SerializeField] enum PlayerFacing {Left, Up, Right, Down}
	[SerializeField] PlayerFacing playerFacing;
	public float speed; //This should be the max speed probably? Right and just work up to it
						//interaction field?
	
	bool moving = false;

	//Sprites
	[SerializeField] SpriteRenderer playerSprite;
	[SerializeField] Sprite UpSprite;
	[SerializeField] Sprite DownSprite;
	[SerializeField] Sprite LeftSprite;
	[SerializeField] Sprite RightSprite;

	//
	[SerializeField] Vector2 target;
	public void PokemonController()
	{
		float step = speed * Time.deltaTime;
		

		if(moving == false)
		{
			if (Input.GetKey(KeyCode.W))
			{
				target = new Vector2(transform.position.x, transform.position.y + 1);
				playerFacing = PlayerFacing.Up;
				playerSprite.sprite = UpSprite;
				if(CheckCollision(target) == false)
				{
					moving = true;
				}
			}
			else if (Input.GetKey(KeyCode.D))
			{
				target = new Vector2(transform.position.x + 1, transform.position.y);
				playerFacing = PlayerFacing.Right;
				playerSprite.sprite = RightSprite;
				if (CheckCollision(target) == false)
				{
					moving = true;
				}
			}
			else if (Input.GetKey(KeyCode.S))
			{
				target = new Vector2(transform.position.x, transform.position.y - 1);
				playerFacing = PlayerFacing.Down;
				playerSprite.sprite = DownSprite;
				if (CheckCollision(target) == false)
				{
					moving = true;
				}
			}
			else if (Input.GetKey(KeyCode.A))
			{
				target = new Vector2(transform.position.x - 1, transform.position.y);
				playerFacing = PlayerFacing.Left;
				playerSprite.sprite = LeftSprite;
				if (CheckCollision(target) == false)
				{
					moving = true;
				}
			}
		}
		else
		{
			transform.position = Vector2.MoveTowards(transform.position, target, step);
			//walk
			if (transform.position.x == target.x && transform.position.y == target.y)
			{
				moving = false;
				
			}
			
		}
		/*
		//Facing
		//This is a dogshit way of handling this
		switch (playerFacing)
		{
			case PlayerFacing.Up:
				playerSprite.sprite = UpSprite;
				break;
			case PlayerFacing.Down:
				playerSprite.sprite = DownSprite;
				break;
			case PlayerFacing.Left:
				playerSprite.sprite = LeftSprite;
				break;
			case PlayerFacing.Right:
				playerSprite.sprite = RightSprite;
				break;
		}
		*/
	}
	
	bool CheckCollision(Vector2 CheckedVector)
	{
		int x = (int)CheckedVector.x;
		int y = (int)CheckedVector.y;
		TileScript tile = GameManager.MapTiles[x, y].GetComponent<TileScript>();
		return tile.Walkable;

	}

	//The dig thing
	void Interaction()
	{
		//Should take a specific amount of time to up the tension
		//throw a interaction thing in front and a progress bar in front of the player?
		//if collided object is a grave
			//DIG
			//after digging select body parts
		//If collided objec is an npc
			//talk/end task
	}

	
}
