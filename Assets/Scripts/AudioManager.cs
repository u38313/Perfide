using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Global Sounds")]
    public AudioSource musicSource;
    public AudioSource sfxSource; // Für 2D Sounds (UI Klicks, etc.)

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null) sfxSource.PlayOneShot(clip, volume);
    }

    // Hilfsfunktion um Sounds an einer 3D Position zu erzeugen
    public void PlaySoundAtPosition(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;
        // Erzeugt ein temporäres Objekt, spielt den Sound, und zerstört sich danach
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }
}