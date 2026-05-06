using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour, IInteractuable
{
    [Header("Configuración de Destino")]
    [SerializeField] private string nombreEscenaDestino;
    [SerializeField] private string puntoEntradaID; // ID per saber a qué punt hem de transportar al personatge

    public void Interactuar()
    {
        // Cridem al LevelManager per carregar la nova escena i transportar al personatge al punt d'entrada corresponent
        LevelManager.Instance.CarregarEscena(nombreEscenaDestino, puntoEntradaID);
    }
}