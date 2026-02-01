using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerminalUI : MonoBehaviour
{
    [Header("UI Elemente")]
    public TextMeshProUGUI displayText;   // Dein TerminalText
    public ScrollRect scrollRect;         // Dein Scroll View
    public Button[] categoryButtons;      // Alle 4 Buttons

    [Header("Einstellungen")]
    public float typingSpeed = 0.03f;     // Buchstaben pro Sekunde
    public float cursorBlinkSpeed = 0.5f; // Cursor blink Intervall

    private Coroutine typingCoroutine;
    private Coroutine cursorCoroutine;
    private const string cursorChar = "▋";
    private bool isTyping = false;

    // --- Start Text für Test / Terminal bereit ---
    void Start()
    {
        displayText.text = "Terminal bereit...\nKlicke eine Kategorie, um Text zu sehen.";
    }

    // --- Hauptfunktion, ruft den Text für die Buttons auf ---
    public void ShowCategory(string text)
    {
        if (isTyping) return; // Wenn Text gerade läuft, nix tun

        StopAllCoroutines();
        typingCoroutine = StartCoroutine(TypeText(text));
    }

    // --- Typewriter Effekt ---
    IEnumerator TypeText(string text)
    {
        isTyping = true;
        SetButtonsInteractable(false);

        displayText.text = "";

        foreach (char c in text)
        {
            displayText.text += c + cursorChar;
            yield return new WaitForSeconds(typingSpeed);
            displayText.text = displayText.text.Replace(cursorChar, "");

            if (scrollRect != null)
                scrollRect.verticalNormalizedPosition = 0f; // Scroll immer unten
        }

        // Cursor blinken lassen
        cursorCoroutine = StartCoroutine(BlinkCursor());

        SetButtonsInteractable(true);
        isTyping = false;
    }

    // --- Blinkender Cursor ---
    IEnumerator BlinkCursor()
    {
        while (true)
        {
            displayText.text += cursorChar;
            yield return new WaitForSeconds(cursorBlinkSpeed);
            displayText.text = displayText.text.Replace(cursorChar, "");
            yield return new WaitForSeconds(cursorBlinkSpeed);
        }
    }

    // --- Buttons sperren/freigeben ---
    void SetButtonsInteractable(bool state)
    {
        foreach (Button btn in categoryButtons)
            btn.interactable = state;
    }

    // --- Beispieltexte für 4 Kategorien ---
    public void ShowCategory1() => ShowCategory(
        "SYSTEM LOG\n----------------\nBoot sequence complete.\nAll systems nominal.\nNo anomalies detected."
    );

    public void ShowCategory2() => ShowCategory(
        "CREW INFORMATION\n----------------\nCaptain: Missing\nEngineer: Active\nMedical: Standby\nAll personnel accounted for."
    );

    public void ShowCategory3() => ShowCategory(
        "SECURITY PROTOCOLS\n----------------\nAccess Level: Granted\nUnauthorized attempts: 0\nSurveillance online.\nAll systems secure."
    );

    public void ShowCategory4() => ShowCategory(
        "MAINTENANCE REPORT\n----------------\nLife support: Functional\nPower systems: Stable\nReactor temperature: Nominal\nMinor repairs required in Deck 3."
    );
}
