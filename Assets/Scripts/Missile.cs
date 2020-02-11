using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private int _speed;
    private Rigidbody2D _rb;
    [SerializeField]
    private GameObject _explosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        _target = FindClosestEnemy();
        if (_target != null)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            _rb.MovePosition(transform.position + direction * _speed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2((_target.position.y - transform.position.y), (_target.position.x - transform.position.x)) * Mathf.Rad2Deg);
        }
        else
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        if (transform.position.y < -5.90f)
        {
            Destroy(this.gameObject);
        }
    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;
        Transform closest;

        if (enemies.Length == 0)
            return null;

        closest = enemies[0].transform;
        for (int i = 1; i < enemies.Length; i++)
        {
            float distance = (enemies[i].transform.position - transform.position).sqrMagnitude;

            if(distance <minDistance)
            {
                closest = enemies[i].transform;
                minDistance = distance;
            }
        }
        return closest;
    }

}
