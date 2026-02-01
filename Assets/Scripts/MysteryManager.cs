using UnityEngine;
using Ink.Runtime;

public class MysteryManager : MonoBehaviour
{
    public static MysteryManager Instance;

    [Header("References")]
    public TerminalUI terminalUI;
    public TextAsset interrogationInk;
    public GameObject gunObject;
    public GameObject leverObject;

    [Header("Game State Flags")]
    public bool heardInsideStory = false;
    public bool heardOutsideStory = false;
    public bool scanLogUnlocked = false;

    private void Awake()
    {
        Instance = this;
        if (gunObject) gunObject.GetComponent<Collider>().enabled = false;
        if (leverObject) leverObject.GetComponent<Collider>().enabled = false;
    }

    private void Start()
    {
        // Wir hören auf alle Events vom DialogueManager
        DialogueManager.Instance.OnGameEvent += HandleInkEvents;
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnGameEvent -= HandleInkEvents;
    }

    // --- 1. Wird aufgerufen, wenn das INTRO vorbei ist ---
    public void FinishIntro()
    {
        Debug.Log("Intro Finished: Unlocking Crew Log");
        terminalUI.UnlockLog(2); // Crew Info (Button 3) freischalten
    }

    // --- 2. Wird aufgerufen von DialogueManager, wenn Tags kommen ---
    void HandleInkEvents(string eventName)
    {
        switch (eventName)
        {
            case "IntroDone":
                FinishIntro();
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
                EnableEndgame();
                break;
        }
    }

    // --- 3. Logik: Haben wir beide Stories gehört? ---
    void CheckForScanUnlock()
    {
        // Nur feuern, wenn noch nicht passiert, und beide Bedingungen wahr sind
        if (!scanLogUnlocked && heardInsideStory && heardOutsideStory)
        {
            Debug.Log("Widerspruch erkannt! Scan Log freigeschaltet.");
            scanLogUnlocked = true;
            terminalUI.UnlockLog(3); // Scan Log (Button 4) freischalten

            // WICHTIG: Ink sagen, dass wir es wissen!
            // (Passiert beim Start des nächsten Dialogs, siehe TalkTo...)
        }
    }

    // --- 4. Startet Dialoge ---
    public void TalkToInside() => StartInterrogation("Interrogate_G1");
    public void TalkToOutside() => StartInterrogation("Interrogate_G2");

    private void StartInterrogation(string knotName)
    {
        DialogueManager.Instance.StartDialogue(interrogationInk);
        DialogueManager.Instance.story.ChoosePathString(knotName);

        // Wenn das Log unlocked ist, sagen wir es Ink
        if (scanLogUnlocked)
        {
            DialogueManager.Instance.story.variablesState["know_crane_fixed_log"] = true;
        }

        DialogueManager.Instance.RefreshView();
    }

    void EnableEndgame()
    {
        Debug.Log("Endphase aktiv!");
        if (gunObject) gunObject.GetComponent<Collider>().enabled = true;
        if (leverObject) leverObject.GetComponent<Collider>().enabled = true;
    }
}