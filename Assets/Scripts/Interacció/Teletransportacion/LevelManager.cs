using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Gestiona les transicions: Fade Out -> Teleport -> Espera 1s -> Fade In.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Configuració de Transició")]
    [SerializeField] private GameObject panelFadePanel;
    [SerializeField] private float duradaFadeOut = 0.1f;
    [SerializeField] private float duradaEsperaNegre = 0.1f; // El segon d'espera que demanes
    [SerializeField] private float duradaFadeIn = 0.1f;

    private string idPuntEntradaObjectiu;
    private bool estaTransicionant = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InicialitzarPanel();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InicialitzarPanel()
    {
        if (panelFadePanel == null) return;
        DontDestroyOnLoad(panelFadePanel);
        panelFadePanel.SetActive(false);
        EstablirAlphaPanel(0f);
    }

    public void CarregarEscena(string nomEscena, string idPunt)
    {
        if (estaTransicionant) return;
        idPuntEntradaObjectiu = idPunt;
        StartCoroutine(RutinaTransicio(nomEscena));
    }

    private IEnumerator RutinaTransicio(string nomEscena)
    {
        estaTransicionant = true;

        // 1. FADE OUT (Pantalla es torna negra)
        BlockejarInputJugador(true);
        if (panelFadePanel != null) panelFadePanel.SetActive(true);
        yield return StartCoroutine(FerFade(0f, 1f, duradaFadeOut));

        // 2. CÀRREGA D'ESCENA
        AsyncOperation operacioCarrega = SceneManager.LoadSceneAsync(nomEscena);
        while (!operacioCarrega.isDone)
        {
            yield return null;
        }

        // 3. TELETRANSPORT (Encara en negre)
        // Esperem un parell de frames per assegurar que els objectes de la nova escena s'han despertat
        yield return new WaitForEndOfFrame(); 
        TeletransportarJugador();
        
        // 4. ESPERA DE 1 SEGON (Amb la pantalla en negre)
        // Això és el que has demanat específicament.
        yield return new WaitForSeconds(duradaEsperaNegre);

        // 5. FADE IN (La pantalla torna a ser visible)
        yield return StartCoroutine(FerFade(1f, 0f, duradaFadeIn));

        if (panelFadePanel != null) panelFadePanel.SetActive(false);
        BlockejarInputJugador(false);
        estaTransicionant = false;
    }

    private void TeletransportarJugador()
    {
        EntryPoint puntObjectiu = TrobarPuntEntrada(idPuntEntradaObjectiu);
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (puntObjectiu != null && jugador != null)
        {
            CharacterController cc = jugador.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            jugador.transform.SetPositionAndRotation(
                puntObjectiu.transform.position,
                puntObjectiu.transform.rotation
            );

            if (cc != null) cc.enabled = true;
            
            // Forçar que la càmera estigui ja a lloc abans de treure el negre
            ForçarSnapCàmera(jugador.transform);
        }
    }

    private EntryPoint TrobarPuntEntrada(string id)
    {
        EntryPoint[] punts = Object.FindObjectsByType<EntryPoint>(FindObjectsSortMode.None);
        foreach (EntryPoint p in punts)
        {
            if (p.ID == id) return p;
        }
        return null;
    }

    private void ForçarSnapCàmera(Transform jugador)
    {
        // Enviem missatge per si la càmera té un script de seguiment manual
        Camera.main?.SendMessage("SnapToTarget", SendMessageOptions.DontRequireReceiver);
    }

    private void BlockejarInputJugador(bool bloquejar)
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador == null) return;
        
        // Exemple genèric: desactivar el script de moviment (ajusta el nom al teu)
        // var script = jugador.GetComponent<PlayerMovement>();
        // if(script != null) script.enabled = !bloquejar;
    }

    private IEnumerator FerFade(float alphaInici, float alphaFinal, float durada)
    {
        if (panelFadePanel == null) yield break;
        Image img = panelFadePanel.GetComponent<Image>();
        float temps = 0f;

        while (temps < durada)
        {
            temps += Time.deltaTime;
            float t = Mathf.Clamp01(temps / durada);
            EstablirAlphaPanel(Mathf.Lerp(alphaInici, alphaFinal, t));
            yield return null;
        }
        EstablirAlphaPanel(alphaFinal);
    }

    private void EstablirAlphaPanel(float alpha)
    {
        Image img = panelFadePanel.GetComponent<Image>();
        if (img != null)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }
}