using UnityEngine;

public class Dialeg : MonoBehaviour, IInteractuable
{
    public DialegSO dades; // L'arxiu ScriptableObject amb els textos
    public GestorDialegs gestor; // Arrossega el Gestor de l'escena aquí

    public void Interactuar()
    {


        if (dades != null && gestor != null)
        {
            gestor.IniciarDialeg(dades);
        }
    }
}