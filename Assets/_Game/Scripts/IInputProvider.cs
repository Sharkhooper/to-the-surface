using UnityEngine;

public interface IInputProvider {
	Vector2 MoveDirection { get; }

	bool JumpPressed { get; }
	bool PeekingPressed { get; }
}
