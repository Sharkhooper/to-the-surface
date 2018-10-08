using UnityEngine;

public abstract class InputProvider : ScriptableObject {
	public abstract Vector2 MoveDirection { get; }

	public abstract bool JumpPressed { get; }
	public abstract bool PeekingPressed { get; }
	public abstract bool InteractionPressed { get; }
}