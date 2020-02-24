using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed;
    private AudioSource _buildupSound;
    private AudioSource _dropSound;
    private AudioSource _bassySound;
    private AudioSource _explosionSound;
    private AudioSource _energyBallSound;
    private AudioSource _laserSound;

    private UIManager _uiManager;
    private GameManager _gameManager;
    private Image _bossHealthBar;

    private Animator _camAnim;

    [SerializeField]
    private GameObject _leftEnemyPrefab;
    [SerializeField]
    private GameObject _rightEnemyPrefab;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _bigExplosionPrefab;
    [SerializeField]
    private GameObject _energyBallPrefab;
    [SerializeField]
    private GameObject _bigLaserPrefab;
    [SerializeField]
    private GameObject _laserCirclePrefab;


    private GameObject[] _enemies;

    [SerializeField]
    private int _lives;
    private bool _isDead = false;
    private bool _isInvincible;
    private bool _canGetShot;
    private bool _canShake = false;
    private bool _canSeek;
    void Start()
    {
        _bassySound = GameObject.Find("Bassy").GetComponent<AudioSource>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _bossHealthBar = GameObject.Find("BossHealthBar").GetComponent<Image>();
        _camAnim = GameObject.Find("Main Camera").GetComponent<Animator>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _laserSound = GameObject.Find("LaserSound").GetComponent<AudioSource>();
        _energyBallSound = GameObject.Find("EnergyBallSound").GetComponent<AudioSource>();
        _explosionSound = GameObject.Find("Explosion").GetComponent<AudioSource>();
        _buildupSound = GameObject.Find("Buildup").GetComponent<AudioSource>();
        _dropSound = GameObject.Find("Drop").GetComponent<AudioSource>();

        _isInvincible = true;
        _canGetShot = false;
        StartCoroutine(StartSequence());
        _uiManager.UpdateBossHealth(_lives);
        _bassySound.Stop();
    }


    void Update()
    {
        FindEnemies();
        if (transform.position.y >= 2.5f)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
    }

    void TakeDamage()
    {
        if (_isInvincible == false)
        {
            if (_lives > 1)
            {
                _lives--;
                _uiManager.UpdateBossHealth(_lives);
            }
            else
            {
                _lives--;
                _isDead = true;
                _uiManager.UpdateBossHealth(_lives);
                _gameManager.GameOver();
                _dropSound.Stop();
                StartCoroutine(DestroyHealthBar());
                StartCoroutine(DestroyProjectile());
                StartCoroutine(OnDeathSequence());
            }
        }
    }
    IEnumerator ShakeViolently()
    {
        yield return new WaitForSeconds(11);
        _camAnim.SetBool("IsViolentShaking", true);
    }
    IEnumerator OnDeathSequence()
    {
        for (int i = 1; i <= 5; i++)
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(_explosionPrefab, transform.position - new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0f), Quaternion.identity);
            _explosionSound.Play();
        }
        Instantiate(_bigExplosionPrefab, transform.position, Quaternion.identity);
        _uiManager.VictorySequence();
        Destroy(this.gameObject, 0.3f);
    }

    IEnumerator DestroyProjectile()
    {
        GameObject bigLaser = GameObject.FindWithTag("BigLaser");
        if (bigLaser != null)
            Destroy(bigLaser.gameObject);
        yield return new WaitForSeconds(0.5f);
        GameObject circleLaser = GameObject.FindWithTag("BossCircleLaser");
        if (circleLaser != null)
            Destroy(circleLaser.gameObject);
        yield return new WaitForSeconds(0.5f);
        GameObject[] energyBall = GameObject.FindGameObjectsWithTag("BossEnergyLaser");
        if (energyBall.Length > 0)
        {
            for (int i = 0; i < energyBall.Length; i++)
            {
                Destroy(energyBall[i].gameObject);
            }
        }
    }
    IEnumerator StartSequence()
    {
        StartCoroutine(ShowHealthBar());
        StartCoroutine(ShakeViolently());
        _camAnim.SetBool("IsShaking", true);
        _buildupSound.Play();
        yield return new WaitForSeconds(14f);
        _dropSound.Play();
        StartCoroutine(ShootSequence());
        StartCoroutine(LaserSequence());
        Instantiate(_leftEnemyPrefab, transform.position + new Vector3(-3.5f, -1.15f, 0f), _leftEnemyPrefab.transform.rotation);
        Instantiate(_rightEnemyPrefab, transform.position + new Vector3(3.5f, -1.15f, 0f), _rightEnemyPrefab.transform.rotation);
        Instantiate(_enemyPrefab, transform.position + new Vector3(0.65f, -3.47f, 0f), Quaternion.identity);
        Instantiate(_enemyPrefab, transform.position + new Vector3(-0.65f, -3.47f, 0f), Quaternion.identity);
        _canGetShot = true;
        _camAnim.SetBool("IsViolentShaking", false);
        _camAnim.SetBool("IsShaking", false);
    }

    IEnumerator ShowHealthBar()
    {
        yield return new WaitForSeconds(12f);
        float alpha = _bossHealthBar.color.a;
        for (float t = 0f; t < 1f; t += Time.deltaTime / 3)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 1, t));
            _bossHealthBar.color = newColor;
            yield return null;
        }
    }

    IEnumerator DestroyHealthBar()
    {
        float alpha = _bossHealthBar.color.a;
        for (float t = 0f; t < 1f; t += Time.deltaTime / 2)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0.0f, t));
            _bossHealthBar.color = newColor;
            yield return null;
        }
        Destroy(_bossHealthBar.gameObject, 0.2f);
    }
    IEnumerator ShootSequence()
    {
        yield return new WaitForSeconds(4f);
        while(_isDead == false)
        {
            Instantiate(_energyBallPrefab, transform.position + new Vector3(-3.5f, -1.15f, 0f), Quaternion.identity);
            Instantiate(_energyBallPrefab, transform.position + new Vector3(3.5f, -1.15f, 0f), Quaternion.identity);
            _energyBallSound.Play();
            yield return new WaitForSeconds(Random.Range(12, 18));
        }
    }

    IEnumerator LaserSequence()
    {
        yield return new WaitForSeconds(25f);
        while (_isDead == false)
        {
            Instantiate(_laserCirclePrefab, transform.position - new Vector3(0f, 3.85f, 0f), _laserCirclePrefab.transform.rotation);
            _laserSound.Play();
            _camAnim.SetBool("IsShaking", true);
            yield return new WaitForSeconds(2f);
            Instantiate(_bigLaserPrefab, transform.position - new Vector3(0f, 4.5f, 0f), Quaternion.identity);
            yield return new WaitForSeconds(20f);
        }

    }


    void FindEnemies()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (_enemies.Length > 1 && _canGetShot == true)
        {
            _canSeek = false;
            _isInvincible = true;
        }
        else
        {
            _canSeek = true;
            _isInvincible = false;
        }
    }

    public void Seeking()
    {
        if (_isDead == true)
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
        if (other.tag == "Laser" && _isInvincible == false && _canGetShot == true) 
        {
            Destroy(other.gameObject);
            TakeDamage();
        }

        if(other.tag == "Missile" && _isInvincible == false && _canGetShot == true)
        {
            Destroy(other.gameObject);
            TakeDamage();
        }

        if (other.tag == "Player")
        {
            other.GetComponent<Player>().TakeDamage();
        }
    }
}
