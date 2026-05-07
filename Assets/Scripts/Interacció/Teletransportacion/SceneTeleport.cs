using UnityEngine;

/// <summary>
/// Trigger de teletransport a una altra escena.
/// Delega tota la lògica al LevelManager per mantenir una única
/// font de veritat sobre l'estat de les transicions.
/// </summary>
public class SceneTeleport : MonoBehaviour, IInteractuable
{
    [Header("Configuració de Destí")]
    [Tooltip("Nom de l'escena de destí (ha de coincidir amb el nom al Build Settings).")]
    [SerializeField] private string nombreEscenaDestino;

    [Tooltip("ID del punt d'entrada a l'escena destí on apareixerà el jugador.")]
    [SerializeField] private string puntoEntradaID;

    // ─────────────────────────────────────────────────────────────────────────
    // VALIDACIÓ EN EDITOR
    // ─────────────────────────────────────────────────────────────────────────

    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(nombreEscenaDestino))
            Debug.LogWarning($"[SceneTeleport] '{gameObject.name}': nombreEscenaDestino és buit.", this);

        if (string.IsNullOrWhiteSpace(puntoEntradaID))
            Debug.LogWarning($"[SceneTeleport] '{gameObject.name}': puntoEntradaID és buit.", this);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // INTERACCIÓ
    // ─────────────────────────────────────────────────────────────────────────

    public void Interactuar()
    {
        if (LevelManager.Instance == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(nombreEscenaDestino) || string.IsNullOrWhiteSpace(puntoEntradaID))
        {
            return;
        }

        LevelManager.Instance.CarregarEscena(nombreEscenaDestino, puntoEntradaID);
    }
}