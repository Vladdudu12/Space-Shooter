using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private GameObject _explosionPrefab;
    private AudioSource _explosionSound;
    private WaveManager _waveManager;

    void Start()
    {
        _waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
        _explosionSound = GameObject.Find("Explosion").GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _waveManager.StartNextWave();
            _explosionSound.Play();
            Destroy(this.GetComponent<Collider2D>());
            Destroy(this.gameObject, 0.25f);
        }
    }
}
