using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Movement
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _speedBoost;
    #endregion
    #region Powerups
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;
    #endregion
    #region Firing Mechanism
    private float _canFire = -1f;
    [SerializeField]
    private float _fireRate;
    #endregion
    #region UI
    private UIManager _uiManager;
    private int _score = 0;
    [SerializeField]
    private int _lives = 3;
    #endregion
    #region Managers
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
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
    #endregion

    void Start()
    {
        _laserAudio = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
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
        Vector3 direction = new Vector3(HorizontalAxis, VerticalAxis, 0f);
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.75f, 0f), transform.position.z);
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
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        }
        else if (_isTripleShotActive == false)
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z), Quaternion.identity);
        }
        _laserAudio.Play();
        _canFire = Time.time + _fireRate;
    }

    public void TakeDamage()
    {
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisual.active = false;
        }
        else
        {
            _lives--;
            if (_leftEngineStart == false)
            {
                _rightEngine.SetActive(true);
                _leftEngineStart = true;
            }
            else if (_leftEngine == true)
            {
                _leftEngine.SetActive(true);
                _leftEngineStart = false;
            }

            _uiManager.UpdateLives(_lives);
        }


        if (_lives < 1)
        {
            _gameManager.GameOver();
            _spawnManager.OnPlayerDeath();
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
    #endregion

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

}
