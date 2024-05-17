using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool _isPlayerDead = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerDead && Input.GetMouseButton(0))
        {
            //Load Main room scene
        }
    }

    public void SetPlayerDead()
    {
        _isPlayerDead = true;
    }
}
