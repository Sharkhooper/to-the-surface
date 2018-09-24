using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Charackter_Controller : MonoBehaviour
{

	private bool isMoving;

	private enum BlockType
	{
		Block,
		Convex,
		Concave
	}

	private enum PlayerHead
	{
		Up,
		Right,
		Down,
		Left
	}

	public Tilemap tilemapVar;
	
	private PlayerHead headPos=PlayerHead.Up;
	
	[SerializeField] private BlockType blockRight;
	
	public InputProviderPlayer InputProvider;

	private TileBase nextTileOn,nextTileIn;

	


	// Use this for initialization
	void Start () {
		InputProvider = GetComponent<InputProviderPlayer>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (isMoving) return;
		
		StartCoroutine(Wait());
		
		Vector2 movement = InputProvider.MoveDirection;

		var x = movement.x;
		var y = movement.y;



		if (isHorizontal())
		{


			nextTileOn= GetNextTileOn(Vector3Int.right *movement.x);
			nextTile = CheckNextTile(Vector2.right* movement.x);
			
			


		}
		else
		{
			nextBlock= CheckNextBlock(Vector2.up *movement.y);
			nextTile = CheckNextTile()
			
		}


		if (nextIsSomething() == true)
		{

			if (nextIsACube() == true)
			{
				MovePlayerCube();
				
				
			}
			else
			{
				if(nextIsKonkanv()==true)
					MovePlayerconcav();
				else
					MovePlayerconvex();
			}

		}
		else
		{
			return ;
		}


	}

	private IEnumerator Wait()
	{
		isMoving = true;
		yield return new WaitForSeconds(5.0f);
		isMoving = false;
	}

	



	private TileBase GetNextTileOn(Vector3Int direction)
	{
		
		Vector3Int tilePos = Vector3Int.RoundToInt(transform.position + direction);


		switch (headPos)
		{
			
			case PlayerHead.Up:
				tilePos -= Vector3Int.up;
				break;
			case PlayerHead.Down:
				tilePos+=Vector3Int.up ;
				break;
			case PlayerHead.Left:
				tilePos += Vector3Int.right;
				break;
			case PlayerHead.Right:
				tilePos -= Vector3Int.right;
				break;
			default:
				print("Headpos error");
				break;
			
			
		}

		return tilemapVar.GetTile(tilePos));



	}

	private TileBase GetNextTileIn(Vector3Int direction)
	{

		Vector3Int tilePos = Vector3Int.RoundToInt( transform.position + direction);


		return tilemapVar.GetTile(tilePos);
	}


	
	private bool isHorizontal()
	{

		return (headPos == PlayerHead.Up || headPos == PlayerHead.Down);
	}

}

