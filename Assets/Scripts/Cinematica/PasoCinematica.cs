using UnityEngine;

public enum TipusPas { MourePJ, MoureActor, Dialeg, Espera }

[System.Serializable]
public struct PasoCinematica
{
    public TipusPas tipusPas;

    [Header("Mover — escriu el nom exacte del GameObject de la escena")]
    public string nomActor;     // Nombre del NPC en la jerarquía
    public string nomDestí;     // Nombre del punto destino en la jerarquía
    public float velocitat;

    [Header("Diàleg")]
    public DialegSO dialeg;     // Esto SÍ se puede arrastrar (es un asset del Project)

    [Header("Espera")]
    public float tempsEspera;
}