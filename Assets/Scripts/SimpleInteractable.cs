using UnityEngine;
using UnityEngine.Events;

public class SimpleInteractable : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    public string promptText = "Interagieren";

    [Header("Highlight Settings")]
    public Renderer targetRenderer; // Das Mesh der Pistole

    [Tooltip("Die Farbe, in der es leuchten soll (z.B. helles Gelb)")]
    [ColorUsage(true, true)] // Erlaubt HDR Farben (für extra helles Leuchten)
    public Color highlightColor = new Color(0.2f, 0.2f, 0.2f);

    // Speicher für die Original-Zustände aller Materials
    private Material[] cachedMaterials;
    private Color[] originalEmissionColors;

    [Header("Was soll passieren?")]
    public UnityEvent onInteract;

    private void Start()
    {
        // Wenn kein Renderer zugewiesen, nimm den eigenen
        if (targetRenderer == null) targetRenderer = GetComponent<Renderer>();

        if (targetRenderer != null)
        {
            // Wir holen uns ALLE Materials (als Instanz-Kopie, damit wir andere Pistolen nicht ändern)
            cachedMaterials = targetRenderer.materials;

            // Arrays initialisieren
            originalEmissionColors = new Color[cachedMaterials.Length];

            for (int i = 0; i < cachedMaterials.Length; i++)
            {
                // Merke dir die aktuelle Farbe (wahrscheinlich Schwarz)
                if (cachedMaterials[i].HasProperty("_EmissionColor"))
                {
                    originalEmissionColors[i] = cachedMaterials[i].GetColor("_EmissionColor");
                }

                // Stelle sicher, dass das Keyword an ist (falls Schritt 1 vergessen wurde)
                cachedMaterials[i].EnableKeyword("_EMISSION");
            }
        }
    }

    public void OnHoverEnter()
    {
        if (cachedMaterials == null) return;

        // Gehe durch JEDES Material und mach es hell
        foreach (var mat in cachedMaterials)
        {
            mat.SetColor("_EmissionColor", highlightColor);
        }
    }

    public void OnHoverExit()
    {
        if (cachedMaterials == null) return;

        // Setze alles zurück auf den Originalzustand
        for (int i = 0; i < cachedMaterials.Length; i++)
        {
            cachedMaterials[i].SetColor("_EmissionColor", originalEmissionColors[i]);
        }
    }

    public void OnInteract()
    {
        onInteract.Invoke();
    }

    public string GetDescription()
    {
        return promptText;
    }
}