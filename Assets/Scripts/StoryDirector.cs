using UnityEngine;

public class StoryDirector : MonoBehaviour
{
    [Header("Die Akteure")]
    public GameObject grigsbyInside;  // Der, der reinkommt
    public GameObject grigsbyOutside; // Der vor dem Fenster

    [Header("Story Data")]
    public TextAsset introInkJSON;    // Die Dialog-Datei

    private void Start()
    {
        // 1. Am Anfang sind beide unsichtbar
        if (grigsbyInside) grigsbyInside.SetActive(false);
        if (grigsbyOutside) grigsbyOutside.SetActive(false);

        // 2. Wir hören auf den DialogueManager
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnGameEvent += HandleInkEvents;
        }
    }

    private void OnDestroy()
    {
        // Sauber abmelden um Errors zu vermeiden
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnGameEvent -= HandleInkEvents;
        }
    }

    // --- Diese Funktion wird von der LUKE aufgerufen (via SimpleInteractable) ---
    public void StartIntruderSequence()
    {
        // A. Grigsby Drinnen taucht auf
        if (grigsbyInside) grigsbyInside.SetActive(true);

        // B. Dialog Starten
        DialogueManager.Instance.StartDialogue(introInkJSON);
    }

    // --- Diese Funktion reagiert auf Ink Tags ---
    void HandleInkEvents(string eventName)
    {
        if (eventName == "RevealOutside")
        {
            if (grigsbyOutside) grigsbyOutside.SetActive(true);
        }
    }
}