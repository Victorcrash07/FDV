public interface IInteractable
{
    // Cualquier objeto interactuable debe tener este método
    void Interact();

    // (Opcional) Podemos añadir un mensaje para la UI
    string GetInteractionMessage();
}