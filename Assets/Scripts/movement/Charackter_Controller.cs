using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Unite2017.Variables;
using UnityEngine;

public class Charackter_Controller : MonoBehaviour
{

	private bool isMoving;

	public void MovePlayer()
	{if (isHorizontal == true)
		{


			transform.Translate(x, 0);
		}




		if (isHorizontal == false)
		{


			transform.Translate(0, y);
		}
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update()
	{
		if (isMoving) return;
		
		var x = Input.GetAxis("Horizontal");
		var y = Input.GetAxis("Vertical");



		if (nextIsSomething() == true)
		{

			if (nextIsACube() == true)
			{
				
				
				
			}

		}
		else
		{
			return ;
		}


	}
}
