using System;
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

    // Callback que es crida quan un diàleg acaba.
    // Qualsevol script pot subscriure's per reaccionar al final del diàleg.
    public event Action onDialegAcabat;

    private Queue<LiniaDialeg> cuaDialegs; 

    void Start()
    {
        cuaDialegs = new Queue<LiniaDialeg>();
        contenidorDialeg.SetActive(false); 
    }

    void Update()
    {
        // Avança la línia amb Espai o clic esquerre
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

        // Notifiquem a tots els subscriptors que el diàleg ha acabat
        onDialegAcabat?.Invoke();
        onDialegAcabat = null; // Netejem els callbacks per evitar que es cridin en diàlegs futurs
    }
}