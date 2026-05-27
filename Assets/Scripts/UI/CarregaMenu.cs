using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Adjunta aquest component a un botó UI (UnityEngine.UI) o a un botó de UI Toolkit.
/// Crida CarregaEscena des de l'esdeveniment OnClick per anar a l'escena del menú inicial.
/// </summary>
public class CarregaMenu : MonoBehaviour
{
    // Nom de l'escena del menú inicial. Canvia-ho si la teva escena té un altre nom.
    [SerializeField] private string nomEscenaMenu = "MenuInici";

    /// <summary>
    /// Carrega l'escena del menú.
    /// Assigna aquest mètode a l'esdeveniment OnClick del botó des de l'Inspector.
    /// </summary>
    public void CarregaEscena()
    {
        if (string.IsNullOrEmpty(nomEscenaMenu))
        {
            Debug.LogError("[CarregaMenu] No s'ha especificat cap nom d'escena.");
            return;
        }
        Debug.Log($"[CarregaMenu] Carregant l'escena '{nomEscenaMenu}'.");
        SceneManager.LoadScene(nomEscenaMenu);
    }
}
