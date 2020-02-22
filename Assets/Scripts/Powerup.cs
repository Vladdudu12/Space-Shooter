using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _magnetSpeed;
    [SerializeField]
    private int _id;
    private AudioSource _powerUpSound;
    private Transform _target;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _powerUpSound = GameObject.Find("PowerUp").GetComponent<AudioSource>();
    }

    void Update()
    {
        _target = FindPlayer();
        if (Input.GetKey(KeyCode.C))
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            _rb.MovePosition(transform.position + direction * _magnetSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (transform.position.y < -6f)
        {
            Destroy(this.gameObject);
        }
    }

    private Transform FindPlayer()
    {
        GameObject player = GameObject.Find("Player");
        Transform closest;

        if (player == null)
            return null;

        closest = player.transform;
        return closest;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            switch (_id)
            {
                case 0:
                    {
                        player.TripleShotCollected();
                        break;
                    }
                case 1:
                    {
                        player.SpeedCollected();
                        break;
                    }
                case 2:
                    {
                        player.ShieldsCollected();
                        break;
                    }
                case 3:
                    {
                        player.AmmoCollected();
                        break;
                    }
                case 4:
                    {
                        player.RepairCollected();
                        break;
                    }
                case 5:
                    {
                        player.MissilesCollected();
                        break;
                    }
                case 6:
                    {
                        player.DizzinessCollected();
                        break;
                    }
            }
            _powerUpSound.Play();
            Destroy(this.gameObject);
        }
        if (other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
