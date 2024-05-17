using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Player Stats")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage;

    [Header ("Movement Settings")]
    [SerializeField] private float _dodgeSpeedMultiplier;
    [SerializeField] private float _dodgeDuration;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _dodgeDamage;

    [Header("Laser Settings")]
    [SerializeField] private bool _canFireLaser;
    [SerializeField] private float _laserSpeed;
    [SerializeField] private float _laserCooldown;

    [Header ("Everything Else")]
    [SerializeField] private Animator _animator;
    [SerializeField] private BulletSpawner[] _bulletSpawner;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private Rigidbody _rigidbody;

    private bool _isDodging;
    private bool _isMoving;
    private bool _isMovingBackwards;
    private Vector2 _moveInput;
    private Vector3 _moveDirection;
    private GameManager _gameManager;

    public bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        _health = _maxHealth;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _bulletSpawner = GameObject.Find("BulletSpawner").GetComponents<BulletSpawner>();
        print(_gameManager);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        // Check for dodge input
        if (Input.GetKeyDown(KeyCode.Space) && !_isDodging)
        {
            StartCoroutine(StartDodge());
        }

        if (_health <= 0)
        {
            PlayerDied();
        }
    }

    // FixedUpdate is called at a fixed interval
    private void FixedUpdate()
    {
        // Check for laser firing input
        if (_canFireLaser && Input.GetMouseButton(0))
        {
            FireLaser();
        }
    }

    private void CalculateMovement()
    {
        // Get movement input from the player
        _moveInput.x = Input.GetAxis("Horizontal");
        _moveInput.y = Input.GetAxis("Vertical");
        _moveInput.Normalize();

        // Apply movement to the player's Rigidbody
        _rigidbody.velocity = new Vector3(_moveInput.x * _moveSpeed, _rigidbody.velocity.y, _moveInput.y * _moveSpeed);

        // Determine if the player is moving
        if (_moveInput.x != 0 || _moveInput.y != 0)
        {
            _isMoving = true;
            _animator.SetBool("isMoving", _isMoving);
        }
        else
        {
            _isMoving = false;
            _animator.SetBool("isMoving", _isMoving);
        }

        // Calculate movement direction based on input
        Vector3 inputDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);

        // Calculate dot product with character's forward direction
        float forwardDirection = Vector3.Dot(transform.forward, inputDirection.normalized);

        // Set Animator parameters based on dot product
        if (forwardDirection > 0f)
        {
            _isMoving = true;
            _isMovingBackwards = false;
        }
        else if (forwardDirection < 0f)
        {
            _isMoving = false;
            _isMovingBackwards = true;
        }
        else
        {
            _isMoving = false;
            _isMovingBackwards = false;
        }

        _animator.SetBool("isMoving", _isMoving);
        _animator.SetBool("isMovingBackwards", _isMovingBackwards);

        // Cast a ray from the mouse position to determine player rotation
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        // Check if the ray intersects with the ground plane
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            // Get the point of intersection
            Vector3 targetPoint = ray.GetPoint(rayDistance);

            // Ensure the target point is at the same height as the player
            targetPoint.y = transform.position.y;

            // Rotate the player towards the target point
            RotateTowardsCursor(targetPoint);
        }
    }

    private void RotateTowardsCursor(Vector3 targetPoint)
    {
        // Calculate the direction to the target point
        Vector3 direction = targetPoint - transform.position;
        
        // Calculate the rotation to face the target point
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate the player towards the target point
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private IEnumerator StartDodge()
    {
        _isDodging = true;
        float currentSpeed = _moveSpeed;
        _moveSpeed *= _dodgeSpeedMultiplier;
        _health -= _dodgeDamage;

        // Wait for the duration of the dodge
        yield return new WaitForSeconds(_dodgeDuration);

        // Reset dodge state and speed
        _isDodging = false;
        _moveSpeed = currentSpeed;
    }

    private void FireLaser()
    {
        Vector3 laserOffset = transform.forward * 1.05f;
        GameObject laser = Instantiate(_laserPrefab, transform.position + laserOffset, Quaternion.identity);
        laser.GetComponent<Rigidbody>().velocity = transform.forward * _laserSpeed;

        // Start the laser cooldown coroutine
        StartCoroutine(LaserCooldown());
    }

    private IEnumerator LaserCooldown()
    {
        _canFireLaser = false;
        yield return new WaitForSeconds(_laserCooldown);
        _canFireLaser = true;
    }

    public void PlayerDied()
    {
        _gameManager.SetPlayerDead();
        foreach (BulletSpawner spawner in _bulletSpawner)
        {
            spawner.SetGameOver();
        }
    }
}
