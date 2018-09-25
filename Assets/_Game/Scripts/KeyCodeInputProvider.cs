using UnityEngine;

[CreateAssetMenu]
public class KeyCodeInputProvider : ScriptableObject , IInputProvider {
	[SerializeField] private KeyCode jump;
	[SerializeField] private KeyCode peek;

	public Vector2 MoveDirection => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

	public bool JumpPressed => Input.GetKeyDown(jump);

	public bool PeekingPressed => Input.GetKey(peek);
}
