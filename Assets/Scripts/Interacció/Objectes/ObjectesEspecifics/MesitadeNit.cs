using UnityEngine;
//aquest objecte s'utilitzara per a objectes que es poden llegir, com un llibre o un diari, i al interactuar amb ell apareixera un text a la pantalla amb el contingut del llibre o diari
public class MesitadeNit : MonoBehaviour, IInteractuable
{
    [TextArea] public string textPrimeraIteraccio; //text que apareixera a la pantalla quan s'interactue amb l'objecte
    [TextArea] public string textSegonaIteraccio; //text que apareixera a la pantalla quan s'interactue amb l'objecte
    bool interactuat = false;
    public bool pocio;
    public int numPocions;

    public void Interactuar()
    {
        if(!interactuat)
        {
            ObjecteDialegManager.instance.mostrarDialeg(textPrimeraIteraccio); //mostra el text a la pantalla
            interactuat = true;
            if(pocio)
            {
                VariablesGlobals.pocions += numPocions; //aumenta el nombre de pocions que te el jugador
            }
        }
        else
        {
            ObjecteDialegManager.instance.mostrarDialeg(textSegonaIteraccio); //mostra el text a la pantalla
        }
    }
}
