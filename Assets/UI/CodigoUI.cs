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
        Button buttonContinue = root.Q<Button>("Continue");
        Button buttonExit = root.Q<Button>("Exit");

        VisualElement tutorialPanel = root.Q<VisualElement>("TutorialPanel");
        Button buttonCloseTutorial = root.Q<Button>("CloseTutorial");

        // Nou Joc
        buttonNewGame.clicked += () =>
        {
            SceneManager.LoadScene("CasaEldrin");
        };

        // Com Jugar? (Obrir tutorial)
        buttonContinue.clicked += () =>
        {
            tutorialPanel.style.display = DisplayStyle.Flex;
        };

        // Tancar Tutorial
        buttonCloseTutorial.clicked += () =>
        {
            tutorialPanel.style.display = DisplayStyle.None;
        };

        // Sortir
        buttonExit.clicked += () =>
        {
            Application.Quit();
        };
    }
}
