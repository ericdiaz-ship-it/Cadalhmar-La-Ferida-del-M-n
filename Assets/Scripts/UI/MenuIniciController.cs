using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuIniciController : MonoBehaviour
{
    [Header("Configuració de l'Escena")]
    [Tooltip("Nom de l'escena on comença el joc (ex: Level1, Overworld, etc.)")]
    public string escenaNouJoc = "ElTeuNomDEscenaAqui";

    private UIDocument uiDocument;
    private VisualElement tutorialPanel;

    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null)
        {
            Debug.LogError("[MenuIniciController] Aquest script requereix un UIDocument adjunt al mateix GameObject.");
            return;
        }

        var root = uiDocument.rootVisualElement;

        // Cercar els botons per nom segons el teu menuInici.uxml
        Button btnNouJoc = root.Q<Button>("NewGame");
        Button btnTutorial = root.Q<Button>("Continue"); // El teu .uxml li diu "Continue" al botó de "Com jugar?"
        Button btnSortir = root.Q<Button>("Exit");
        Button btnTancarTutorial = root.Q<Button>("CloseTutorial");

        tutorialPanel = root.Q<VisualElement>("TutorialPanel");

        // Assignar les funcions a cada botó
        if (btnNouJoc != null) btnNouJoc.clicked += OnNouJocClicked;
        if (btnTutorial != null) btnTutorial.clicked += OnTutorialClicked;
        if (btnSortir != null) btnSortir.clicked += OnSortirClicked;
        if (btnTancarTutorial != null) btnTancarTutorial.clicked += OnTancarTutorialClicked;
    }

    private void OnDisable()
    {
        if (uiDocument == null) return;
        var root = uiDocument.rootVisualElement;

        Button btnNouJoc = root.Q<Button>("NewGame");
        Button btnTutorial = root.Q<Button>("Continue");
        Button btnSortir = root.Q<Button>("Exit");
        Button btnTancarTutorial = root.Q<Button>("CloseTutorial");

        // Desvincular events per evitar errors de memòria
        if (btnNouJoc != null) btnNouJoc.clicked -= OnNouJocClicked;
        if (btnTutorial != null) btnTutorial.clicked -= OnTutorialClicked;
        if (btnSortir != null) btnSortir.clicked -= OnSortirClicked;
        if (btnTancarTutorial != null) btnTancarTutorial.clicked -= OnTancarTutorialClicked;
    }

    private void OnNouJocClicked()
    {
        Debug.Log("[MenuInici] Iniciant Nou Joc... Esborrant PlayerPrefs.");
        
        // 1. Esborrar TOTS els PlayerPrefs (Això inclou les intro_vista de les cinemàtiques)
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // 2. També pots reiniciar VariablesGlobals directament aquí si cal
        VariablesGlobals.pocions = 0;
        VariablesGlobals.documentsRecollits.Clear();
        VariablesGlobals.CatherineCinematica = false;
        VariablesGlobals.paperRitual = false;
        VariablesGlobals.parlaSacerdot = false;
        VariablesGlobals.clauPorta = false;

        // 3. Carregar la nova escena
        if (!string.IsNullOrEmpty(escenaNouJoc))
        {
            SceneManager.LoadScene(escenaNouJoc);
        }
        else
        {
            Debug.LogWarning("[MenuInici] Falta posar el nom de l'escena on comença el joc a l'inspector!");
        }
    }

    private void OnTutorialClicked()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.style.display = DisplayStyle.Flex; // Mostrar tutorial
        }
    }

    private void OnTancarTutorialClicked()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.style.display = DisplayStyle.None; // Amagar tutorial
        }
    }

    private void OnSortirClicked()
    {
        Debug.Log("[MenuInici] Sortint del joc...");
        Application.Quit();
    }
}
