using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputProvider : MonoBehaviour
{
	// Stores the direction the player intends to walk
	public Vector2 MoveDirection { get; protected set; }

	
}
