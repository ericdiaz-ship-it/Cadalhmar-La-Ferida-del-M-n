using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GestorDialegs : MonoBehaviour
{
    [Header("Referències de la UI")]
    public GameObject contenidorDialeg; 
    public TextMeshProUGUI textNom;     
    public TextMeshProUGUI textDialeg;  
    public RawImage imatgePersonatge;      

    private Queue<LiniaDialeg> cuaDialegs; 

    void Start()
    {
        cuaDialegs = new Queue<LiniaDialeg>();
        contenidorDialeg.SetActive(false); 
    }

    void Update()
    {
        // Pasar línea con Espacio o Click Izquierdo
        if (contenidorDialeg.activeInHierarchy && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            MostrarSeguentLinia();
        }
    }

    // Aquesta funció rep el ScriptableObject
    public void IniciarDialeg(DialegSO dades)
    {
        if (dades == null || dades.linies.Length == 0) return;

        contenidorDialeg.SetActive(true);
        cuaDialegs.Clear();

        foreach (LiniaDialeg linia in dades.linies)
        {
            cuaDialegs.Enqueue(linia);
        }

        MostrarSeguentLinia();
    }

    public void MostrarSeguentLinia()
    {
        if (cuaDialegs.Count == 0)
        {
            FinalitzarDialeg();
            return;
        }

        LiniaDialeg liniaActual = cuaDialegs.Dequeue();

        textNom.text = liniaActual.nomPersonatge;
        textDialeg.text = liniaActual.textDialeg;

        if (liniaActual.imatgePersonatge != null)
        {
            imatgePersonatge.texture = liniaActual.imatgePersonatge.texture;
            imatgePersonatge.enabled = true;
        }
        else
        {
            imatgePersonatge.enabled = false;
        }
    }

    void FinalitzarDialeg()
    {
        contenidorDialeg.SetActive(false);
        Debug.Log("Diàleg acabat!");
    }
}