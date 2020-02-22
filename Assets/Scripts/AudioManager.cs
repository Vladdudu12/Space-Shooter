using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _intro;
    private AudioSource _bassy;
    // Start is called before the first frame update
    void Start()
    {
        _intro = GameObject.Find("Intro").GetComponent<AudioSource>();
        _intro.Play();
        _bassy = GameObject.Find("Bassy").GetComponent<AudioSource>();
        StartCoroutine(WaitAndPlay());
    }

    IEnumerator WaitAndPlay()
    {
        yield return new WaitForSeconds(14f);
        _bassy.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
