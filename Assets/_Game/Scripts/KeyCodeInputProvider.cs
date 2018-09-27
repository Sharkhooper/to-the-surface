using UnityEngine;

[CreateAssetMenu]
public class KeyCodeInputProvider : InputProvider {
	[SerializeField] private KeyCode jump;
	[SerializeField] private KeyCode peek;
	[SerializeField] private KeyCode interaction;

	public override Vector2 MoveDirection => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

	public override bool JumpPressed => Input.GetKeyDown(jump);

	public override bool PeekingPressed => Input.GetKey(peek);

	public override bool InteractionPressed => Input.GetKeyDown(interaction);
}
