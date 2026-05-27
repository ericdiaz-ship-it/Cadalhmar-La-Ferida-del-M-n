using UnityEngine;

[CreateAssetMenu(fileName = "NovaCinematica", menuName = "Cinematiques/Seqüència")]
public class CinematicaSO : ScriptableObject
{
    public PasoCinematica[] passos;
    public bool posarVariableGlobal = false; // Set true to activate global variable at end
    public string nomVariableGlobal = ""; // Name of global bool in VariablesGlobals
}