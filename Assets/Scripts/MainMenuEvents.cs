using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    [SerializeField] private UIDocument _document;
    private Button _startButton;
    private Button _controlsButton;
    private Button _creditsButton;
    private Button _backButton;

    private List<Button> _menuButtons = new List<Button>();
    private AudioSource _buttonAudio;
    private AudioSource _menuMusic;


    private void Awake()
    {
        //_document = GetComponent<UIDocument>();

        _menuMusic = GetComponent<AudioSource>();

        _buttonAudio = GetComponent<AudioSource>();

        if(_menuMusic.clip != null)
        {
            _menuMusic.Play();
        }

        print(_document);
        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }

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
        // for (int i = 0; i < _menuButtons.Count; i++)
        // {
        //     _menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        // }

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

    private void OnAllButtonsClick(ClickEvent evt)
    {
        _buttonAudio.PlayOneShot(_buttonAudio.clip, 0.5f);
    }
}
