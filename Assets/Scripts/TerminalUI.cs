using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerminalUI : MonoBehaviour
{
    [Header("UI Elemente")]
    public TextMeshProUGUI displayText;
    public ScrollRect scrollRect;
    public Button[] categoryButtons; // 0=Warning, 1=Log86, 2=Crew, 3=Scan

    [Header("Settings")]
    public float typingSpeed = 0.01f;
    private bool isTyping = false;

    // --- Die Story Texte ---
    // Index 0: Warning
    // Index 1: Log 86
    // Index 2: Crew (Unlock nach Intro)
    // Index 3: Scan (Unlock nach Befragung)
    private string[] logContents = new string[] {
        // Log 0: System Warning
        "SYSTEM WARNING - 37:86:X35\n" +
        "--------------------------\n" +
        "CRITICAL ERROR\n" +
        "Contact to Outer Crane severed.\n" +
        "Major Malfunction suspected.\n" +
        "Action required immediately.",

        // Log 1: Cycle Log 86
        "CYCLE LOG 86 - YEAR X35\n" +
        "-----------------------\n" +
        "35:00 - Outer Crane took a hit.\n" +
        "36:00 - Stopped for inspection. Unknown object.\n" +
        "37:00 - Connection to Crane lost.\n\n" +
        "Note: Sent Grigsby outside to fix it. He insisted. " +
        "Might take a while.",

        // Log 2: Crew Info
        "THE GENBUS CREW\n" +
        "----------------\n" +
        "Editor: S.Faed\n\n" +
        "ID#2: S. Faed\n" +
        "- Age: 47\n" +
        "- Blood Type: 0\n" +
        "- 29 Years Flight Exp.\n" +
        "- 35 Years Mech Exp.\n" +
        "- Joined: 02:X5\n\n" +
        "ID#5: B.Grigsby\n" +
        "- Age: 23\n" +
        "- Blood Type: Gamma\n" +
        "- 4 Years Exp. Antiquity Store\n" +
        "- 2 Years Flight Exp.\n" +
        "- Joined: 93:X34",

        // Log 3: Ship Scan (Der Clue!)
        "SHIP SCAN - 00:87:X35\n" +
        "---------------------\n" +
        "DIAGNOSIS COMPLETE\n" +
        "All Systems Nominal.\n\n" +
        "Resolutions:\n" +
        "- Connection to Outer Crane RE-ESTABLISHED.\n" +
        "- Structural Integrity: 100%"
    };

    void Start()
    {
        displayText.text = "Welcome, Faed.\nChoose a Log.";

        // Startzustand: Nur Warning (0) und Log86 (1) offen
        UnlockLog(0);
        UnlockLog(1);
        LockLog(2); // Crew gesperrt
        LockLog(3); // Scan gesperrt
    }

    public void OnClickLog(int index)
    {
        if (index >= 0 && index < logContents.Length)
        {
            StopAllCoroutines();
            StartCoroutine(TypeText(logContents[index]));
        }
    }

    public void UnlockLog(int index)
    {
        if (index < categoryButtons.Length)
        {
            categoryButtons[index].interactable = true;
            // Visuelles Feedback: Volle Deckkraft
            var text = categoryButtons[index].GetComponentInChildren<TextMeshProUGUI>();
            if (text) text.alpha = 1f;
        }
    }

    public void LockLog(int index)
    {
        if (index < categoryButtons.Length)
        {
            categoryButtons[index].interactable = false;
            // Visuelles Feedback: Ausgegraut
            var text = categoryButtons[index].GetComponentInChildren<TextMeshProUGUI>();
            if (text) text.alpha = 0.3f;
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        displayText.text = "";
        foreach (char c in text)
        {
            displayText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}