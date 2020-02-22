using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float frequency = 10f;
    [SerializeField]
    private float magnitude = 0.5f;

    private Vector3 _position;
    private Vector3 _axis;
    private bool _isDead = false;
    void Start()
    {
        _position = transform.position;
        _axis = transform.right;
    }

    void FixedUpdate()
    {
        if(_isDead == false)
        {
            if (transform.position.y > -6f)
            {
                _position += Vector3.down * Time.deltaTime * _speed;
                transform.position = _position + _axis * Mathf.Sin(Time.time * frequency) * magnitude;
            }
            else if (transform.position.y < -6f)
            {
                float randomX = Random.Range(-9.4f, 9.5f);
                transform.position = new Vector3(randomX, 5f, 0f);
                _position = transform.position;
            }
        }
        if(transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }
        Debug.Log(transform.position);
    }

    public void OnDeath()
    {
        _isDead = true;
    }
}
