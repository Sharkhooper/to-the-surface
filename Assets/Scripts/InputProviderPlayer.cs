using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProviderPlayer : InputProvider {
	void Update()
	{
		MoveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		JumpPressed = Input.GetButtonDown("Jump");
	}
}
