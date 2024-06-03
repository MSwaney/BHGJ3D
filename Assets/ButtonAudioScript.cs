using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ButtonAudioScript : MonoBehaviour
{
    private UIDocument _document;
    private List<Button> _menuButtons = new List<Button>();
    private AudioSource _buttonAudio;
    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _buttonAudio = GetComponent<AudioSource>();

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
        _buttonAudio.PlayOneShot(_buttonAudio.clip, 1f);
        // _buttonAudio.Play();
    }
}
