using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Configuració de Transició")]
    [SerializeField] private GameObject panelFadePanel;
    [SerializeField] private float duradaFade = 1.5f;
    
    private string idPuntEntradaObjectiu;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (panelFadePanel != null) {
                DontDestroyOnLoad(panelFadePanel);
                panelFadePanel.SetActive(false);
                Image panelImage = panelFadePanel.GetComponent<Image>();
                if (panelImage != null) {
                    Color inicial = panelImage.color;
                    inicial.a = 0f;
                    panelImage.color = inicial;
                }
            }
        } else {
            Destroy(gameObject);
        }
    }

    public void CarregarEscena(string nomEscena, string idPunt)
    {
        idPuntEntradaObjectiu = idPunt;
        StartCoroutine(RutinaTransicio(nomEscena));
    }

    private IEnumerator RutinaTransicio(string nomEscena)
    {
        if (panelFadePanel != null) {
            panelFadePanel.SetActive(true);
            yield return null;
        }

        // 1. Fade Out → negro total
        yield return StartCoroutine(FerFade(1f));

        // 2. Cargar escena (pantalla completamente negra)
        AsyncOperation operacioCarrega = SceneManager.LoadSceneAsync(nomEscena);
        while (!operacioCarrega.isDone) yield return null;

        // 3. Teletransportar jugador (el usuario no ve nada)
        MoureJugadorAlPunt();

        // 4. Fade In → aparece la escena con el jugador ya en su sitio
        yield return StartCoroutine(FerFade(0f));

        if (panelFadePanel != null) panelFadePanel.SetActive(false);
    }

    private void MoureJugadorAlPunt()
    {
        EntryPoint[] punts = Object.FindObjectsByType<EntryPoint>(FindObjectsSortMode.None);
        
        foreach (EntryPoint p in punts)
        {
            if (p.ID == idPuntEntradaObjectiu)
            {
                GameObject jugador = GameObject.FindGameObjectWithTag("Player");
                if (jugador != null) {
                    jugador.transform.position = p.transform.position;
                    jugador.transform.rotation = p.transform.rotation;
                }
                break;
            }
        }
    }

    private IEnumerator FerFade(float alphaObjectiu)
    {
        if (panelFadePanel == null) yield break;

        Image panelImage = panelFadePanel.GetComponent<Image>();
        if (panelImage == null) yield break;

        float alphaInicial = panelImage.color.a;
        float temps = 0;

        while (temps < duradaFade)
        {
            Color color = panelImage.color;
            color.a = Mathf.Lerp(alphaInicial, alphaObjectiu, temps / duradaFade);
            panelImage.color = color;
            temps += Time.deltaTime;
            yield return null;
        }

        Color finalColor = panelImage.color;
        finalColor.a = alphaObjectiu;
        panelImage.color = finalColor;
    }
}