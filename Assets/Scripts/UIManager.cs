using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] TextMeshProUGUI _partsText;
    private PlayerController _playerController;
    private int _parts;

    // Start is called before the first frame update
    void Start()
    {
        _partsText.text = "Parts: " + 0.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetMaxHealth(int health)
    {
        _slider.maxValue = health;
        _slider.value = health;
    }

    public void SetHealth(int health)
    {
        _slider.value = health;
    }

    public void UpdatePartsText(int parts)
    {
        _partsText.text = "Parts: " + parts.ToString();
    }
}
