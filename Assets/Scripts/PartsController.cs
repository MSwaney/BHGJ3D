using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartsController : MonoBehaviour
{
    private PlayerController _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision other)
    {
        print("Made it!");

        if (other.transform.tag == "Player")
        {
            _player.AddParts();
            Destroy(this.gameObject);
        }
    }
}
