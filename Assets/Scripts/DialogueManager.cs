using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public event Action<string> OnGameEvent;

    [Header("Ink Setup")]
    public TextAsset inkJSONAsset; // Hier die kompilierte .json von Ink reinziehen
    private Story story;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;
    public GameObject choiceButtonPrefab;
    public Transform choiceContainer;

    [Header("Camera Control")]
    public float turnSpeed = 5f;
    private Transform currentCameraTarget;
    private Quaternion originalRotation; // Um nach dem Dialog zurückzusetzen (optional)

    // Liste von Positionen, die Ink per Name ansteuern kann
    // Z.B. "Grigsby" -> Transform von Grigsby
    [System.Serializable]
    public struct CameraTarget
    {
        public string id;
        public Transform target;
    }
    public List<CameraTarget> cameraTargets;

    private bool isDialogueActive = false;

    private void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        // Wenn Dialog läuft, Kamera zum Ziel drehen
        if (isDialogueActive && currentCameraTarget != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentCameraTarget.position - Camera.main.transform.position);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }

    // --- Aufruf von Außen ---
    public void StartDialogue(TextAsset inkJSON)
    {
        story = new Story(inkJSON.text);

        // GameState ändern (Maus sichtbar, Bewegung aus)
        GameManager.Instance.UpdateGameState(GameState.Dialogue);

        isDialogueActive = true;
        dialoguePanel.SetActive(true);

        RefreshView();
    }

    void RefreshView()
    {
        // 1. Alte Buttons löschen
        foreach (Transform child in choiceContainer) Destroy(child.gameObject);

        // 2. Text anzeigen (falls vorhanden)
        if (story.canContinue)
        {
            string text = story.Continue();
            dialogueText.text = text;
            HandleTags(story.currentTags);
        }

        // 3. Prüfen: Sind wir am Ende ODER gibt es Choices?
        if (story.currentChoices.Count > 0)
        {
            // Es gibt echte Entscheidungen -> Buttons erstellen
            foreach (Choice choice in story.currentChoices)
            {
                CreateChoiceButton(choice.text, () => {
                    story.ChooseChoiceIndex(choice.index);
                    RefreshView();
                });
            }
        }
        else if (story.canContinue)
        {
            // Es gibt KEINE Entscheidungen, aber noch Text im Puffer -> "Weiter" Button
            // Das passiert, wenn Ink Textblöcke hat, die nicht sofort Entscheidungen sind
            CreateChoiceButton("Weiter...", () => RefreshView());
        }
        else
        {
            // Kein Text mehr, keine Entscheidungen mehr -> Ende
            EndDialogue();
        }
    }

    void HandleTags(List<string> tags)
    {
        foreach (string t in tags)
        {
            string[] splitTag = t.Split(':');
            if (splitTag.Length != 2) continue;

            string key = splitTag[0].Trim();
            string value = splitTag[1].Trim();

            switch (key)
            {
                case "camera":
                    SetCameraTarget(value);
                    break;
                case "speaker":
                    speakerNameText.text = value;
                    break;
                case "audio":
                    // AudioManager.Instance.PlaySFX(value); // Falls du das nutzt
                    break;

                // --- NEU: Der Event Case ---
                case "event":
                    // Sendet den Befehl an alle, die zuhören (z.B. StoryDirector)
                    OnGameEvent?.Invoke(value);
                    Debug.Log($"[Ink Event] {value}");
                    break;
                    // ---------------------------
            }
        }
    }

    void SetCameraTarget(string targetID)
    {
        // Suche in der Liste nach dem Ziel
        foreach (var t in cameraTargets)
        {
            if (t.id == targetID)
            {
                currentCameraTarget = t.target;
                return;
            }
        }
    }

    void CreateChoiceButton(string text, UnityEngine.Events.UnityAction onClick)
    {
        GameObject button = Instantiate(choiceButtonPrefab, choiceContainer);
        button.GetComponentInChildren<TextMeshProUGUI>().text = text;
        button.GetComponent<Button>().onClick.AddListener(onClick);
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        currentCameraTarget = null;

        // Zurück zum Spiel
        GameManager.Instance.UpdateGameState(GameState.FreeLook);
    }
}