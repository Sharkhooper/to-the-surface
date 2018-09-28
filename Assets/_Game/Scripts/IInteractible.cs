using UnityEngine.EventSystems;

public interface IInteractible : IEventSystemHandler{
	void Interact(PlayerActor source);
}
