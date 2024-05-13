using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _moveSpeed;
    [SerializeField] BulletSpawner _bulletSpawner;
    [SerializeField] private float _dodgeSpeedMultiplier;
    [SerializeField] private float _dodgeDuration;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _rotationSpeed;

    private Vector2 _moveInput;
    private Vector3 _moveDirection;
    private bool _isDodging;
    private bool _isMoving;

    public bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && !_isDodging)
        {
            StartCoroutine(StartDodge());
        }
    }

    private void CalculateMovement()
    {
        _moveInput.x = Input.GetAxis("Horizontal");
        _moveInput.y = Input.GetAxis("Vertical");
        _moveInput.Normalize();

        _rigidbody.velocity = new Vector3(_moveInput.x * _moveSpeed, _rigidbody.velocity.y, _moveInput.y * _moveSpeed);
        
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

        // Calculate movement direction
        _moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);

        // Rotate the character to face the movement direction
        if (_moveDirection.magnitude > 0.1f) // Only rotate if there's significant movement
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator StartDodge()
    {
        _isDodging = true;
        float currentSpeed = _moveSpeed;
        _moveSpeed *= _dodgeSpeedMultiplier;

        //Wait for the duration of the dodge
        yield return new WaitForSeconds(_dodgeDuration);

        //Reset dodge state and speed
        _isDodging = false;
        _moveSpeed = currentSpeed;
    }

    public void PlayerDied()
    {
        _bulletSpawner.StopSpawning();
    }
}
