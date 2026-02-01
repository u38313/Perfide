using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class IntroSequence : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup blackScreenFade; // Das schwarze Bild
    public GameObject objectiveText;    // "Sieh nach dem Geräusch"

    [Header("Audio")]
    public AudioSource knockingSource;  // Die AudioSource links vom Spieler
    public float delayBeforeKnock = 10f; // Wie lange Ruhe am Anfang?

    [Header("Settings")]
    public float fadeDuration = 0.75f;

    private void Start()
    {
        // 1. Setup: Alles schwarz, Text aus
        blackScreenFade.alpha = 1f;
        objectiveText.SetActive(false);

        // Startet die Sequenz
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        // Warten damit Unity alles geladen hat
        yield return new WaitForSeconds(0.5f);

        // 2. Fade In (Schwarz zu Transparent)
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            blackScreenFade.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }
        blackScreenFade.alpha = 0f;
        // Wichtig: Raycasts blockieren ausschalten, damit man klicken kann
        blackScreenFade.blocksRaycasts = false;

        // 3. Warten am Terminal
        yield return new WaitForSeconds(delayBeforeKnock);

        // 4. Das Klopfen abspielen
        if (knockingSource != null)
        {
            knockingSource.Play();
        }

        // 5. Hinweis einblenden
        objectiveText.SetActive(true);

        // Optional: Nach 5 Sekunden Hinweis wieder ausblenden
        yield return new WaitForSeconds(5f);
        objectiveText.SetActive(false);
    }
}