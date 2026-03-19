using UnityEngine;
//aquest objecte s'utilitzara per a objectes que es poden llegir, com un llibre o un diari, i al interactuar amb ell apareixera un text a la pantalla amb el contingut del llibre o diari
public class ObjecteDebug : MonoBehaviour, IInteractuable
{
    [TextArea] public string text; //text que apareixera a la pantalla quan s'interactue amb l'objecte
    public void Interactuar()
    {
        Debug.Log(text); //mostra el text a la consola de Unity
    }
}
