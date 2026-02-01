using UnityEngine;
using System; // Wichtig für 'Action' Events

public class GameManager : MonoBehaviour
{
    // 1. Singleton: Damit dein CameraController auf "GameManager.Instance" zugreifen kann
    public static GameManager Instance { get; private set; }

    // 2. State Management
    public GameState CurrentState { get; private set; }

    // 3. Event System: Andere Skripte können hier "zuhören" (Observer Pattern)
    // Das spart Performance, weil nicht jeder in Update() prüfen muss.
    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        // Singleton Initialisierung (Standard Pattern)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // Optional: DontDestroyOnLoad(gameObject); // Falls ihr Szenen wechselt
    }

    private void Start()
    {
        // Spiel beginnt im FreeLook Modus (oder Intro, je nach Wunsch)
        UpdateGameState(GameState.FreeLook);
    }

    // Zentrale Methode um den Zustand zu ändern
    public void UpdateGameState(GameState newState)
    {
        CurrentState = newState;

        // Debugging Hilfe für den Jam
        Debug.Log($"[GameManager] State gewechselt zu: {newState}");

        // Mauszeiger-Logik zentral steuern
        HandleCursor(newState);

        // Alle benachrichtigen, die es interessiert (UI, Player, Sounds)
        OnStateChanged?.Invoke(newState);
    }

    private void HandleCursor(GameState state)
    {
        switch (state)
        {
            case GameState.FreeLook:
            case GameState.Cutscene: // Bei Cutscenes meist auch keine Maus
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;

            case GameState.Dialogue:
            case GameState.ReadingLog:
            case GameState.GameOver:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }
}