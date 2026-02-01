using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerminalUI : MonoBehaviour
{
    [Header("UI Elemente")]
    public TextMeshProUGUI displayText;
    public Button[] categoryButtons; // 0=Warning, 1=Log86, 2=Crew, 3=Scan

    [Header("Settings")]
    public float typingSpeed = 0.005f; // Etwas schneller für besseren Flow

    // Die finalen Story-Texte
    private string[] logContents = new string[] {
        // [0] WARNING (Start verfügbar)
        "SYSTEM WARNING - 37:86:X35\n" +
        "--------------------------\n" +
        "CRITICAL ALERT\n\n" +
        "Contact to Outer Crane severed.\n" +
        "Major Malfunction suspected.\n" +
        "Action required immediately.",

        // [1] LOG 86 (Start verfügbar)
        "CYCLE LOG 86 - YEAR X35\n" +
        "-----------------------\n" +
        "35:00 - Outer Crane took a hit.\n" +
        "36:00 - Stopped for inspection. Unknown object collided.\n" +
        "37:00 - Connection to Crane lost.\n\n" +
        "Note: Sent Grigsby outside to fix it. He insisted. " +
        "Might take a while.",

        // [2] CREW (Nach Intro verfügbar)
        "THE GENBUS CREW\n" +
        "----------------\n" +
        "ID#2: S. Faed (You)\n" +
        "- Age: 47, Mech Exp: 35 Years\n" +
        "- Origin: Colony 4\n\n" +
        "ID#5: B. Grigsby\n" +
        "- Age: 23\n" +
        "- Origin: Earth (Antique Store)\n" +
        "- Personality: Insecure, loves old books.\n" +
        "- Joined: Cycle 93:X34",

        // [3] SCAN (Nach Widerspruch verfügbar)
        "SHIP SCAN - 00:87:X35\n" +
        "---------------------\n" +
        "DIAGNOSIS COMPLETE\n\n" +
        "All Systems Nominal.\n" +
        "Resolutions:\n" +
        "- Connection to Outer Crane RE-ESTABLISHED.\n" +
        "- Hull Integrity: 100%"
    };

    void Start()
    {
        displayText.text = "Welcome, Faed.\nSelect a log entry.";
        // Startzustand: Nur 0 und 1 offen
        SetLogState(0, true);
        SetLogState(1, true);
        SetLogState(2, false);
        SetLogState(3, false);
    }

    public void OnClickLog(int index)
    {
        if (index >= 0 && index < logContents.Length)
        {
            StopAllCoroutines();
            StartCoroutine(TypeText(logContents[index]));
        }
    }

    // Zentrale Funktion zum Sperren/Entsperren
    public void UnlockLog(int index) => SetLogState(index, true);

    private void SetLogState(int index, bool unlocked)
    {
        if (index < categoryButtons.Length)
        {
            categoryButtons[index].interactable = unlocked;
            var txt = categoryButtons[index].GetComponentInChildren<TextMeshProUGUI>();
            if (txt) txt.alpha = unlocked ? 1f : 0.3f;
        }
    }

    IEnumerator TypeText(string text)
    {
        displayText.text = "";
        foreach (char c in text)
        {
            displayText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}