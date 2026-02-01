using UnityEngine;

public class StoryDirector : MonoBehaviour
{
    [Header("Actors")]
    public GameObject grigsbyInside;
    public GameObject grigsbyOutside;

    [Header("Ink Files")]
    public TextAsset introInkJSON;

    void Start()
    {
        // Am Anfang alle verstecken
        if (grigsbyInside) grigsbyInside.SetActive(false);
        if (grigsbyOutside) grigsbyOutside.SetActive(false);

        // Auf Ink hören für "RevealOutside" Event im Intro
        DialogueManager.Instance.OnGameEvent += OnDirectorEvent;
    }

    public void StartIntroScene()
    {
        // 1. Grigsby kommt rein
        if (grigsbyInside) grigsbyInside.SetActive(true);

        // 2. Dialog startet
        DialogueManager.Instance.StartDialogue(introInkJSON);
    }

    void OnDirectorEvent(string eventName)
    {
        if (eventName == "RevealOutside")
        {
            if (grigsbyOutside) grigsbyOutside.SetActive(true);
        }
    }
}