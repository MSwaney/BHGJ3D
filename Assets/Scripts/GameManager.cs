using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text _deadText;
    [SerializeField] private Text _restartText;

    private bool _isPlayerDead = false;
    private List<GameObject> _enemies = new List<GameObject>();
    private GameObject[] _doorController;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _isPlayerDead = false;
        _enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        _doorController = GameObject.FindGameObjectsWithTag("Door");
        _audioSource = GetComponent<AudioSource>();

        _audioSource.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerDead && Input.anyKey)
        {
            //Load Main room scene
            SceneManager.LoadScene("SampleScene");
        }  
    }

    public void SetPlayerDead()
    {
        _isPlayerDead = true;
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (_isPlayerDead)
        {
            _deadText.enabled = true;
            _restartText.enabled = true;
            yield return new WaitForSeconds(2);

            _deadText.enabled = false;
            _restartText.enabled = false;
            yield return new WaitForSeconds(.75f);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _enemies.Remove(enemy);
    }

    public int EnemyCount()
    {
        return _enemies.Count;
    }

    public void LevelController()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            SceneManager.LoadScene("Level1");
        }
        else
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
