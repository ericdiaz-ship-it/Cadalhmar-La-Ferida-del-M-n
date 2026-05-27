using UnityEngine;
using System.Reflection;

//aquest objecte comprova una variable global:
//si és TRUE utilitza SceneTeleport per teletransportar a una altra escena.
//si és FALSE mostra un diàleg d'objecte.
public class ObjecteVariableTeletrasnportacio : MonoBehaviour, IInteractuable
{
    [Header("Variable Global a Comprovar")]
    [Tooltip("Introdueix el nom exacte de la variable bool de VariablesGlobals (ex: parlaSacerdot)")]
    public string nomVariableGlobal;

    [Header("Diàleg (quan la variable és FALSE)")]
    [TextArea, Tooltip("El text que es mostrarà quan la variable sigui FALSE.")]
    public string textVariableFalse;

    [Header("Teletransport (quan la variable és TRUE)")]
    [Tooltip("Nom de l'escena de destí (ha de coincidir amb el nom al Build Settings).")]
    [SerializeField] private string nombreEscenaDestino;

    [Tooltip("ID del punt d'entrada a l'escena destí on apareixerà el jugador.")]
    [SerializeField] private string puntoEntradaID;

    public void Interactuar()
    {
        bool valorVariable = ComprovarVariableGlobal();

        if (valorVariable)
        {
            // La variable és true, teletransportem a l'escena destí
            if (LevelManager.Instance == null)
            {
                Debug.LogWarning("ObjecteVariableTeletrasnportacio: LevelManager no trobat.");
                return;
            }

            if (string.IsNullOrWhiteSpace(nombreEscenaDestino) || string.IsNullOrWhiteSpace(puntoEntradaID))
            {
                Debug.LogWarning("ObjecteVariableTeletrasnportacio: Escena o punt d'entrada no configurat.");
                return;
            }

            LevelManager.Instance.CarregarEscena(nombreEscenaDestino, puntoEntradaID);
        }
        else
        {
            // La variable és false, mostrem el diàleg
            if (ObjecteDialegManager.instance != null && !string.IsNullOrEmpty(textVariableFalse))
            {
                ObjecteDialegManager.instance.mostrarDialeg(textVariableFalse);
            }
        }
    }

    // Comprova si la variable global és true o false
    private bool ComprovarVariableGlobal()
    {
        if (string.IsNullOrEmpty(nomVariableGlobal)) return false;

        try
        {
            FieldInfo camp = typeof(VariablesGlobals).GetField(nomVariableGlobal, BindingFlags.Public | BindingFlags.Static);
            if (camp != null && camp.FieldType == typeof(bool))
            {
                return (bool)camp.GetValue(null);
            }
            else
            {
                Debug.LogWarning("ObjecteVariableTeletrasnportacio: No s'ha trobat cap variable bool pública estàtica amb el nom '" + nomVariableGlobal + "' a VariablesGlobals.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("ObjecteVariableTeletrasnportacio: Error en comprovar la variable global: " + e.Message);
        }
        return false;
    }
}
