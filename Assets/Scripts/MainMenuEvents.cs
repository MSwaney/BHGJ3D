using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;
    private Button _startButton;
    private Button _controlsButton;
    private Button _creditsButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _startButton = _document.rootVisualElement.Q("StartGameButton") as Button;
        _startButton.RegisterCallback<ClickEvent>(OnPlayGameClick);

        _controlsButton = _document.rootVisualElement.Q("ControlsButton") as Button;
        _controlsButton.RegisterCallback<ClickEvent>(OnControlsClick);

        _creditsButton = _document.rootVisualElement.Q("CreditsButton") as Button;
        _creditsButton.RegisterCallback<ClickEvent>(OnCreditsClick);

    }

    private void OnDisable()
    {
        _startButton.UnregisterCallback<ClickEvent>(OnPlayGameClick);
        _controlsButton.UnregisterCallback<ClickEvent>(OnControlsClick);
        _creditsButton.UnregisterCallback<ClickEvent>(OnCreditsClick);

    }
    private void OnPlayGameClick(ClickEvent evt)
    {
        Debug.Log("You Pressed the Start Button!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnControlsClick(ClickEvent evt)
    {
        Debug.Log("Learn the Controls!");
        SceneManager.LoadScene("Controls");
    }

    private void OnCreditsClick(ClickEvent evt)
    {
        Debug.Log("Check out our awesome team!");
        SceneManager.LoadScene("Credits");
    }
}
