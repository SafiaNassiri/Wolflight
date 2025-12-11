// Basic interface for anything the player can interact with.
// Any object implementing this must define what happens on Interact().

public interface IInteractable
{
    // Called when the player interacts with this object
    void Interact();
}
