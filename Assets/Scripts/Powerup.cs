using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private int _id;
    private AudioSource _powerUpSound;

    void Start()
    {
        _powerUpSound = GameObject.Find("PowerUp").GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            switch (_id)
            {
                case 0:
                    {
                        player.TripleShotCollected();
                        break;
                    }
                case 1:
                    {
                        player.SpeedCollected();
                        break;
                    }
                case 2:
                    {
                        player.ShieldsCollected();
                        break;
                    }
                case 3:
                    {
                        player.AmmoCollected();
                        break;
                    }
                case 4:
                    {
                        player.RepairCollected();
                        break;
                    }
                case 5:
                    {
                        player.MissilesCollected();
                        break;
                    }
            }
            _powerUpSound.Play();
            Destroy(this.gameObject);
        }
    }
}
