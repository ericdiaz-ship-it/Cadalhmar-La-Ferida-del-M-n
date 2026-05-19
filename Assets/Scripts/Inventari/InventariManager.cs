// InventariManager.cs
using UnityEngine;
using System.Collections.Generic;
using TMPro;

// Gestor de l'inventari: mostra els documents recollits i el seu detall
public class InventariManager : MonoBehaviour {
    public static InventariManager instance; // singleton per accedir fàcilment des d'altres scripts
    [Header("Referències UI")]
    public Transform contentLlista;         // el Content del Scroll View esquerre
    public GameObject prefabBotoDocument;   // el prefab del botó

    [Header("Panell Detall")]
    public TMP_Text textTitol;
    public TMP_Text textContingut;

    [Header("Base de dades")]
    public DocumentsDatabase baseDeDades;
    public GameObject panellInventari; // referència al panell d'inventari (activar/desactivar)

    // Utilitza la llista global definida a VariablesGlobals

    void OnEnable() {
        // S'executa cada cop que s'obre l'inventari
        RefrescarLlista();
    }

    void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);
        // Asegurar que la base de datos tenga su lookup inicializado
        if (baseDeDades != null) baseDeDades.Init();
    }

    // Obre el panell d'inventari i refresca la llista. Opcionalment mostra un document concret
    private string idPerMostrar = null;
    public void ObreInventari(string idAMostrar = null) {
        if (panellInventari != null) panellInventari.SetActive(true);
        idPerMostrar = idAMostrar;
        RefrescarLlista();
    }

    void RefrescarLlista() {
        // Comprovacions bàsiques per evitar NullReferenceException
        if (contentLlista == null) {
            Debug.LogError("InventariManager.RefrescarLlista: 'contentLlista' no està assignat a l'inspector.");
            return;
        }

        // Netejar els botons anteriors
        foreach (Transform fill in contentLlista)
            Destroy(fill.gameObject);

        // Crear un botó per cada ID de document recollit (de les variables globals)
        if (VariablesGlobals.documentsRecollits != null) {
            if (baseDeDades == null) {
                Debug.LogWarning("InventariManager.RefrescarLlista: 'baseDeDades' no assignada, no es poden mostrar documents.");
            } else if (prefabBotoDocument == null) {
                Debug.LogWarning("InventariManager.RefrescarLlista: 'prefabBotoDocument' no està assignat.");
            } else {
                foreach (string id in VariablesGlobals.documentsRecollits) {
                    DocumentData doc = baseDeDades.GetDocument(id);
                    if (doc == null) continue;

                    GameObject btn = Instantiate(prefabBotoDocument, contentLlista);
                    if (btn == null) continue;

                    // Actualizar el text del botó directament
                    TextMeshProUGUI textComponent = btn.GetComponentInChildren<TextMeshProUGUI>();
                    if (textComponent != null) {
                        textComponent.text = doc.titol;
                        Debug.Log($"InventariManager: Text del botó actualitzat a '{doc.titol}'");
                    } else {
                        Debug.LogWarning($"InventariManager: No s'ha trobat TextMeshProUGUI al botó instanciat per '{doc.titol}'");
                    }

                    // Setup del DocumentButton script si l'hi ha
                    DocumentButton docBtn = btn.GetComponent<DocumentButton>();
                    if (docBtn != null) docBtn.Setup(doc.id, doc.titol);

                    // Capturar una copia local per evitar problemes de closure
                    var localDoc = doc;
                    var uiButton = btn.GetComponent<UnityEngine.UI.Button>();
                    if (uiButton != null) uiButton.onClick.AddListener(() => MostrarDocument(localDoc));
                }
            }
        }

        // Si s'ha demanat mostrar un id concret, mostrar-lo ara
        if (!string.IsNullOrEmpty(idPerMostrar)) {
            DocumentData docAMostrar = baseDeDades.GetDocument(idPerMostrar);
            if (docAMostrar != null) MostrarDocument(docAMostrar);
            idPerMostrar = null;
        }

        // Netejar el panell de detall en refrescar
        if (textTitol != null) textTitol.text = string.Empty;
        if (textContingut != null) textContingut.text = string.Empty;
    }

    void MostrarDocument(DocumentData doc) {
        if (doc == null) {
            textTitol.text = string.Empty;
            textContingut.text = string.Empty;
            return;
        }

        textTitol.text = doc.titol;
        textContingut.text = doc.contingut;
    }

    void Update() {
        // Al prémer la tecla I s'obre o es tanca el panell d'inventari
        if (Input.GetKeyDown(KeyCode.I)) {
            if (panellInventari != null && panellInventari.activeSelf) {
                panellInventari.SetActive(false);
            } else {
                ObreInventari();
            }
        }
    }
}