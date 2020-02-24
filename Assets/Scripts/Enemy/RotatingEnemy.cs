using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    private bool _canSeek = true;
    [SerializeField]
    private float _rotationSpeed;
    private bool _goingRight = true;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate;
    private float _canFire = -2f;
    private bool _isDead = false;
    private GameObject _enemyLaser;
    private AudioSource _laserSound;
    private AudioSource _explosionSound;
    [SerializeField]
    private GameObject _explosionPrefab;
    private Player _player;
    private RotatingMovement _rotMovement;
    private Laser[] _lasers;
    private WaveManager _waveManager;

    void Start()
    {
        _waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
        _rotMovement = GetComponentInParent<RotatingMovement>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _laserSound = GetComponent<AudioSource>();
        _explosionSound = GameObject.Find("Explosion").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
        FireLaser();
    }

    void FireLaser()
    {
        if (_isDead == false)
        {
            if (Time.time > _canFire)
            {
                _fireRate = 8;
                _canFire = Time.time + _fireRate;

                _enemyLaser = Instantiate(_laserPrefab, transform.position - new Vector3(0.6f, 1f, 0f), Quaternion.identity);
                if(_enemyLaser != null)
                _lasers = _enemyLaser.GetComponentsInChildren<Laser>();
                _laserSound.Play();
                for (int i = 0; i < _lasers.Length; i++)
                {
                    _lasers[i].AssignEnemy();
                }
            }
            if(_enemyLaser != null)
            _enemyLaser.transform.Rotate(0f, 0f, 1f * (_rotationSpeed / 2) * Time.deltaTime);
        }
    }

    public void Seeking()
    {
        if (_isDead == false)
        {
            _canSeek = false;
        }
    }

    public bool CheckSeek()
    {
        return _canSeek;
    }

    void OnDeath()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _rotationSpeed = 0;
        _rotMovement.OnDeath();
        _isDead = true;
        _explosionSound.Play();
        Destroy(this.GetComponent<Collider2D>());
        _waveManager.EnemyDefeated();
        Destroy(this.gameObject, 1.2f);
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
        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                int _randomScore = Random.Range(15, 30);
                _player.AddScore(_randomScore);
            }
            OnDeath();
        }
    }
}
