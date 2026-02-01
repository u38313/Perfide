using UnityEngine;
using Ink.Runtime; // Benötigt Ink Plugin

public class MysteryManager : MonoBehaviour
{
    public static MysteryManager Instance;

    [Header("Controllers")]
    public TerminalController terminalController;

    [Header("References")]
    public TerminalUI terminalUI;
    public StoryDirector storyDirector;

    [Header("Story Data")]
    public TextAsset interrogationInk; // Das Verhör-Skript
    public GameObject gunObject;
    public GameObject leverObject;

    // --- State Flags ---
    private bool isIntroFinished = false;
    private bool heardInsideStory = false;
    private bool heardOutsideStory = false;
    private bool scanLogUnlocked = false;

    private void Awake()
    {
        Instance = this;
        if (gunObject) gunObject.SetActive(false); // Waffe erst am Ende an
        if (leverObject) leverObject.SetActive(false);
    }

    private void Start()
    {
        // Wir hören auf Ink Events
        DialogueManager.Instance.OnGameEvent += HandleInkEvents;
    }

    // --- INTERAKTIONEN (Werden von Objekten aufgerufen) ---

    // 1. Die Luke/Fenster Interaktion
    public void OnWindowInteract()
    {
        Debug.Log("Luke angeklickt. Intro fertig? " + isIntroFinished); // <--- HIER
        if (!isIntroFinished)
        {
            // Fall A: Intro noch nicht passiert -> Startet Cutscene
            storyDirector.StartIntroScene();
        }
        else
        {
            // Fall B: Intro vorbei -> Startet Gespräch mit Grigsby Draußen
            StartDialogue("Interrogate_G2");
        }
    }

    // 2. Grigsby Drinnen Interaktion
    public void OnInsideGrigsbyInteract()
    {
        Debug.Log("Grigsby Innen angeklickt. Intro fertig? " + isIntroFinished); // <--- HIER
        if (isIntroFinished)
        {
            StartDialogue("Interrogate_G1");
        }
    }

    // --- LOGIK & EVENTS ---

    void HandleInkEvents(string eventName)
    {
        switch (eventName)
        {
            case "IntroDone":
                FinishIntroSequence();
                break;
            case "HeardInsideStory":
                heardInsideStory = true;
                CheckForScanUnlock();
                break;
            case "HeardOutsideStory":
                heardOutsideStory = true;
                CheckForScanUnlock();
                break;
            case "EnableGunAndLever":
                EnableEndGame();
                break;
        }
    }

    void FinishIntroSequence()
    {
        Debug.Log("Intro beendet. Free Game Mode.");

        // 1. Logs freischalten
        terminalUI.UnlockLog(2);

        // 3. Flags setzen
        isIntroFinished = true;
    }

    void CheckForScanUnlock()
    {
        // Wenn beide Geschichten gehört wurden -> Scan Log freischalten
        if (!scanLogUnlocked && heardInsideStory && heardOutsideStory)
        {
            Debug.Log("Widerspruch gefunden! Scan Log unlocked.");
            scanLogUnlocked = true;
            terminalUI.UnlockLog(3);
        }
    }

    void StartDialogue(string knotName)
    {
            DialogueManager.Instance.StartDialogue(interrogationInk); // Lädt Story, Panel geht an (und bleibt jetzt an!)
            DialogueManager.Instance.story.ChoosePathString(knotName); // Springt zum richtigen Knot

            // Wissen an Ink übergeben
            if (scanLogUnlocked)
        {
            DialogueManager.Instance.story.variablesState["know_crane_fixed_log"] = true;
        }

        DialogueManager.Instance.RefreshView(); // Zeigt den Text an -> PERFEKT!
}

    void EnableEndGame()
    {
        Debug.Log("Endphase! Wähle weise.");
        if (gunObject) gunObject.SetActive(true);
        if (leverObject) leverObject.SetActive(true);
        // Optional: Collider aktivieren statt SetActive, je nach Setup
    }
}