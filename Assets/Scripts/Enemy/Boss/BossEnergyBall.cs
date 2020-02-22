using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnergyBall : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject _player;
    [SerializeField]
    private float _speed;



    private bool _canShoot = false;
    void Start()
    {
        StartCoroutine(WaitAndShoot());
    }

    // Update is called once per frame
    void Update()
    {
        if(_canShoot == true)
        {
            Transform _targetPosition = FindPlayer();
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition.position, _speed * Time.deltaTime);
        }
    }

    private Transform FindPlayer()
    {
        _player = GameObject.Find("Player");
        Transform _playerPos;
        _playerPos = _player.transform;
        if (_player == null)
            return null;

        return _playerPos;
    }


    IEnumerator WaitAndShoot()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(FollowRoutine());
        _canShoot = true;
    }
    IEnumerator FollowRoutine()
    {
        yield return new WaitForSeconds(6f);
        StartCoroutine(FadeOut());
        Destroy(this.gameObject,2f);
    }

    IEnumerator FadeOut()
    {
        float alpha = transform.GetComponent<SpriteRenderer>().material.color.a;
        for (float t = 0f; t < 1f; t += Time.deltaTime / 2)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0, t));
            transform.GetComponent<SpriteRenderer>().material.color = newColor;
            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player.GetComponent<Player>().TakeDamage();
            Destroy(this.gameObject);
        }
    }
}
