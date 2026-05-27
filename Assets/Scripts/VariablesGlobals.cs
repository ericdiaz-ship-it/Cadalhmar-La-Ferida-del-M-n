using UnityEngine;
using System.Collections.Generic;

public class VariablesGlobals 
{
    public static int pocions = 0; //variable global que conte el nombre de pocions que te el jugador
    public static List<string> documentsRecollits = new List<string>(); //variable global que conte els id dels documents que te el jugador
    // Afegeix un document a la llista global si no existeix ja.
    // Retorna true si s'ha afegit, false si ja existia o l'id no és vàlid.
    // Afegir un document; per defecte NO obre l'inventari.
    // Passa `obrirInventari=true` si vols obrir-lo explícitament.
    public static bool AfegirDocument(string id, bool obrirInventari = false)
    {
        if (string.IsNullOrEmpty(id)) return false;
        if (documentsRecollits.Contains(id)) return false;

        documentsRecollits.Add(id);

        // No obre l'inventari automàticament tret que se sol·liciti
        if (obrirInventari && InventariManager.instance != null)
            InventariManager.instance.ObreInventari(id);

        return true;
    }

    // Afegir una llista d'identificadors. No obre l'inventari per cada un; si
    // `obrirInventari` és true s'obrirà una sola vegada amb el primer document afegit.
    public static int AfegirDocuments(IEnumerable<string> ids, bool obrirInventari = false)
    {
        if (ids == null) return 0;

        string primerAfegit = null;
        int comptador = 0;

        foreach (var id in ids)
        {
            if (string.IsNullOrEmpty(id)) continue;
            if (documentsRecollits.Contains(id)) continue;

            documentsRecollits.Add(id);
            if (primerAfegit == null) primerAfegit = id;
            comptador++;
        }

        if (obrirInventari && primerAfegit != null && InventariManager.instance != null)
            InventariManager.instance.ObreInventari(primerAfegit);

        return comptador;
    }
}
