using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Bacile : MonoBehaviour
{
    [SerializeField] AudioClip[] ac;
    AudioSource aS;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aS = GetComponent<AudioSource>();
        StartCoroutine(c());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator c()
    {
        foreach (AudioClip clip in ac)
        {
            yield return new WaitForSeconds(67f);
            aS.clip = clip;
            aS.Play();
        }
    }
}
