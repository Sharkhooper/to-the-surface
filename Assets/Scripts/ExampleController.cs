using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleController : MonoBehaviour
{
	public InputProviderPlayer InputProvider;

	private void Start()
	{
		InputProvider = GetComponent<InputProviderPlayer>();
	}

	// Update is called once per frame
	void Update ()
	{
		Vector2 movement = InputProvider.MoveDirection;
		
		//Debug, check if movement is registered
		if(movement.x != 0 || movement.y != 0)
			print("Movement: x=" + movement.x + " y=" + movement.y);
	}
}
