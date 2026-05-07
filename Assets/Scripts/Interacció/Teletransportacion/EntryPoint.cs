using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [Tooltip("L'identificador que coincideix amb el 'puntoEntradaID' del script de teletransport")]
    public string ID; 

    // Opcional: Dibuixa una petita esfera a l'editor per veure on està el punt
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}