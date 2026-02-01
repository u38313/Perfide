using UnityEngine;
using TMPro; // Falls ihr TextMeshPro für UI nutzt (empfohlen)
// using UnityEngine.UI; // Falls ihr das alte Standard Text UI nutzt

public class InteractionController : MonoBehaviour
{
    [Header("Settings")]
    public float interactionDistance = 3f; // Wie weit kann man greifen?
    public LayerMask interactableLayer;    // Performance: Nur auf bestimmte Layer prüfen

    [Header("UI Reference")]
    public TextMeshProUGUI interactionPromptText; // Z.B. "Drücke E um zu reden"
    // public Text interactionPromptText; // (Für altes UI)

    private Camera cam;
    private IInteractable currentInteractable;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // 1. Darf der Spieler überhaupt interagieren?
        // Wenn wir im Dialog oder Cutscene sind, Raycast deaktivieren
        if (GameManager.Instance.CurrentState != GameState.FreeLook)
        {
            ClearInteraction(); // Falls wir gerade was angeschaut haben -> weg damit
            return;
        }

        CheckForInteractable();
        CheckInput();
    }

    void CheckForInteractable()
    {
        // Strahl genau aus der Mitte der Kamera schießen
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            // Versuchen, das IInteractable Interface vom getroffenen Objekt zu holen
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                // Ist es ein NEUES Objekt, das wir vorher nicht angeschaut haben?
                if (interactable != currentInteractable)
                {
                    ClearInteraction(); // Altes Highlight entfernen

                    currentInteractable = interactable;
                    currentInteractable.OnHoverEnter(); // Neues Highlight an

                    // UI Text setzen
                    if (interactionPromptText != null)
                        interactionPromptText.text = currentInteractable.GetDescription();
                }
                return; // Wir haben was gefunden, fertig für diesen Frame
            }
        }

        // Wenn wir nichts (oder nichts Interagierbares) treffen:
        ClearInteraction();
    }

    void CheckInput()
    {
        // Option A: Nur noch E erlaubt (Empfohlen!)
        // Verhindert das versehentliche Klicken beim Fokus-Wechsel
        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.OnInteract();
        }

        /* // Option B: Falls du Mausklick UNBEDINGT behalten willst:
        // Prüfe, ob der Cursor auch wirklich gelockt ist (im Editor oft buggy beim Wechsel)
        if (currentInteractable != null && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)))
        {
             // Nur interagieren, wenn wir nicht über UI sind
             if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
             {
                 currentInteractable.OnInteract();
             }
        }
        */
    }

    void ClearInteraction()
    {
        if (currentInteractable != null)
        {
            currentInteractable.OnHoverExit(); // Highlight aus
            currentInteractable = null;

            // UI Text leeren
            if (interactionPromptText != null)
                interactionPromptText.text = "";
        }
    }
}