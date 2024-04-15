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
	bool digging = false;
	[SerializeField] int secondsToDig = 3;
	[SerializeField] int digCompletionTime;
	[SerializeField] GraveScript GraveTarget;

	//Sprites
	[SerializeField] SpriteRenderer playerSprite;
	[SerializeField] Sprite UpSprite;
	[SerializeField] Sprite DownSprite;
	[SerializeField] Sprite LeftSprite;
	[SerializeField] Sprite RightSprite;

	//
	[SerializeField] Vector2 target;

	//Pokemon Controller is a once per frame not tic
	public void PokemonController()
	{
		float step = speed * Time.deltaTime;
		

		if(moving == false && digging == false)
		{
			if (Input.GetKey(KeyCode.W))
			{
				target = new Vector2(transform.position.x, transform.position.y + 1);
				playerFacing = PlayerFacing.Up;
				playerSprite.sprite = UpSprite;
				if(checkTile(target).Walkable == true)
				{
					moving = true;
				}
			}
			else if (Input.GetKey(KeyCode.D))
			{
				target = new Vector2(transform.position.x + 1, transform.position.y);
				playerFacing = PlayerFacing.Right;
				playerSprite.sprite = RightSprite;
				if (checkTile(target).Walkable == true)
				{
					moving = true;
				}
			}
			else if (Input.GetKey(KeyCode.S))
			{
				target = new Vector2(transform.position.x, transform.position.y - 1);
				playerFacing = PlayerFacing.Down;
				playerSprite.sprite = DownSprite;
				if (checkTile(target).Walkable == true)
				{
					moving = true;
				}
			}
			else if (Input.GetKey(KeyCode.A))
			{
				target = new Vector2(transform.position.x - 1, transform.position.y);
				playerFacing = PlayerFacing.Left;
				playerSprite.sprite = LeftSprite;
				if (checkTile(target).Walkable == true)
				{
					moving = true;
				}
			}
			else if(Input.GetKeyDown(KeyCode.Space))
			{
				TileScript ts = checkTile(transform.position).GetComponent<TileScript>();
				if (ts.ObjectOnTile.tag == "Grave")
				{

					GraveTarget = ts.ObjectOnTile.GetComponent<GraveScript>();
					if(GraveTarget.isDiggable == true)
					{
						digCompletionTime = (int)GameManager.TotalGameSeconds + secondsToDig;
						digging= true;
						GameManager.AudioManager.PlayOneShot(GameManager.DigNoise);   //play dig sound
						//Start animation
					}
					
					else
					{
						//Theres nothing left!
					}
				}
				Debug.Log($"");
			}
		}
		else
		{
			if (moving== true)
			{
				transform.position = Vector2.MoveTowards(transform.position, target, step);
				//walk
				if (transform.position.x == target.x && transform.position.y == target.y)
				{
					moving = false;

				}
			}
			else if (digging == true)
			{
				if(GameManager.TotalGameSeconds >= digCompletionTime)
				{
					digging= false;
					GraveTarget.iveBeenDug();
				}
				else
				{
					//GameManager.SFXManager.PlayOneShot(GameManager.DigNoise);	//play dig sound
					//play dig animation
				}				
			}
		}

	}

	public void PlayerTicActions()
	{

	}
	
	TileScript checkTile(Vector2 CheckedVector)
	{
		int x = (int)CheckedVector.x;
		int y = (int)CheckedVector.y;
		TileScript tile = GameManager.MapTiles[x, y].GetComponent<TileScript>();
		return tile;
	}

	
}
