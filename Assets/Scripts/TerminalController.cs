using UnityEngine;
using System.Collections;

public class TerminalController : MonoBehaviour, IInteractable
{
    [Header("Setup")]
    public Transform viewPoint;       // Wo ist der Kopf beim Lesen?
    public Canvas terminalCanvas;     // Das UI

    [Header("Einstellungen")]
    public bool startActive = true;   // Startet man sitzend?
    public float transitionSpeed = 2.0f;
    [Tooltip("Wie viele Meter hinter dem 'ViewPoint' landet man, wenn man aufsteht (nur beim Spielstart wichtig)?")]
    public float standUpOffset = 0.6f; // Kleinerer Wert = näher am Tisch
    public string interactPrompt = "Terminal benutzen";

    private Camera mainCam;
    private Vector3 savedPosition;     // Gespeicherte Position
    private Quaternion savedRotation;  // Gespeicherte Rotation
    private bool isAtTerminal = false;

    void Start()
    {
        mainCam = Camera.main;

        if (startActive)
        {
            // Startet direkt im Sitz-Modus
            StartCoroutine(ForceStartAtTerminal());
        }
    }

    void Update()
    {
        // Rauskommen mit ESC oder X
        if (isAtTerminal && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X)))
        {
            ExitTerminal();
        }
    }

    // --- Interface für InteractionController ---
    public void OnInteract()
    {
        if (!isAtTerminal)
        {
            EnterTerminal();
        }
    }

    public void OnHoverEnter() { }
    public void OnHoverExit() { }

    public string GetDescription()
    {
        return isAtTerminal ? "" : interactPrompt;
    }

    // --- Logik ---

    public void EnterTerminal()
    {
        if (isAtTerminal) return;

        // 1. JETZIGE Position merken (bevor wir uns bewegen)
        savedPosition = mainCam.transform.position;
        savedRotation = mainCam.transform.rotation;

        Debug.Log($"Position gemerkt: {savedPosition}"); // Check Konsole ob das stimmt

        // 2. Status ändern
        isAtTerminal = true;
        GameManager.Instance.UpdateGameState(GameState.ReadingLog);

        if (terminalCanvas != null) terminalCanvas.gameObject.SetActive(true);

        // 3. Kamerafahrt zum Bildschirm
        StopAllCoroutines();
        StartCoroutine(MoveCamera(viewPoint.position, viewPoint.rotation));
    }

    public void ExitTerminal()
    {
        if (!isAtTerminal) return;

        isAtTerminal = false;

        // 1. Zurückbewegen zur gemerkten Position
        StopAllCoroutines();
        StartCoroutine(MoveCamera(savedPosition, savedRotation, () => {
            // Erst wenn wir angekommen sind:
            GameManager.Instance.UpdateGameState(GameState.FreeLook);
        }));
    }

    // --- Spezialfall: Spielstart ---
    IEnumerator ForceStartAtTerminal()
    {
        yield return new WaitForEndOfFrame(); // Warten bis alles initialisiert ist

        isAtTerminal = true;
        GameManager.Instance.UpdateGameState(GameState.ReadingLog);

        if (terminalCanvas != null) terminalCanvas.gameObject.SetActive(true);

        // Kamera hart setzen
        mainCam.transform.position = viewPoint.position;
        mainCam.transform.rotation = viewPoint.rotation;

        // Da wir keine "vorherige" Position haben, berechnen wir eine direkt hinter dem Stuhl
        // ViewPoint schaut zum Monitor (Z+), also gehen wir -Z (nach hinten)
        savedPosition = viewPoint.position - (viewPoint.forward * standUpOffset);

        // Rotation: Wir schauen beim Aufstehen wieder Richtung Monitor
        savedRotation = Quaternion.LookRotation(viewPoint.forward);
    }

    // --- Kamerafahrt ---
    IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot, System.Action onComplete = null)
    {
        float t = 0f;
        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            mainCam.transform.position = Vector3.Lerp(startPos, targetPos, smoothT);
            mainCam.transform.rotation = Quaternion.Slerp(startRot, targetRot, smoothT);

            yield return null;
        }

        mainCam.transform.position = targetPos;
        mainCam.transform.rotation = targetRot;

        onComplete?.Invoke();
    }
}