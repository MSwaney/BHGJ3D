using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (_gameManager.EnemyCount() == 7)
        {
            Destroy(this.gameObject);
        }
    }
}
