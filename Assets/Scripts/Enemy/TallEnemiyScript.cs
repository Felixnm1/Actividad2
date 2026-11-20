using System;
using UnityEngine;

public class TallEnemiyScript : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] AudioClip spawn;
    [SerializeField] AudioClip Attack;
    [SerializeField] AudioClip Dead;
    [SerializeField] AudioClip Run;


    AudioSource audioS;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        audioS.PlayOneShot(spawn);
        audioS.clip = Run;
        audioS.loop = true;
        audioS.Play();

        BasicEnemyMovement.OnPlayerAttacked += attackSound;
        BasicEnemyMovement.OnExorcited += exorciteSound;
    }

    private void exorciteSound(GameObject gO)
    {
        if (gO == null || gO != gameObject) { return; }
        audioS.Stop();
        audioS.PlayOneShot(Dead);
    }

    private void attackSound(Transform tr)
    {
        if (tr == null || tr.gameObject != gameObject) { return; }
        audioS.Stop();
        audioS.PlayOneShot(Attack);
    }

    private void OnDestroy()
    {
        BasicEnemyMovement.OnPlayerAttacked -= attackSound;
        BasicEnemyMovement.OnExorcited -= exorciteSound;
    }



}
