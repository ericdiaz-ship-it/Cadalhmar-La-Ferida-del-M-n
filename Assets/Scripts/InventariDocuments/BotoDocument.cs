// DocumentButton.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DocumentButton : MonoBehaviour {
    public string documentId;
    public string titolDocument;
    private TextMeshProUGUI labelText;  // Utilitzem TextMeshProUGUI explícitament

    void Start() {
        // Busquem el TMP_Text a Start per si de cas
        if (labelText == null) {
            labelText = GetComponentInChildren<TextMeshProUGUI>();
            if (labelText != null) {
                Debug.Log("DocumentButton.Start: Trobat TMP_Text a " + labelText.gameObject.name);
            }
        }
    }

    public void Setup(string id, string titulo) {
        documentId = id;
        titolDocument = titulo;
        
        // Busquem el TextMeshProUGUI
        if (labelText == null) {
            labelText = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        // Actualitzem el text de forma forçada
        if (labelText != null) {
            labelText.text = titulo;
            labelText.SetAllDirty();  // Forcem l'actualització
            Debug.Log($"DocumentButton.Setup: Text actualitzat a '{titulo}' al GameObject {gameObject.name}");
        } else {
            // Si no el trobem amb GetComponentInChildren, intentem trobar-lo directament
            Transform textTransform = transform.Find("Text (TMP)");
            if (textTransform != null) {
                labelText = textTransform.GetComponent<TextMeshProUGUI>();
                if (labelText != null) {
                    labelText.text = titulo;
                    labelText.SetAllDirty();
                    Debug.Log($"DocumentButton.Setup: Text trobat per Find() i actualitzat a '{titulo}'");
                    return;
                }
            }
            Debug.LogError($"DocumentButton.Setup: No s'ha pogut trobar TextMeshProUGUI al prefab. Jerarquia del botó: {GetHierarchy(transform)}");
        }
    }

    // Funció auxiliar per veure la jerarquia
    private string GetHierarchy(Transform t, int depth = 0) {
        string result = new string(' ', depth * 2) + t.name + "\n";
        foreach (Transform child in t) {
            result += GetHierarchy(child, depth + 1);
        }
        return result;
    }
}