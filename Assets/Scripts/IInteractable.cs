public interface IInteractable
{
    void OnInteract();
    void OnHoverEnter(); // Für das Highlighten
    void OnHoverExit();  // Highlight entfernen
    string GetDescription(); // Für UI Text ("Luke öffnen", "Sprechen")
}