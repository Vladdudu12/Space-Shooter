using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private UIManager _uiManager;
    private Player _player;
    private Animator _anim;
    private AudioSource _explosionSound;
    [SerializeField]
    private GameObject _laserPrefab;
    private AudioSource _laserSound;
    private float _fireRate;
    private float _canFire = -1f;
    private bool _isDead = false;

    void Start()
    {
        _explosionSound = GameObject.Find("Explosion").GetComponent<AudioSource>();
        _laserSound = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        Movement();
        FireLaser();
    }

    void FireLaser()
    {
        if (_isDead == false)
        {
            if (Time.time > _canFire)
            {
                _fireRate = Random.Range(3f, 7f);
                _canFire = Time.time + _fireRate;
                GameObject enemyLaser = Instantiate(_laserPrefab, transform.position - new Vector3(0f, 1.2f, 0f), Quaternion.identity);
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
                _laserSound.Play();
                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemy();
                }
            }
        }
    }

    void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-9.4f, 9.5f);
            transform.position = new Vector3(randomX, 6f, 0f);
        }
    }

    void OnDeath()
    {
        _anim.SetTrigger("OnEnemyDeath");
        _speed = 0;
        _isDead = true;
        _explosionSound.Play();
        Destroy(GetComponent<Collider2D>(), 0.4f);
        Destroy(this.gameObject, 2.5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.TakeDamage();
            }
            OnDeath();

        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                int _randomScore = Random.Range(5, 15);
                _player.AddScore(_randomScore);
            }
            OnDeath();
        }
    }
}
