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
    private Button _backButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _startButton = _document.rootVisualElement.Q("StartGameButton") as Button;
        if ( _startButton != null ) {
            _startButton.RegisterCallback<ClickEvent>(OnPlayGameClick);
        }

        _controlsButton = _document.rootVisualElement.Q("ControlsButton") as Button;
        if ( _controlsButton != null ) {
            _controlsButton.RegisterCallback<ClickEvent>(OnControlsClick);

        }

        _creditsButton = _document.rootVisualElement.Q("CreditsButton") as Button;
        if( _creditsButton != null ) {
            _creditsButton.RegisterCallback<ClickEvent>(OnCreditsClick);
        }

        _backButton = _document.rootVisualElement.Q("BackButton") as Button;
        if ( _backButton != null ) {
            _backButton.RegisterCallback<ClickEvent>(OnBackClick);
        }

    }

    private void OnDisable()
    {
        if ( _startButton != null ) {
            _startButton.UnregisterCallback<ClickEvent>(OnPlayGameClick);
        }

        if ( _controlsButton != null ) {
            _controlsButton.UnregisterCallback<ClickEvent>(OnControlsClick);

        }

        if( _creditsButton != null ) {
            _creditsButton.UnregisterCallback<ClickEvent>(OnCreditsClick);
        }

        if ( _backButton != null ) {
            _backButton.UnregisterCallback<ClickEvent>(OnBackClick);
        }
        // _startButton.UnregisterCallback<ClickEvent>(OnPlayGameClick);
        // _controlsButton.UnregisterCallback<ClickEvent>(OnControlsClick);
        // _creditsButton.UnregisterCallback<ClickEvent>(OnCreditsClick);
        // _backButton.UnregisterCallback<ClickEvent>(OnBackClick);

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

    private void OnBackClick(ClickEvent evt)
    {
        Debug.Log("GO BACK YO!");
        SceneManager.LoadScene("MainMenu");
    }
}
