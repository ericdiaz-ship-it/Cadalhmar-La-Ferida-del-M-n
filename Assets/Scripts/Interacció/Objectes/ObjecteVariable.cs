using UnityEngine;
using System.Reflection;

//aquest objecte comprova una variable global: si és false mostra un diàleg, si és true en mostra un altre.
//Opcionalment pot eliminar l'objecte després de mostrar el diàleg.
public class ObjecteVariable : MonoBehaviour, IInteractuable
{
    [Header("Variable Global a Comprovar")]
    [Tooltip("Introdueix el nom exacte de la variable bool de VariablesGlobals (ex: paper)")]
    public string nomVariableGlobal;

    [Header("Textos de Diàleg")]
    [TextArea, Tooltip("El text que es mostrarà quan la variable sigui FALSE.")]
    public string textVariableFalse;

    [TextArea, Tooltip("El text que es mostrarà quan la variable sigui TRUE.")]
    public string textVariableTrue;

    [Header("Eliminació de l'Objecte")]
    [Tooltip("Si està activat, l'objecte s'eliminarà després de mostrar el diàleg.")]
    public bool eliminarObjecte = false;

    public void Interactuar()
    {
        bool valorVariable = ComprovarVariableGlobal();

        if (!valorVariable)
        {
            // La variable és false, mostrem el diàleg corresponent
            if (ObjecteDialegManager.instance != null && !string.IsNullOrEmpty(textVariableFalse))
            {
                ObjecteDialegManager.instance.mostrarDialeg(textVariableFalse);
            }
        }
        else
        {
            // La variable és true, mostrem el diàleg corresponent
            if (ObjecteDialegManager.instance != null && !string.IsNullOrEmpty(textVariableTrue))
            {
                ObjecteDialegManager.instance.mostrarDialeg(textVariableTrue);
            }
        }

        // Destruïm l'objecte només si la variable global és true i s'ha indicat eliminarObjecte
        if (eliminarObjecte && valorVariable)
        {
            Destroy(gameObject);
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
                Debug.LogWarning("InteraccioObjectes: No s'ha trobat cap variable bool pública estàtica amb el nom '" + nomVariableGlobal + "' a VariablesGlobals.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("InteraccioObjectes: Error en comprovar la variable global: " + e.Message);
        }
        return false;
    }
}
