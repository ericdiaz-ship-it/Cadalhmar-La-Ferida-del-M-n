using UnityEngine;

[CreateAssetMenu(fileName = "NouDialeg", menuName = "Dialegs/Sistema de Dialeg")]
public class DialegSO : ScriptableObject
{
    public LiniaDialeg[] linies; // El array de frases que definiste en tu struct
}