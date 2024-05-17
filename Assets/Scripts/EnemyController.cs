using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private BulletSpawner _bulletSpawner;
    // Start is called before the first frame update
    void Start()
    {
        _bulletSpawner = GetComponentInChildren<BulletSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        /*print(_bulletSpawner.isFiring);

        if (_bulletSpawner.isFiring)
        {
            _animator.SetBool("isFiring", true);
        }
        else
        {
            _animator.SetBool("isFiring", false);
        }*/
    }


    public IEnumerator PlayFireAnim(float cooldown)
    {
        _animator.SetBool("isFiring", true);
        yield return new WaitForSeconds(cooldown);
        _animator.SetBool("isFiring", false);
    }
}
