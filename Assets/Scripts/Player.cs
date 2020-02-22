using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Movement
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _speedBoost;
    private Vector3 _direction;
    #endregion
    #region Powerups
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private GameObject _missilePrefab;
    [SerializeField]
    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;
    private bool _isDizzyActive = false;
    [SerializeField]
    private bool _isMissileActive = false;
    private int _shieldLives = 3;
    private SpriteRenderer _shieldColor;
    #endregion
    #region Firing Mechanism
    private float _canFire = -1f;
    [SerializeField]
    private float _fireRate;
    [SerializeField]
    private int _maxAmmo;
    [SerializeField]
    private int _ammoCount;
    #endregion
    #region UI
    private UIManager _uiManager;
    private int _score = 0;
    [SerializeField]
    private int _lives = 3;
    #endregion
    #region Managers
    private GameManager _gameManager;
    private WaveManager _waveManager;
    #endregion
    #region Audio and Animations
    private bool _leftEngineStart = false;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    private AudioSource _laserAudio;
    [SerializeField]
    private AudioSource _explosionAudio;
    [SerializeField]
    private GameObject _explosionPrefab;
    private AudioSource _reloadSound;
    [SerializeField]
    private AudioSource _missileSound;
    private Animator _cameraAnim;
    private Animator _playerAnim;
    #endregion
    #region Thruster boost
    private bool _startReloadingThruster = true;
    [SerializeField]
    private float _fuel;
    [SerializeField]
    private float _maxFuel;
    private bool _isBoosting = false;
    private Slider _thrusterSlider;
    #endregion

    [SerializeField]
    private bool _godMode = false;

    void Start()
    {
        #region Getting Components
        _playerAnim = GetComponent<Animator>();
        _cameraAnim = GameObject.Find("Main Camera").GetComponent<Animator>();
        _thrusterSlider = GameObject.Find("ThrusterBar").GetComponent<Slider>();
        _laserAudio = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
        _reloadSound = GameObject.Find("Reload").GetComponent<AudioSource>();
        #endregion
        #region Initial Assignments
        _thrusterSlider.value = _maxFuel;
        _fuel = _maxFuel;
        _ammoCount = _maxAmmo;
        _uiManager.UpdateAmmo(_ammoCount, _maxAmmo);
        transform.position = Vector3.zero;
        float randomEngine = Random.value;
        if (randomEngine < 0.5f)
        {
            _leftEngineStart = true;
        }
        else if (randomEngine > 0.5f)
        {
            _leftEngineStart = false;
        }
        #endregion
    }

    void Update()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void Movement()
    {
        float HorizontalAxis = Input.GetAxis("Horizontal");
        float VerticalAxis = Input.GetAxis("Vertical");
        if (HorizontalAxis == 0.0f)
        {
            //go idle
            _playerAnim.SetBool("GoingLeft", false);
            _playerAnim.SetBool("GoingRight", false);
        }
        else if (HorizontalAxis > 0f)
        {
            _playerAnim.SetBool("GoingRight", true);
            _playerAnim.SetBool("GoingLeft", false);
            //go right
        }
        else if (HorizontalAxis < 0f)
        {
            _playerAnim.SetBool("GoingLeft", true);
            _playerAnim.SetBool("GoingRight", false);
            //go left
        }

        if(_isDizzyActive == true)
        {
            _direction = new Vector3(-HorizontalAxis, -VerticalAxis, 0f);
        }
        else
        {
            _direction = new Vector3(HorizontalAxis, VerticalAxis, 0f);
        }

        transform.Translate(_direction * _speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            if (_fuel > 0)
            {
                _fuel -= Time.deltaTime;
                _thrusterSlider.value = _fuel;
                _isBoosting = true;
                transform.Translate(_direction * (_speed * _speedBoost) * Time.deltaTime);
            }
            else
            {
                _fuel = 0;
                _thrusterSlider.value = _fuel;
                _isBoosting = false;
                StartCoroutine(ThrusterCooldown());
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isBoosting = false;
            transform.Translate(_direction * _speed * Time.deltaTime);
            StartCoroutine(ThrusterCooldown());
        }

        if(_fuel < _maxFuel && _startReloadingThruster && !_isBoosting)
        {
            _fuel = Mathf.Clamp(_fuel + Time.deltaTime, 0f, _maxFuel);
            _thrusterSlider.value = _fuel;
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.75f, 2f), transform.position.z);
        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, transform.position.z);
        }
    }

    void FireLaser()
    {
        if (_ammoCount > 0)
        {
            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                _laserAudio.Play();
            }
            else if (_isMissileActive == true)
            {
                Instantiate(_missilePrefab, new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z), new Quaternion(0f, 0f, 90f, 90f));
                _missileSound.Play();
            }
            else if (_isTripleShotActive == false && _isMissileActive == false) 
            {
                Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z), Quaternion.identity);
                _laserAudio.Play();
            }
            _ammoCount--;
            _canFire = Time.time + _fireRate;
            _uiManager.UpdateAmmo(_ammoCount, _maxAmmo);
        }
        else if (_ammoCount == 0)
        {
            _reloadSound.Play();
            _uiManager.NoAmmoSequence();
        }
        
    }

    public void TakeDamage()
    {
        if(_godMode == false)
        {
            if (_isShieldActive == true)
            {
                _shieldColor = GameObject.Find("Shields").GetComponent<SpriteRenderer>();
                if (_shieldColor != null)
                {
                    _shieldLives--;
                    StartCoroutine(CameraShake());
                    switch (_shieldLives)
                    {
                        case 0:
                            {
                                _shieldLives = 3;
                                _shieldColor.color = Color.white;
                                _isShieldActive = false;
                                _shieldVisual.active = false;
                                break;
                            }
                        case 1:
                            {
                                _shieldColor.color = Color.red;
                                break;
                            }
                        case 2:
                            {
                                _shieldColor.color = Color.green;
                                break;
                            }
                    }
                }
            }
            else
            {
                _lives--;
                StartCoroutine(CameraShake());
                switch (_lives)
                {
                    case 1:
                        {
                            _leftEngine.SetActive(true);
                            _rightEngine.SetActive(true);
                            break;
                        }
                    case 2:
                        {
                            if (_leftEngineStart == false)
                            {
                                _rightEngine.SetActive(true);
                                _leftEngine.SetActive(false);
                            }
                            else if (_leftEngine == true)
                            {
                                _leftEngine.SetActive(true);
                                _rightEngine.SetActive(false);
                            }
                            break;
                        }
                    case 3:
                        {
                            _leftEngine.SetActive(false);
                            _rightEngine.SetActive(false);
                            break;
                        }
                }
            }
        
            _uiManager.UpdateLives(_lives);
        }

        if (_lives < 1)
        {
            _gameManager.GameOver();
            _waveManager.OnPlayerDeath();
            _uiManager.GameOverSequence();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _explosionAudio.Play();
            Destroy(this.gameObject);
        }
    }

    #region Powerup Functions
    public void TripleShotCollected()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

    public void SpeedCollected()
    {
        _isSpeedActive = true;
        _speed *= _speedBoost;
        StartCoroutine(SpeedPowerDownRoutine());
    }
    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isSpeedActive = false;
        _speed /= _speedBoost;
    }

    public void ShieldsCollected()
    {
        _isShieldActive = true;
        _shieldVisual.active = true;
    }

    public void AmmoCollected()
    {
        _ammoCount = _maxAmmo;
        _uiManager.UpdateAmmo(_ammoCount, _maxAmmo);
        _reloadSound.Stop();
    }

    public void RepairCollected()
    {
        if(_lives < 3)
        {
            _lives++;
            switch (_lives)
            {
                case 1:
                    {
                        _leftEngine.SetActive(true);
                        _rightEngine.SetActive(true);
                        break;
                    }
                case 2:
                    {
                        if (_leftEngineStart == false)
                        {
                            _rightEngine.SetActive(true);
                            _leftEngine.SetActive(false);
                        }
                        else if (_leftEngine == true)
                        {
                            _leftEngine.SetActive(true);
                            _rightEngine.SetActive(false);
                        }
                        break;
                    }
                case 3:
                    {
                        _leftEngine.SetActive(false);
                        _rightEngine.SetActive(false);
                        break;
                    }
            }
            _uiManager.UpdateLives(_lives);
        }
    }

    public void MissilesCollected()
    {
        _isMissileActive = true;
        StartCoroutine(MissilesPowerDownRoutine());
    }
    IEnumerator MissilesPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isMissileActive = false;
    }

    public void DizzinessCollected()
    {
        _isDizzyActive = true;
        StartCoroutine(DizzinessPowerDownRoutine());
    }

    IEnumerator DizzinessPowerDownRoutine()
    {
        yield return new WaitForSeconds(8f);
        _isDizzyActive = false;
    }
    #endregion

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void InstantDeath()
    {
        _lives = 0;
        TakeDamage();
    }
    IEnumerator ThrusterCooldown()
    {
        _startReloadingThruster = false;
        yield return new WaitForSeconds(3f);
        _startReloadingThruster = true;
    }

    IEnumerator CameraShake()
    {
        _cameraAnim.SetBool("IsHit", true);
        yield return new WaitForSeconds(0.3f);
        _cameraAnim.SetBool("IsHit", false);
    }
}
