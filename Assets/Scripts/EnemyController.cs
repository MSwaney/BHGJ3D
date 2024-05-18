using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header ("Runner Settings")]
    [SerializeField] private bool _isRunner;
    [SerializeField] private float _chaseRadius = 10f;
    [SerializeField] private float _stoppingDistance = 2f;

    [Header ("Other Settings")]
    [SerializeField] private Animator _animator;

    private bool _isChasing;

    private BulletSpawner _bulletSpawner;
    private NavMeshAgent _navMeshAgent;
    private Transform _player;
    // Start is called before the first frame update
    void Start()
    {
        // Get the BulletSpawner component attached to the enemy
        _bulletSpawner = GetComponentInChildren<BulletSpawner>();
        // Get the NavMeshAgent component attached to the enemy
        _navMeshAgent = GetComponent<NavMeshAgent>();
        // Find the player's transform (assuming the player has a tag "Player")
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        if (_isRunner)
        {
            _animator.SetBool("isRunner", _isRunner);
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

    // Draw the chase radius in the scene view for debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _chaseRadius);
    }

    public IEnumerator PlayFireAnim(float cooldown)
    {
        _animator.SetBool("isFiring", true);
        yield return new WaitForSeconds(cooldown);
        _animator.SetBool("isFiring", false);
    }
}
