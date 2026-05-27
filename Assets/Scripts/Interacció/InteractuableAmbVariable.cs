using UnityEngine;
using System.Reflection;

public class InteractuableAmbVariable : MonoBehaviour, IInteractuable
{
    [Header("Textos de Lectura")]
    [TextArea, Tooltip("El text que es mostrarà el primer cop.")]
    public string textPrimeraVegada;
    
    [TextArea, Tooltip("El text que es mostrarà a partir del segon cop.")]
    public string textRepetit;
    
    [Header("Variable Global a Activar")]
    [Tooltip("Introdueix el nom exacte de la variable de VariablesGlobals (ex: paper)")]
    public string nomVariableGlobal;

    public void Interactuar()
    {
        // Comprovem quin és l'estat actual de la variable global
        bool variableJaEstaActiva = ComprovarVariableGlobal();

        if (!variableJaEstaActiva)
        {
            // Si la variable està a false (és el primer cop que ho llegim)
            ActivarVariableGlobal(); // La posem a true

            if (ObjecteDialegManager.instance != null && !string.IsNullOrEmpty(textPrimeraVegada))
            {
                ObjecteDialegManager.instance.mostrarDialeg(textPrimeraVegada);
            }
        }
        else
        {
            // Si la variable ja està a true (ja ho havíem llegit abans)
            if (ObjecteDialegManager.instance != null && !string.IsNullOrEmpty(textRepetit))
            {
                ObjecteDialegManager.instance.mostrarDialeg(textRepetit);
            }
        }
    }

    // Aquesta funció comprova si la variable és true o false
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
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error en comprovar la variable global: " + e.Message);
        }
        return false;
    }

    // Aquesta funció posa la variable global a true
    private void ActivarVariableGlobal()
    {
        if (string.IsNullOrEmpty(nomVariableGlobal)) return;

        try
        {
            FieldInfo camp = typeof(VariablesGlobals).GetField(nomVariableGlobal, BindingFlags.Public | BindingFlags.Static);
            if (camp != null && camp.FieldType == typeof(bool))
            {
                camp.SetValue(null, true);
                Debug.Log("S'ha activat (posat a TRUE) la variable global: " + nomVariableGlobal);
            }
            else
            {
                Debug.LogWarning("Interactuable: No s'ha trobat cap variable bool pública estàtica amb el nom '" + nomVariableGlobal + "' a VariablesGlobals.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error en escriure a la variable global: " + e.Message);
        }
    }
}
