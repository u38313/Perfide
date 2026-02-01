using UnityEngine;
using System.Collections;

public class TerminalController : MonoBehaviour, IInteractable
{
    [Header("References")]
    public TerminalUI terminalUIScript; // Dein existierendes Skript
    public Transform viewPoint;         // Das leere Objekt vor dem Monitor
    public Canvas terminalCanvas;       // Das WorldSpace Canvas

    [Header("Settings")]
    public float transitionSpeed = 2.0f;
    public bool startActive = true;     // Soll das Spiel direkt hier starten?

    private Camera mainCam;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isAtTerminal = false;

    void Start()
    {
        mainCam = Camera.main;

        // Canvas am Anfang unsichtbar/inaktiv machen, falls gewünscht
        // terminalCanvas.gameObject.SetActive(true); 

        if (startActive)
        {
            // Hack: Kleiner Delay, damit GameManager erst initialisiert ist
            StartCoroutine(StartSequenceDelayed());
        }
    }

    IEnumerator StartSequenceDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        EnterTerminal(true); // true = sofort springen ohne Animation
    }

    void Update()
    {
        // Wenn wir am Terminal sind und Escape drücken -> Aufstehen
        if (isAtTerminal && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X)))
        {
            ExitTerminal();
        }
    }

    // --- IInteractable Implementation ---

    public void OnInteract()
    {
        EnterTerminal(false);
    }

    public void OnHoverEnter()
    {
        // Optional: Monitor Rahmen leuchten lassen
    }

    public void OnHoverExit()
    {
        // Leuchten aus
    }

    public string GetDescription()
    {
        return "Terminal benutzen";
    }

    // --- Logik ---

    public void EnterTerminal(bool instant)
    {
        if (isAtTerminal) return;

        // 1. Position merken
        originalPosition = mainCam.transform.position;
        originalRotation = mainCam.transform.rotation;

        // 2. State ändern (Maus wird sichtbar, Movement gesperrt)
        GameManager.Instance.UpdateGameState(GameState.ReadingLog);

        isAtTerminal = true;

        // 3. UI aktivieren (falls vorher aus)
        terminalCanvas.gameObject.SetActive(true);

        // 4. Hinbewegen
        StopAllCoroutines();
        if (instant)
        {
            mainCam.transform.position = viewPoint.position;
            mainCam.transform.rotation = viewPoint.rotation;
        }
        else
        {
            StartCoroutine(MoveCamera(viewPoint.position, viewPoint.rotation));
        }
    }

    public void ExitTerminal()
    {
        if (!isAtTerminal) return;

        isAtTerminal = false;

        // 1. Zurückbewegen
        StopAllCoroutines();
        StartCoroutine(MoveCamera(originalPosition, originalRotation, () => {
            // Wenn angekommen:
            GameManager.Instance.UpdateGameState(GameState.FreeLook);
        }));
    }

    // Sanfte Kamerafahrt
    IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot, System.Action onComplete = null)
    {
        float t = 0f;
        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            // SmoothStep für weicheres Anfahren/Bremsen
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            mainCam.transform.position = Vector3.Lerp(startPos, targetPos, smoothT);
            mainCam.transform.rotation = Quaternion.Slerp(startRot, targetRot, smoothT);

            yield return null;
        }

        // Sicherstellen dass wir exakt ankommen
        mainCam.transform.position = targetPos;
        mainCam.transform.rotation = targetRot;

        onComplete?.Invoke();
    }
}