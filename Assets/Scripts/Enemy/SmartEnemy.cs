using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed;
    private bool _canSeek = true;
    [SerializeField]
    private GameObject _energyBallPrefab;
    private bool _isDead = false;
    private bool _canDodge = false;
    [SerializeField]
    private bool _switch;
    private Player _player;
    [SerializeField]
    private GameObject _explosionPrefab;
    private AudioSource _explosionSound;
    private AudioSource _energyChargeSound;

    private WaveManager _waveManager;
    void Start()
    {
        _waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
        _energyChargeSound = GetComponent<AudioSource>();
        _explosionSound = GameObject.Find("Explosion").GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        float sidePicker = Random.value;
        if (sidePicker < 0.5)
        {
            _switch = false;
        }
        else if (sidePicker >= 0.5)
        {
            _switch = true;
        }

        StartCoroutine(FireEnergyBallRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (_canDodge == true)
        {
            Dodge();
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

    void Dodge()
    {
        if (_switch == false)
        {
            StartCoroutine(DodgeDown());
            transform.Translate(Vector3.left * (_speed * 3) * Time.deltaTime);
        }
        else if (_switch == true)
        {
            StartCoroutine(DodgeDown());
            transform.Translate(Vector3.right * (_speed * 3) * Time.deltaTime);
        }
    }

    public void ShouldDodge()
    {
        _canDodge = true;
    }

    public void Seeking()
    {
        if(_isDead == false)
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
        _speed = 0;
        _isDead = true;
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _explosionSound.Play();
        Destroy(GetComponent<Collider2D>());
        _waveManager.EnemyDefeated();
        Destroy(this.gameObject, 1f);
    }

    IEnumerator DodgeDown() //this dodge behaviour is intended
    {
        yield return new WaitForSeconds(0.25f);
        _canDodge = false;
        if (_switch == true)
        {
            _switch = false;
        }
        else if (_switch == false)
        {
            _switch = true;
        }
    }
    IEnumerator FireEnergyBallRoutine()
    {
        yield return new WaitForSeconds(1f);
        while(_isDead == false)
        {
            GameObject energyBall = Instantiate(_energyBallPrefab, transform.position - new Vector3(0f, 1.3f, 0f), Quaternion.identity);
            energyBall.transform.parent = this.transform;
            _energyChargeSound.Play();
            yield return new WaitForSeconds(7f);
        }
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
