using UnityEngine;
using UnityEngine.Audio;
[RequireComponent(typeof(AudioSource))]
public class Walk : MonoBehaviour
{
    private enum State
    {
        Ground,
        Water,
        Air
    }
    [Header("Audio")]

    [SerializeField] AudioClip[] stepSounds;
    [SerializeField] AudioClip[] launchLandGround;
    [SerializeField] AudioClip[] waterSounds;
    [SerializeField] AudioClip[] launchLandWater;
    private AudioSource audioSource;
    [Header("Detection")]
    [SerializeField] float distanceStep=0.5f;
    [SerializeField] float distanceWater=1f;
    [SerializeField] private float radioGroundCheck = 0.2f;
    [SerializeField] LayerMask Ground;
    [SerializeField] LayerMask Water;
    [SerializeField] Transform grounndDetector;
    

    private State state;
    private State prevState;
    private Vector3 prevPos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        prevPos = transform.position;
        detectState();
    }

    // Update is called once per frame
    void Update()
    {
        detectState();
        switch (state)
        {
            case State.Ground:
                if(prevState != State.Air && checkDistance())
                {
                    audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
                }
                else if(prevState == State.Air){
                    audioSource.PlayOneShot(launchLandGround[1]);
                }
                else if(prevState == State.Water)
                {
                    audioSource.PlayOneShot(launchLandWater[0]);
                }
                break;
            case State.Water:
                if (prevState != State.Air && checkDistance())
                {
                    audioSource.PlayOneShot(waterSounds[Random.Range(0, waterSounds.Length)]);
                }
                else if (prevState == State.Air)
                {
                    audioSource.PlayOneShot(launchLandWater[1]);
                }
                break;
            case State.Air:
                if(prevState == State.Ground)
                {
                    audioSource.PlayOneShot(launchLandGround[0]);
                }
                
                break;

        }
    }


    private void detectState()
    {
        prevState = state;
        if (Physics.CheckSphere(grounndDetector.position, radioGroundCheck, Water))
        {
            state = State.Water;
        }else if (Physics.CheckSphere(grounndDetector.position, radioGroundCheck, Ground))
        {
            state=State.Ground;
        }
        else
        {
            state = State.Air;
        }
    }

    private bool checkDistance()
    {
        if(state == State.Air) { 
            prevPos = transform.position;
            return true; 
        }
        float distance = (state ==State.Ground)? distanceStep: distanceWater;
        bool result = distance <= Vector3.Distance(prevPos, transform.position);
        if (result)
        {
            prevPos = transform.position;
        }
        return result;
    }

}
