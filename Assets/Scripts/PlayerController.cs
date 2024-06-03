using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header ("Player Stats")]
    [SerializeField] private float  _moveSpeed;
    [SerializeField] private int    _health;
    [SerializeField] private int    _maxHealth;
    [SerializeField] private int    _maximumMaxHealth;
    [SerializeField] private int    _damage;
    [SerializeField] private int    _maxDamage;
    [SerializeField] private int    _parts;

    [Header ("Movement Settings")]
    [SerializeField] private float  _dodgeSpeedMultiplier;
    [SerializeField] private float  _dodgeDuration;
    [SerializeField] private float  _rotationSpeed;
    [SerializeField] private int    _dodgeDamage;
    [SerializeField] private int    _minDodgeDamage;

    [Header("Laser Settings")]
    [SerializeField] private bool   _canFireLaser;
    [SerializeField] private float  _laserSpeed;
    [SerializeField] private float  _laserCooldown;

    [Header ("Everything Else")]
    [SerializeField] private Animator           _animator;
    [SerializeField] private Camera             _mainCamera;
    [SerializeField] private GameObject         _laserPrefab;
    [SerializeField] private HealthBar          _healthBar;
    [SerializeField] private Rigidbody          _rigidbody;

    private bool                    _isDead = false;
    private bool                    _isDodging;
    private bool                    _isMoving;
    private bool                    _isMovingBackwards;
    private GameObject              _healthButton;
    private Vector2                 _moveInput;
    private Vector2                 _rightStick;
    private Vector2                 _mousePosition;
    private Vector2                 _warpPosition;
    private Vector2                 _overflow;
    private Vector3                 _moveDirection;
    private GameManager             _gameManager;

    public bool isAlive = true;
    public Vector2 sensitivity = new Vector2(1500f, 1500f);
    public Vector2 bias = new Vector2 (0f, -1f);

    PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Dodge.performed += ctx => StartCoroutine(StartDodge());
        controls.Gameplay.Fire.performed += ctx => FireLaser();

    }
    void Start()
    {
        _isDead = false;
        _health = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        _healthButton = GameObject.Find("Health_Button");
    }

    void Update()
    {
        CalculateMovement();
        CalculateJoystickMovement();
        
        // Check for dodge input
        if (Input.GetKeyDown(KeyCode.Space) && !_isDodging)
        {
            StartCoroutine(StartDodge());
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

    private void CalculateJoystickMovement()
    {
        _rightStick = Gamepad.current.rightStick.ReadValue();

        if (_rightStick.magnitude < 0.1f) return;

        _mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        _warpPosition = _mousePosition + bias + _overflow + sensitivity * Time.deltaTime * _rightStick;

        _warpPosition = new Vector2(Mathf.Clamp(_warpPosition.x, 0, Screen.width), Mathf.Clamp(_warpPosition.y, 0, Screen.height));

        _overflow = new Vector2(_warpPosition.x % 1, _warpPosition.y % 1);

        Mouse.current.WarpCursorPosition(_warpPosition);
    }

    private void RotateTowardsCursor(Vector3 targetPoint)
    {
        if (!_isDead)
        {
            // Calculate the direction to the target point
            Vector3 direction = targetPoint - transform.position;
        
            // Calculate the rotation to face the target point
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate the player towards the target point
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator StartDodge()
    {
        _isDodging = true;
        float currentSpeed = _moveSpeed;
        _moveSpeed *= _dodgeSpeedMultiplier;
        _health -= _dodgeDamage;
        _healthBar.SetHealth(_health);

        // Wait for the duration of the dodge
        yield return new WaitForSeconds(_dodgeDuration);

        // Reset dodge state and speed
        _isDodging = false;
        _moveSpeed = currentSpeed;
    }

    private void FireLaser()
    {
        Vector3 laserOffset = transform.forward * 1.05f;
        float laserHeightOffset = 1.0f;
        GameObject laser = Instantiate(_laserPrefab, transform.position + laserOffset + new Vector3(0f, laserHeightOffset, 0f), Quaternion.identity);
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
        _isDead = true;
        _animator.SetBool("isDead", _isDead);

        ReturnState();
    }

    public bool ReturnState()
    {
        if (_isDead)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void IncreaseHealth()
    {
        if (_maxHealth < _maximumMaxHealth)
        {
            _maxHealth += 25;
            if (_maxHealth == _maximumMaxHealth)
            {
                TextMeshProUGUI canvasButtonText = _healthButton.GetComponentInChildren<TextMeshProUGUI>();
                canvasButtonText.text = "Max";
            }
        }
    }

    public void IncreaseDamage()
    {
        if (_damage < _maxDamage)
        {
            _damage += 2;
            if ( _damage == _maxDamage)
            {
                //TextMeshProUGUI canvasButtonText = _canvasButton.GetComponentInChildren<TextMeshProUGUI>();
                //canvasButtonText.text = "Max";
            }
        }
    }

    public void DecreaseDodgeDamage()
    {
        if (_dodgeDamage > _minDodgeDamage)
        {
            _dodgeDamage /= 2;
            if (_dodgeDamage == _minDodgeDamage)
            {
                //TextMeshProUGUI canvasButtonText = _canvasButton.GetComponentInChildren<TextMeshProUGUI>();
                //canvasButtonText.text = "Max";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet" && !_isDodging)
        {
            DoDamage();
        }

        if (other.tag == "Enemy" && !_isDodging)
        {
            DoDamage();
        }

        if (other.name == "Level_Trigger")
        {
            _gameManager.LevelController();
        }
    }

    private void DoDamage()
    {
        _health--;
        _healthBar.SetHealth(_health);
        if (_health < 0)
        {
            _health = 0;
        }
        if (_health == 0)
        {
            StartCoroutine(PlayerDeath());
        }
    }

    private IEnumerator PlayerDeath()
    {
        PlayerDied();
        yield return new WaitForSeconds(5);
        //Spawn into Main room
    }

    public bool CheckDodge()
    {
        return _isDodging;
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
