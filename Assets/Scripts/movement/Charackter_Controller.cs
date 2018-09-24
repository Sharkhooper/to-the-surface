using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	[SerializeField] private BlockType blockRight;
	
	public InputProviderPlayer InputProvider;

	private GameObject nextBlock;

	


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



		if (isHorizontal == true)
		{


			nextBlock= checkNextBlock(Vector2.right *movement.x);
		}
		else
		{
			nextBlock= checkNextBlock(Vector2.up *movement.y);
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

	private void MovePlayerCube(){

		if (isHorizontal() == true)
		{

			transform.Translate(x, 0);

		}
		else
		{
			transform.Translate(0, y);
		}

	}



	private GameObject checkNextBlock(Vector2 direction)
	{
		
		
		
		
		
		
		
		
		
	}
	
	
}

