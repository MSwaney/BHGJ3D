using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenuEvents : MonoBehaviour
{
    [SerializeField] private UIDocument _document;
    private Button _startButton;
    private Button _controlsButton;
    private Button _creditsButton;
    private Button _backButton;
    private Button _quitButton;
    private Button _cancelButton;
    private Button _exitGameButton;

    private VisualElement _confirmQuit;

    public AudioSource _menuMusic;

    private void Awake()
    {
        _confirmQuit = _document.rootVisualElement.Q("ConfirmQuit") as VisualElement;

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

        _quitButton = _document.rootVisualElement.Q("QuitButton") as Button;
        if ( _quitButton != null ) {
            _quitButton.RegisterCallback<ClickEvent>(QuitGame);
        }

        _cancelButton = _document.rootVisualElement.Q("Cancel") as Button;
        if ( _cancelButton != null ) {
            _cancelButton.RegisterCallback<ClickEvent>(ReturnToMenu);
        }

        _exitGameButton = _document.rootVisualElement.Q("Exit") as Button;
        if ( _exitGameButton != null ) {
            _exitGameButton.RegisterCallback<ClickEvent>(ExitGame);
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

        if ( _quitButton != null ) {
            _quitButton.UnregisterCallback<ClickEvent>(QuitGame);
        }
    }
    private void OnPlayGameClick(ClickEvent evt)
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void OnControlsClick(ClickEvent evt)
    {
        SceneManager.LoadScene("Controls");
    }

    private void OnCreditsClick(ClickEvent evt)
    {
        SceneManager.LoadScene("Credits");
    }

    private void OnBackClick(ClickEvent evt)
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void QuitGame(ClickEvent evt) 
    {
        _confirmQuit.style.display = DisplayStyle.Flex;

    }
    private void ReturnToMenu(ClickEvent evt) 
    {
        _confirmQuit.style.display = DisplayStyle.None;

    }
    private void ExitGame(ClickEvent evt) 
    {
        if (Application.isEditor)
        {

            EditorApplication.isPlaying = false;
        } else {

            Application.Quit();

        }


    }
}
