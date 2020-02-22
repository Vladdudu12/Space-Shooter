using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCheck : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private SmartEnemy _smartEnemy;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _smartEnemy.ShouldDodge();
        }
    }
}
