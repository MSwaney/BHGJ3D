using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    private PlayerController _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Leila").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpgradeHealth()
    {
        _player.IncreaseHealth();
    }
    public void UpgradeDamage()
    {
        _player.IncreaseDamage();
    }

    public void UpgradeDodgeDamage()
    {
        _player.DecreaseDodgeDamage();
    }

}
