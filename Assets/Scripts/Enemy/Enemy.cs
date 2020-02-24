using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Audio and Animations
    private Animator _anim;
    private AudioSource _explosionSound;
    private AudioSource _laserSound;
    #endregion

    #region GameObjects
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _backLaserPrefab;
    private GameObject _enemyLaser;
    [SerializeField]
    private GameObject _shieldVisual;
    private GameObject _playerObject;
    #endregion

    #region Bools
    private bool _isDead = false;
    [SerializeField]
    private bool _goingRight = true;
    [SerializeField]
    private bool _goingDown = false;
    private bool _isShieldActive;
    [SerializeField]
    private bool _canRam = false;
    [SerializeField]
    private bool _canBackShot = false;
    private bool _canFrontShot = false;
    private bool _canSeek = true;
    #endregion

    #region ScriptManagement
    private UIManager _uiManager;
    private Player _player;
    private WaveManager _waveManager;
    #endregion

    #region floats and ints
    [SerializeField]
    private float _speed;
    private float _startingSpeed;
    [SerializeField]
    private float _ramSpeed;

    private float _fireRate;
    private float _canFire = -1f;
    private int _shieldPicker;
    private int _backShotPicker;
    private int _frontShotPicker;
    #endregion



    private Quaternion _startRotation;

    void Start()
    {
        _waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
        _startingSpeed = _speed;
        _startRotation = transform.rotation;
        _playerObject = GameObject.Find("Player");
        _explosionSound = GameObject.Find("Explosion").GetComponent<AudioSource>();
        _laserSound = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _ramSpeed = _speed * 0.75f;
        if (transform.rotation.z > 0f)
        {
            _goingRight = true;
            _speed *= 1.5f;
        }
        else if (transform.rotation.z < 0f)
        {
            _goingRight = false;
            _speed *= 1.5f;
        }
        else if (transform.rotation.z == 0)
        {
            _goingDown = true;
        }

        _shieldPicker = Random.Range(0, 5);
        if (_shieldPicker == 3)
        {
            _isShieldActive = true;
            _shieldVisual.active = true;
        }

        _backShotPicker = Random.Range(0, 5);
        if(_backShotPicker == 2)
        {
            _canBackShot = true;   
        }

        _frontShotPicker = Random.Range(0, 5);
        if (_frontShotPicker == 4)
        {
            _canFrontShot = true;
        }
    }

    void Update()
    {
        Movement();
        FireLaser();
        if(_playerObject != null)
        {
            Ram();
        }
    }

    void FireLaser()
    {
        if (_isDead == false)
        {
            if (Time.time > _canFire)
            {
                _fireRate = Random.Range(3f, 7f);
                _canFire = Time.time + _fireRate;
                if (_goingRight == false && _goingDown == false)
                {
                    _enemyLaser = Instantiate(_laserPrefab, transform.position - new Vector3(0.6f, 1f, 0f), Quaternion.identity);
                }
                else if (_goingRight == true && _goingDown == false)
                {
                    _enemyLaser = Instantiate(_laserPrefab, transform.position - new Vector3(-0.6f, 0.9f, 0f), Quaternion.identity);
                }
                else if (_goingDown == true && _goingRight == false)
                {
                    _enemyLaser = Instantiate(_laserPrefab, transform.position - new Vector3(0f, 1.06f, 0f), Quaternion.identity);
                }
                _enemyLaser.transform.rotation = transform.rotation;
                Laser[] lasers = _enemyLaser.GetComponentsInChildren<Laser>();
                _laserSound.Play();
                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemy();
                }
            }
        }
    }

    public void FireLaserFront()
    {
        if (_isDead == false && _canFrontShot == true)
        {
            if (_goingRight == false && _goingDown == false)
            {
                _enemyLaser = Instantiate(_laserPrefab, transform.position - new Vector3(0.6f, 1f, 0f), Quaternion.identity);
            }
            else if (_goingRight == true && _goingDown == false)
            {
                _enemyLaser = Instantiate(_laserPrefab, transform.position - new Vector3(-0.6f, 0.9f, 0f), Quaternion.identity);
            }
            else if (_goingDown == true && _goingRight == false)
            {
                _enemyLaser = Instantiate(_laserPrefab, transform.position - new Vector3(0f, 1.06f, 0f), Quaternion.identity);
            }
            _enemyLaser.transform.rotation = transform.rotation;
            Laser[] lasers = _enemyLaser.GetComponentsInChildren<Laser>();
            _laserSound.Play();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemy();
            }
        }

    }

    public void FireLaserBack()
    {
        if (_isDead == false && _canBackShot == true)
        {
            if (_goingRight == false && _goingDown == false)
            {
                _enemyLaser = Instantiate(_backLaserPrefab, transform.position + new Vector3(0.6f, 1f, 0f), Quaternion.identity);
            }
            else if (_goingRight == true && _goingDown == false)
            {
                _enemyLaser = Instantiate(_backLaserPrefab, transform.position + new Vector3(-0.6f, 1f, 0f), Quaternion.identity);
            }
            else if (_goingDown == true && _goingRight == false)
            {
                _enemyLaser = Instantiate(_backLaserPrefab, transform.position - new Vector3(0f, -1.15f, 0f), Quaternion.identity);
            }
            _enemyLaser.transform.rotation = transform.rotation;
            Laser[] lasers = _enemyLaser.GetComponentsInChildren<Laser>();
            _laserSound.Play();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemy();
                lasers[i].AssignCircleLaser();
            }
        }
    }

    void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);


        if (transform.position.y < -6f)
        {
            _canRam = true;
            float randomX = Random.Range(-9.4f, 9.5f);
            transform.position = new Vector3(randomX, 6f, 0f);
        }
    }

    void Ram()
    {
        float distance = Vector3.Distance(_playerObject.transform.position, transform.position);
            if (distance <= 3f && _isShieldActive == false && _canRam == true && _isDead == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, _playerObject.transform.position, _ramSpeed * Time.deltaTime);
                transform.LookAt(_playerObject.transform.position);
                transform.rotation = new Quaternion(0f, 0f, transform.rotation.y, 90f);
            }
            else if (distance <= 0f && _isShieldActive == false)
            {
                transform.rotation = _startRotation;
                _speed = _startingSpeed;
            }
    }
    void OnDeath()
    {
        _anim.SetTrigger("OnEnemyDeath");
        _speed = 0;
        _isDead = true;
        _explosionSound.Play();
        Destroy(GetComponent<Collider2D>());
        _waveManager.EnemyDefeated();
        Destroy(this.gameObject, 2.5f);
    }


    public void Seeking()
    {
        if(_isShieldActive == false && _isDead == false)
        {
            _canSeek = false;
        }
    }

    public bool CheckSeek()
    {
        return _canSeek;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.TakeDamage();
            }
            if (_isShieldActive == true)
            {
                _isShieldActive = false;
                _shieldVisual.active = false;
            }
            else
            {
                OnDeath();
            }
        }
        if (other.tag == "Barrier")
        {
            _canRam = false;
        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            
            if (_isShieldActive == true)
            {
                _isShieldActive = false;
                _shieldVisual.active = false;
            }
            else
            {
                if (_player != null)
                {
                    int _randomScore = Random.Range(5, 15);
                    _player.AddScore(_randomScore);
                }
                OnDeath();
            }
        }
        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            
            if (_isShieldActive == true)
            {
                _isShieldActive = false;    
                _shieldVisual.active = false;
            }
            else
            {
                if (_player != null)
                {
                    int _randomScore = Random.Range(15, 30);
                    _player.AddScore(_randomScore);
                }
                OnDeath();
            }
        }
    }
}
