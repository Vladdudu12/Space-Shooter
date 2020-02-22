using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLaser : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveDown();
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        transform.Rotate(0f, 0f, 2f * (_speed / 2) * Time.deltaTime);
        if (transform.position.y < -7f)
        {
            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        if(transform.position.y > 7f)
        {
            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
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
