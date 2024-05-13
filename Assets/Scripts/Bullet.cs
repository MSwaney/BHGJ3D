using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public Vector3 _velocity;
    [SerializeField] public float _speed;
    [SerializeField] public float _rotation;
    // Start is called before the first frame update
    void Start()
    {
        //transform.rotation = Quaternion.Euler(0f, 0f, _rotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_velocity * _speed * Time.deltaTime);
    }
}
