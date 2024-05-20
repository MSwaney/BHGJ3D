using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _bulletResource;
    [SerializeField] private Vector3 _bulletVelocity;
    [SerializeField] private float _minRotation;
    [SerializeField] private float _maxRotation;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _numberOfBullets;
    [SerializeField] private bool _isRandom;
    [SerializeField] private bool _isRotating = false;

    private bool _isGameOver = false;
    private float _timer;
    private float[] _rotations;

    private EnemyController _enemyController;
    private PlayerController _player;

    public bool isFiring;

    // Start is called before the first frame update
    void Start()
    {
        _enemyController = this.transform.parent.GetComponent<EnemyController>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        _timer = _cooldown;
        _rotations = new float[_numberOfBullets];
        if (!_isRandom)
        {
            DistributedRotations();
        }

        if (_isRotating)
        {
            StartCoroutine(RotateSpawner());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGameOver)
        {
            if (_timer <= 0)
            {
                SpawnBullets();
                _timer = _cooldown;
            }
            _timer -= Time.deltaTime;

            if (_isRotating) // Example condition for when rotation changes
            {
                DistributedRotations();
            }
        }
        CheckPlayerState();
    }

    public float[] RandomRotations()
    {
        for (int i = 0; i < _numberOfBullets; i++)
        {
            _rotations[i] = Random.Range(_minRotation, _maxRotation);
        }
        return _rotations;
    }

    private float[] DistributedRotations()
    {
        _rotations = new float[_numberOfBullets];

        if (_numberOfBullets == 1)
        {
            _rotations[0] = _minRotation;
            return _rotations;
        }

        // Calculate the angle increment between each bullet
        float angleIncrement = (_maxRotation - _minRotation) / (_numberOfBullets - 1);

        // Set the rotation angle for each bullet
        for (int i = 0; i < _numberOfBullets; i++)
        {
            var fraction = (float)i / ((float)_numberOfBullets - 1);
            var difference = _maxRotation - _minRotation;
            var fractionOfDifference = fraction * difference;
            _rotations[i] = fractionOfDifference + _minRotation; // Add MinRotation to undo Difference
        }

        return _rotations;
    }

    private void CheckPlayerState()
    {
        if (_player.ReturnState() == true)
        {
            SetGameOver();
        }
    }

    public GameObject[] SpawnBullets()
    {
        if (_isRandom)
        {
            RandomRotations();
        }

        if (!_isRotating)
        {
            StartCoroutine(_enemyController.PlayFireAnim(_cooldown));
        }

        // Spawn Bullets
        GameObject[] spawnedBullets = new GameObject[_numberOfBullets];
        for (int i = 0; i < _numberOfBullets; i++)
        {
            spawnedBullets[i] = Instantiate(_bulletResource, transform.position, transform.rotation);

            var b = spawnedBullets[i].GetComponent<Bullet>();
            b._speed = _bulletSpeed;

            // Calculate direction vector based on bullet's rotation
            float angle = _rotations[i];
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            
            // Check if direction vector is valid
            if (!float.IsNaN(direction.x) && !float.IsNaN(direction.z))
            {
                b._velocity = direction * _bulletSpeed;
            }
            else
            {
                Destroy(spawnedBullets[i]);
                continue; // Skip to the next iteration of the loop
            }
        }

        isFiring = false;

        return spawnedBullets;
    }

    private IEnumerator RotateSpawner()
    {
        while (_isRotating)
        {
            transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);

            yield return null;
        }
    }

    public void SetGameOver()
    {
        _isGameOver = true;
    }
}
