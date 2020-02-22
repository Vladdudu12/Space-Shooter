using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private SpriteRenderer[] _backgrounds;
    private int _currentWave = -1;
    [SerializeField]
    private ParticleSystem _particleSystem;
    private Animator _cameraAnim;
    private WaveManager _waveManager;
    void Start()
    {
        _waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
        _cameraAnim = GameObject.Find("Main Camera").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }


    public void ChangeBackground()
    {
        _currentWave++;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _particleSystem.Play();
            StartCoroutine(CameraShake());
            _waveManager.StartNextWave();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator CameraShake()
    {
        _cameraAnim.SetBool("EnterBlackHole", true);
        yield return new WaitForSeconds(0.3f);
        _backgrounds[_currentWave].enabled = true;
        _cameraAnim.SetBool("EnterBlackHole", false);
    }
}
