using UnityEngine;

public enum TipusPas { MourePJ, MoureActor, Dialeg, Espera, PosarVariableGlobal, CanviarEscena, VisibilitatActor }

[System.Serializable]
public struct PasoCinematica
{
    public TipusPas tipusPas;

    [Header("Mover — escriu el nom exacte del GameObject de la escena")]
    public string nomActor;     // Nom del NPC a la jerarquia
    public string nomDestí;     // Nom del punt de destí a la jerarquia
    public float velocitat;

    [Header("Diàleg")]
    public DialegSO dialeg;     // Això sí que es pot arrossegar (és un asset del projecte)

    [Header("Espera")]
    public float tempsEspera;

    [Header("Visibilitat (Requereix 'nomActor')")]
    public bool ferVisible;

    [Header("Posar Variable Global a TRUE")]
    [Tooltip("Introdueix el nom exacte de la variable bool de VariablesGlobals que es posarà a true.")]
    public string nomVariableGlobal;

    [Header("Canviar Escena")]
    [Tooltip("Nom de l'escena on anirem després d'aquest pas.")]
    public string escenaACanviar;
}