using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCircleLaser : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _rotationSpeed;

    void Start()
    {
        StartCoroutine(DestructionSequence());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.left * _rotationSpeed * Time.time);
    }

    IEnumerator DestructionSequence()
    {
        yield return new WaitForSeconds(3.5f);
        Destroy(this.gameObject);
    }
}
