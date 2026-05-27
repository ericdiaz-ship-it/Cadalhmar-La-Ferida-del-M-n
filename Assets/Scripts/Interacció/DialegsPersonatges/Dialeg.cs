using UnityEngine;
using System.Reflection;

public class Dialeg : MonoBehaviour, IInteractuable
{
    [Header("Configuració Base")]
    public GestorDialegs gestor;

    [Header("Scriptable Objects de Diàlegs")]
    [Tooltip("El diàleg que es mostrarà el primer cop que parles amb ell.")]
    public DialegSO dialegPrimeraVegada;
    [Tooltip("Opcional: Diàleg que es mostra els cops posteriors.")]
    public DialegSO dialegRepetit;
    
    [Header("Diàleg per Variable Global")]
    [Tooltip("Opcional: Diàleg que prioritza si una variable global és 'true'.")]
    public DialegSO dialegEspecial;
    [Tooltip("Introdueix el nom exacte de la variable de VariablesGlobals (ex: CatherineCinematica)")]
    public string nomVariableGlobal;

    [Header("Variable Global a Activar al Final del Primer Diàleg")]
    [Tooltip("Opcional: Nom de la variable bool de VariablesGlobals que es posarà a TRUE quan acabi el primer diàleg.")]
    public string nomVariableActivarAlFinal;

    // Variables d'estat interns
    private bool jaHaParlar = false;

    public void Interactuar()
    {
        if (gestor == null) return;

        DialegSO dialegActiu = null;

        // 1. Condició per variable global (si s'ha configurat)
        bool condicioGlobal = ComprovarVariableGlobal();

        if (condicioGlobal && dialegEspecial != null)
        {
            dialegActiu = dialegEspecial;
        }
        // 2. Condició de si ja hem parlat amb ell abans
        else if (jaHaParlar && dialegRepetit != null)
        {
            dialegActiu = dialegRepetit;
        }
        // 3. Diàleg per defecte (primera vegada)
        else
        {
            // Per si l'únic que té és el dialegPrimeraVegada
            dialegActiu = dialegPrimeraVegada;

            // Si hi ha una variable configurada per activar, ens subscrivim al callback
            if (!string.IsNullOrEmpty(nomVariableActivarAlFinal))
            {
                gestor.onDialegAcabat += ActivarVariableAlFinalDialeg;
            }

            jaHaParlar = true; 
        }

        // Executem el diàleg que hagi sortit escollit si n'hi ha un d'assignat
        if (dialegActiu != null)
        {
            gestor.IniciarDialeg(dialegActiu);
        }
    }

    // Callback que es crida quan el primer diàleg acaba.
    // Posa la variable global configurada a TRUE.
    private void ActivarVariableAlFinalDialeg()
    {
        if (string.IsNullOrEmpty(nomVariableActivarAlFinal)) return;

        try
        {
            FieldInfo camp = typeof(VariablesGlobals).GetField(nomVariableActivarAlFinal, BindingFlags.Public | BindingFlags.Static);
            if (camp != null && camp.FieldType == typeof(bool))
            {
                camp.SetValue(null, true);
                Debug.Log("Dialeg: S'ha activat (posat a TRUE) la variable global '" + nomVariableActivarAlFinal + "' al final del primer diàleg.");
            }
            else
            {
                Debug.LogWarning("Dialeg: No s'ha trobat cap variable bool pública estàtica amb el nom '" + nomVariableActivarAlFinal + "' a VariablesGlobals.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error en activar la variable global al final del diàleg: " + e.Message);
        }
    }

    // Aquesta funció busca la variable de forma dinàmica pel seu nom text
    private bool ComprovarVariableGlobal()
    {
        if (string.IsNullOrEmpty(nomVariableGlobal)) return false;

        try
        {
            // Busquem el camp (variable) dins de VariablesGlobals
            FieldInfo camp = typeof(VariablesGlobals).GetField(nomVariableGlobal, BindingFlags.Public | BindingFlags.Static);
            
            // Si el trobem i és un 'bool', en retornem el valor
            if (camp != null && camp.FieldType == typeof(bool))
            {
                return (bool)camp.GetValue(null);
            }
            else
            {
                Debug.LogWarning("Dialeg: No s'ha trobat cap variable bool pública estètica amb el nom '" + nomVariableGlobal + "' a VariablesGlobals.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error en comprovar la variable global: " + e.Message);
        }

        return false;
    }
}