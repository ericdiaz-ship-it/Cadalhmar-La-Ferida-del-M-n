using TMPro;
using UnityEngine;

public class ObjecteDialegManager : MonoBehaviour
{
    //Definirem les variables que necessitarem per a gestionar els diàlegs dels objectes interactuables
    public static ObjecteDialegManager instance; //Singleton per a accedir a aquest manager des de qualsevol altre script
    public GameObject dialegPanel; //Panell que contindrà el text del diàleg
    public TextMeshProUGUI textDialeg; //TextMeshPro on es mostrarà el text del diàleg
    private bool dialegActiu = false; //Variable per a controlar si el diàleg està actiu o no
    //awake ès el primer mètode que s'executa quan es carrega l'script.
    void Awake() { instance = this; } //Assignem el singleton a aquest script
    // Update ès crida una vegada per frame.
    void Update()
    {
        //si el diàleg està actiu i el jugador prem la tecla espai, es tancara el diàleg
        if (dialegActiu && Input.GetKeyDown(KeyCode.Space))
        {
            tancarDialeg();
        }
    }
    public void mostrarDialeg(string text)
    {
        textDialeg.text = text; //Assignem el text del diàleg al TextMeshPro
        dialegPanel.SetActive(true); //Activem el panell del diàleg
        dialegActiu = true; //Marquem el diàleg com a actiu
        Time.timeScale = 0f; //Pausar el joc mentre el diàleg està actiu
    }
    public void tancarDialeg()
    {
        dialegPanel.SetActive(false); //Desactivem el panell del diàleg
        dialegActiu = false; //Marquem el diàleg com a inactiu
        Time.timeScale = 1f; //Reanudar el joc
    }
}
