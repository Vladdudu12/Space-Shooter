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
        _target = FindClosestEnemy();

    }

    // Update is called once per frame
    void Update()
    {
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
        int i = 0;
        Transform closest;

        if (enemies.Length == 0)
            return null;
        while (i < enemies.Length)
        {
            if (enemies[i].name == "Enemy(Clone)" || enemies[i].name == "Right_Enemy(Clone)" || enemies[i].name == "Left_Enemy(Clone)")
            {
                if (enemies[i].GetComponent<Enemy>().CheckSeek() == true)
                {
                    enemies[i].GetComponent<Enemy>().Seeking();
                    closest = enemies[i].transform;
                    return closest;
                }

            }
            else if (enemies[i].name == "SmartEnemy(Clone)")
            {
                if (enemies[i].GetComponent<SmartEnemy>().CheckSeek() == true)
                {
                    enemies[i].GetComponent<SmartEnemy>().Seeking();
                    closest = enemies[i].transform;
                    return closest;
                }
            }
            else if (enemies[i].name == "RotatingEnemy")
            {
                if (enemies[i].GetComponent<RotatingEnemy>().CheckSeek() == true)
                {
                    enemies[i].GetComponent<RotatingEnemy>().Seeking();
                    closest = enemies[i].transform;
                    return closest;
                }
            }
            else if (enemies[i].name == "OrganicShip(Clone)")
            {
                if (enemies[i].GetComponent<BossAI>().CheckSeek() == true)
                {
                    enemies[i].GetComponent<BossAI>().Seeking();
                    closest = enemies[i].transform;
                    return closest;
                }
            }
            i++;
        }
        return null;
    }

}
