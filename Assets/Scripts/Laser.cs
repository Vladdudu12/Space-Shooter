using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private bool _isEnemyLaser = false;
    private bool _isCircleLaser = false;

    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > 10f)
        {
            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    void MoveDown()
    {
        if (_isCircleLaser == true)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            if (transform.position.y > 10f)
            {
                if (this.transform.parent != null)
                {
                    Destroy(this.transform.parent.gameObject);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            if (transform.position.y < -11f)
            {
                if (this.transform.parent != null)
                {
                    Destroy(this.transform.parent.gameObject);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public void AssignEnemy()
    {
        _isEnemyLaser = true;
    }

    public void AssignCircleLaser()
    {
        _isCircleLaser = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage();
            }
            Destroy(this.gameObject);
        }
    }

}
