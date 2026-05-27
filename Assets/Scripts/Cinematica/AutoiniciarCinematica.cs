using System.Collections;
using UnityEngine;

public class AutoIniciarCinematica : MonoBehaviour
{
    public CinematicaDirector director;
    public CinematicaSO cinematica;

    [Header("Retard inicial (segons)")]
    public float retard = 0.1f; // Un mínim de 0.1 per deixar que la escena carregui

    [Header("Només una vegada (usa PlayerPrefs)")]
    public bool nomesMesUnaVegada = false;
    public string clauPlayerPrefs = "intro_vista";

    void Awake()
    {
        // Comprovacions en Awake, abans que qualsevol Start() s'executi
        if (director == null)
        {
            Debug.LogError("[AutoIniciar] ERROR: El camp 'Director' està buit! Assigna el CinematicaDirector a l'inspector.");
            return;
        }

        if (cinematica == null)
        {
            Debug.LogError("[AutoIniciar] ERROR: El camp 'Cinematica' està buit! Assigna el ScriptableObject a l'inspector.");
            return;
        }

        if (nomesMesUnaVegada && PlayerPrefs.GetInt(clauPlayerPrefs, 0) == 1)
        {
            Debug.Log("[AutoIniciar] Cinematica ja vista, no es torna a llançar.");
            return;
        }

        StartCoroutine(LlancarAmbRetard());
    }

    private IEnumerator LlancarAmbRetard()
    {
        Debug.Log($"[AutoIniciar] Esperant {retard}s abans de llançar '{cinematica.name}'...");
        yield return new WaitForSeconds(retard);

        if (nomesMesUnaVegada)
        {
            PlayerPrefs.SetInt(clauPlayerPrefs, 1);
            PlayerPrefs.Save();
        }

        Debug.Log($"[AutoIniciar] Llançant cinematica '{cinematica.name}'");
        director.IniciarCinematica(cinematica);
    }

    [ContextMenu("Resetear PlayerPref (debug)")]
    public void ResetearPlayerPref()
    {
        PlayerPrefs.DeleteKey(clauPlayerPrefs);
        Debug.Log($"[AutoIniciar] Clau '{clauPlayerPrefs}' esborrada. Torna a fer Play per veure la intro.");
    }

    [ContextMenu("Forçar cinematica ara (debug)")]
    public void ForçarCinematica()
    {
        if (director != null && cinematica != null)
            director.IniciarCinematica(cinematica);
        else
            Debug.LogError("[AutoIniciar] Falten referències per forçar la cinematica.");
    }
}