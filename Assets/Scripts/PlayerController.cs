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

        if (_bulletSpawner == null )
        {
            Debug.Log("Bullet Spawner is NULL.");
        }
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

        // Calculate movement vector
        Vector3 movement = new Vector3(_moveInput.x, 0f, _moveInput.y) * _moveSpeed;

        // Apply movement using AddForce with ForceMode.VelocityChange
        _rigidbody.AddForce(movement * Time.deltaTime, ForceMode.VelocityChange);

        // Ensure the Y component of velocity is zero to prevent floating
        Vector3 currentVelocity = _rigidbody.velocity;
        currentVelocity.y = 0;
        _rigidbody.velocity = currentVelocity;

        // Debug the movement vector

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

    public void IncreaseHealth()
    {
        if (_maxHealth < 200)
        {
            _maxHealth += 25;
        }
    }

    public void IncreaseDamage()
    {
        if (_damage < 10)
        {
            _damage += 2;
        }
    }

    public void DecreaseDodgeDamage()
    {
        if (_dodgeDamage > 2)
        {
            _dodgeDamage /= 2;
        }
    }
}
