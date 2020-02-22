using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrontCheck : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Enemy _enemy;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PowerUp")
        {
            _enemy.FireLaserFront();
        }
    }
}
