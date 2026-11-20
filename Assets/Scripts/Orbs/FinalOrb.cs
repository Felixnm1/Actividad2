using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class FinalOrb : MonoBehaviour
{

    [SerializeField] float finalAnimationDuration = 5f;
    [SerializeField] float maxHeight = 100;
    [SerializeField] float maxSize = 100;
    [SerializeField] Color lightColor;

    /*
    //Borrar
    [SerializeField] float upSpeed = 1;
    [SerializeField] float scaleSpeed = 3f;
    [SerializeField] float fogSpeed = 0.5f;*/
    public static event Action OnPlayerWin;


    private static readonly int blendID = Shader.PropertyToID("_Blend");

    Camera playerCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FPS_Controller playerController = FindAnyObjectByType<FPS_Controller>();
        playerCamera = playerController.GetComponentInChildren<Camera>();
        playerController.enabled = false;
        StartCoroutine(finalAnimation());
    }

    // Update is called once per frame

    void Update()
    {
        /*
        transform.position += Vector3.up * upSpeed* Time.deltaTime;
        foreach(Transform child in transform)
        {
            child.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
        }
        RenderSettings.fogDensity = Mathf.Clamp01(RenderSettings.fogDensity - fogSpeed * Time.deltaTime);
        RenderSettings.sun.color = Color.Lerp(RenderSettings.sun.color, lightColor, 0.5f);
        playerCamera.transform.LookAt(transform.position);*/
    }

    IEnumerator finalAnimation()
    {
        Vector3 savePosition = transform.position;
        Vector3 endPosition = transform.position + new Vector3(0, maxHeight, 0);


        Vector3 saveScale = transform.localScale;
        Vector3 endScale = transform.localScale * maxSize;
        Vector3 lastFrameScale = saveScale;


        float saveFogDensity = RenderSettings.fogDensity;

        Color savedSunColor = RenderSettings.sun.color;



        float timer = 0;
        while (timer <= finalAnimationDuration)
        {
            yield return null;
            timer += Time.deltaTime;
            float progress = timer / finalAnimationDuration;
            transform.position = Vector3.Lerp(savePosition, endPosition, progress);

            transform.localScale = Vector3.Lerp(saveScale, endScale, progress);
            foreach (Transform child in transform)
            {
                child.localScale += saveScale-lastFrameScale;
            }
            lastFrameScale = transform.localScale;

            RenderSettings.fogDensity = Mathf.Lerp(saveFogDensity, 0, progress);
            RenderSettings.sun.color = Color.Lerp(savedSunColor, lightColor, progress);
            RenderSettings.skybox.SetFloat(blendID, progress);

            playerCamera.transform.LookAt(transform.position);

           
            
        }
        OnPlayerWin?.Invoke();



    }
}
