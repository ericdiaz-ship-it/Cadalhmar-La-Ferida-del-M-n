using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CodigoInici : MonoBehaviour
{
    void OnEnable()
    {
        var DocumentUI = GetComponent<UIDocument>();
        VisualElement root = DocumentUI.rootVisualElement;
        Button buttonNewGame = root.Q<Button>("NewGame");
        buttonNewGame.clicked += () =>
        {
            SceneManager.LoadScene("CasaEldrin");
        };
    }
}
