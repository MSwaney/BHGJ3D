using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public Vector3 _velocity;
    [SerializeField] public float _speed;
    [SerializeField] public float _rotation;
    [SerializeField] public float _maxDistance = 100f;

    private Vector3 _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Store the starting position of the bullet
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the bullet according to its velocity and speed
        transform.Translate(_velocity * _speed * Time.deltaTime);

        // Check if the bullet has traveled the maximum distance
        if (Vector3.Distance(_startPosition, transform.position) >= _maxDistance)
        {
            // Destroy the bullet if it exceeds the maximum distance
            Destroy(this.gameObject);
        }
    }
}
