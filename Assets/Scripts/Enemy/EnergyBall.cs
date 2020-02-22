using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    // Start is called before the first frame update
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
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            if (transform.position.y < -7f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator WaitAndShoot()
    {
        yield return new WaitForSeconds(2f);
        _canShoot = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage();
            }
            Destroy(this.gameObject);
        }
    }
}
