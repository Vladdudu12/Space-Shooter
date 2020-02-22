using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _camAnim;
    void Start()
    {
        _camAnim = GameObject.Find("Main Camera").GetComponent<Animator>();
        StartCoroutine(EngageLaser());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EngageLaser()
    {
        _camAnim.speed = 0.5f;
        yield return new WaitForSeconds(1f);
        float xScale = transform.localScale.x;
        float xPosition = transform.position.x;
        for (float t = 0f; t < 1f; t += Time.deltaTime * 1.5f)
        {
            transform.localScale = new Vector3(Mathf.Lerp(xScale, 10f, t), transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(Mathf.Lerp(xPosition, 0.1f, t), transform.position.y, transform.position.z);
            yield return null;
        }
        _camAnim.SetBool("IsViolentShaking", true);
        StartCoroutine(OvershootCorection());
    }
    IEnumerator OvershootCorection()
    {
        float xScale = transform.localScale.x;
        for (float t = 0f; t < 1f; t += Time.deltaTime * 2f)
        {
            transform.localScale = new Vector3(Mathf.Lerp(xScale, 9.1f, t), transform.localScale.y, transform.localScale.z);
            yield return null;
        }
        StartCoroutine(DisengageLaser());
    }
    IEnumerator DisengageLaser()
    {
        yield return new WaitForSeconds(10f);
        float xScale = transform.localScale.x;
        float xPosition = transform.position.x;
        for (float t = 0f; t < 1f; t += Time.deltaTime * 1)
        {
            transform.localScale = new Vector3(Mathf.Lerp(xScale, 0.0f, t), transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(Mathf.Lerp(xPosition, 0.0f, t), transform.position.y, transform.position.z);
            yield return null;
        }
        _camAnim.SetBool("IsViolentShaking", false);
        _camAnim.SetBool("IsShaking", false);

        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().InstantDeath();
        }
    }
}
