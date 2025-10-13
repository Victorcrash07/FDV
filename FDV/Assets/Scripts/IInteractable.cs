public interface IInteractable
{
    // Cualquier objeto interactuable DEBE tener este método.
    void Interact();

    // (Opcional) Podemos añadir un mensaje para la UI.
    string GetInteractionMessage();
}