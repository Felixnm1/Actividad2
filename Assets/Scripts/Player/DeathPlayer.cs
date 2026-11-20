using System;
using System.Collections;
using UnityEngine;

public class DeathPlayer : MonoBehaviour
{
    [SerializeField] Transform spawn;
    [SerializeField] float cameraTrunTime=0.2f;
    [SerializeField] float reespawnAnimation = 4f;
    [SerializeField] int lives = 3;


    public static event Action OnPlayerDefeat;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BasicEnemyMovement.OnPlayerAttacked += startAnimation;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    Coroutine saveCoroutine;
    private void startAnimation(Transform tr)
    {
        if(saveCoroutine == null)
        {
            saveCoroutine = StartCoroutine(AnimationCoroutine(tr));
        }
        
    }
    IEnumerator AnimationCoroutine(Transform tr)
    {
        float timer = 0f;
        GetComponent<FPS_Controller>().enabled = false;
        Transform playerCamera = GetComponentInChildren<Camera>().transform;
        Quaternion saveRotation = playerCamera.rotation;

        Quaternion endRotation = Quaternion.LookRotation(tr.position-playerCamera.position);

        playerCamera.rotation = saveRotation;

        while (timer < cameraTrunTime)
        {
            yield return null;
            timer += Time.deltaTime;
            playerCamera.rotation = Quaternion.Slerp(saveRotation, endRotation, timer/cameraTrunTime);
        }
        timer = 0f;
        float saveFogValue = RenderSettings.fogDensity;
        float maxFogValue = 5;
        lives--;
        
        if (lives == 0)
        {
            OnPlayerDefeat?.Invoke();
        }
        while (timer < reespawnAnimation/2)
        {
            yield return null;
            timer += Time.deltaTime;
            RenderSettings.fogDensity = Mathf.Lerp(saveFogValue, maxFogValue, timer / (reespawnAnimation/2));
        }
        playerCamera.rotation = saveRotation;
        timer = 0;
        transform.position = spawn.position;
        while (timer < reespawnAnimation / 2)
        {
            yield return null;
            timer += Time.deltaTime;
            RenderSettings.fogDensity = Mathf.Lerp(maxFogValue, saveFogValue,  timer / (reespawnAnimation/2));
        }
        RenderSettings.fogDensity = saveFogValue;


        GetComponent<FPS_Controller>().enabled = true;
        saveCoroutine = null;
    }
}
