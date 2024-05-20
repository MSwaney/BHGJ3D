using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header ("Runner Settings")]
    [SerializeField] private bool   _isRunner;
    [SerializeField] private float  _chaseRadius = 10f;
    [SerializeField] private float  _stoppingDistance = 2f;

    [Header ("Spinner Settings")]
    [SerializeField] private bool   _isSpinner;

    [Header ("Other Settings")]
    [SerializeField] private Animator   _animator;
    [SerializeField] private int        _health;

    private bool _isChasing;
    private bool _isDead;

    private BulletSpawner   _bulletSpawner;
    private GameManager     _gameManager;
    private NavMeshAgent    _navMeshAgent;
    private Transform       _player;

    // Start is called before the first frame update
    void Start()
    {
        // Get the BulletSpawner component attached to the enemy
        _bulletSpawner = GetComponentInChildren<BulletSpawner>();
        // Get the NavMeshAgent component attached to the enemy
        _navMeshAgent = GetComponent<NavMeshAgent>();
        // Find the player's transform (assuming the player has a tag "Player")
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_isRunner)
        {
            _animator.SetBool("isRunner", _isRunner);
        }
        if (_isSpinner)
        {
            _animator.SetBool("isSpinner", _isSpinner);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRunner)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

            if (distanceToPlayer <= _chaseRadius)
            {
                _isChasing = true;
            }
            else
            {
                _isChasing = false;
            }

            if (_isChasing)
            {
                _navMeshAgent.SetDestination(_player.position);
                if (distanceToPlayer <= _stoppingDistance)
                {
                    _navMeshAgent.isStopped = true;
                }
                else
                {
                    _navMeshAgent.isStopped = false;
                }
            }
        }
    }

    public IEnumerator PlayFireAnim(float cooldown)
    {
        _animator.SetBool("isFiring", true);
        yield return new WaitForSeconds(cooldown);
        _animator.SetBool("isFiring", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Laser")
        {
            _health--;

            if (_health == 0 )
            {
                //play death animation
                _isDead = true;
                _animator.SetBool("isDead", _isDead);
                //stop shooting or running
                if (!_isRunner)
                {
                    _bulletSpawner.SetGameOver();
                }
                StopRunner();
                StopCoroutine(PlayFireAnim(0f));
                StartCoroutine(DestroyEnemy());
            }
        }
    }

    private IEnumerator DestroyEnemy()
    {
        _gameManager.RemoveEnemy(this.gameObject);
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }

    public void StopRunner()
    {
        _isRunner = false;
    }
}
