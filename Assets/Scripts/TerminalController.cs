using UnityEngine;
using System.Collections;

public class TerminalController : MonoBehaviour, IInteractable
{
    [Header("References")]
    public Transform viewPoint;
    public Canvas terminalCanvas;

    [Header("Settings")]
    public string promptText = "Terminal benutzen";
    public bool isInteractable = false; // Wird vom MysteryManager gesteuert!
    public bool startActive = true;     // Startet das Spiel sitzend?

    private Camera mainCam;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isAtTerminal = false;

    void Start()
    {
        mainCam = Camera.main;

        // Setup f¸r Spielstart
        if (startActive)
        {
            // Wir tun so, als w‰ren wir schon reingegangen
            isAtTerminal = true;
            GameManager.Instance.UpdateGameState(GameState.ReadingLog);

            // Kamera direkt positionieren ohne Fahrt
            if (viewPoint != null)
            {
                mainCam.transform.position = viewPoint.position;
                mainCam.transform.rotation = viewPoint.rotation;
                originalPosition = viewPoint.position - viewPoint.forward * 1.5f; // Fallback Position
                originalRotation = Quaternion.LookRotation(viewPoint.forward);
            }
        }
    }

    void Update()
    {
        // Rauskommen mit ESC
        if (isAtTerminal && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X)))
        {
            ExitTerminal();
        }
    }

    // --- IInteractable Implementation (Das hat gefehlt!) ---

    public void OnInteract()
    {
        // Nur interagieren, wenn erlaubt (vom MysteryManager)
        if (isInteractable && !isAtTerminal)
        {
            EnterTerminal();
        }
    }

    public void OnHoverEnter()
    {
        // Optional: Monitor leuchten lassen
    }

    public void OnHoverExit()
    {
        // Leuchten aus
    }

    public string GetDescription()
    {
        if (isInteractable) return promptText;
        return ""; // Kein Text, wenn gesperrt
    }

    // --- Logik ---

    public void EnterTerminal()
    {
        isAtTerminal = true;

        // 1. Position merken (wo wir standen)
        originalPosition = mainCam.transform.position;
        originalRotation = mainCam.transform.rotation;

        // 2. State ‰ndern
        GameManager.Instance.UpdateGameState(GameState.ReadingLog);

        // 3. Hinbewegen
        StopAllCoroutines();
        StartCoroutine(MoveCamera(viewPoint.position, viewPoint.rotation));
    }

    public void ExitTerminal()
    {
        isAtTerminal = false;

        // 1. Zur¸ckbewegen (zu der Position, wo wir standen, ODER Standard Position)
        // Falls wir direkt im Stuhl gestartet sind, haben wir keine "originalPosition". 
        // Wir nehmen eine Position etwas hinter dem Stuhl an.
        Vector3 targetPos = (originalPosition == Vector3.zero) ? viewPoint.position - viewPoint.forward * 1.5f : originalPosition;
        Quaternion targetRot = (originalPosition == Vector3.zero) ? Quaternion.LookRotation(viewPoint.forward) : originalRotation;

        StopAllCoroutines();
        StartCoroutine(MoveCamera(targetPos, targetRot, () => {
            GameManager.Instance.UpdateGameState(GameState.FreeLook);
        }));
    }

    // --- Helper zum Aktivieren von Auﬂen ---
    public void SetInteractable(bool state)
    {
        isInteractable = state;
    }

    IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot, System.Action onComplete = null)
    {
        float t = 0f;
        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * 2.0f; // Speed
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