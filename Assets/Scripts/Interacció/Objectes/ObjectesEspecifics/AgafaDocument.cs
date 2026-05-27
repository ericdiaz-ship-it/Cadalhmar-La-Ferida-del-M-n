using UnityEngine;
//aquest objecte s'utilitzara per a objectes que es poden llegir, com un llibre o un diari, i al interactuar amb ell apareixera un text a la pantalla amb el contingut del llibre o diari
public class AgafaDocument : MonoBehaviour, IInteractuable
{
    [TextArea] public string textPrimeraIteraccio; //text que apareixera a la pantalla quan s'interactue amb l'objecte
    [TextArea] public string textSegonaIteraccio; //text que apareixera a la pantalla quan s'interactue amb l'objecte
    bool interactuat = false;
    [Header("Documents")]
    public string idDocument; //id del document que es pot agafar (compatible)
    public string[] idDocuments; //per si volem afegir varis documents a la vegada
    public GameObject objecte;
    public void Interactuar()
    {
        if(!interactuat)
        {
            ObjecteDialegManager.instance.mostrarDialeg(textPrimeraIteraccio); //mostra el text a la pantalla
            interactuat = true;
            if(!string.IsNullOrEmpty(idDocument))
            {
                VariablesGlobals.AfegirDocument(idDocument);
            }
            else if (idDocuments != null && idDocuments.Length > 0)
            {
                VariablesGlobals.AfegirDocuments(idDocuments);
            }
            if(objecte != null)
            {
                Destroy(objecte); //destrueix l'objecte que es pot agafar
            }
            
        }
        else
        {
            ObjecteDialegManager.instance.mostrarDialeg(textSegonaIteraccio); //mostra el text a la pantalla
        }
    }
}
