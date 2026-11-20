using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class FlashLightScript : MonoBehaviour
{
    
    [SerializeField] private InputActionReference turnFlashLight;
    [SerializeField] private GameObject FlashLight;
    [SerializeField] float distanceDetection;
    [SerializeField] float angleDetection;
    [SerializeField] LayerMask layerObstacles;
    [SerializeField] private AudioClip[] turnSounds = new AudioClip[2];
    List<Transform> enemiesTransforms = new List<Transform>();
    RaycastHit hit;

    private void Awake()
    {
        turnFlashLight.action.started += TrunFlashlight;
        BasicEnemyMovement.OnEnemySpawn += addEnemy;
    }

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesTransforms.Count > 0 && FlashLight.activeSelf)
        {
            for (int i = 0; i < enemiesTransforms.Count; i++)
            {
                if (enemiesTransforms[i] == null)
                {
                    enemiesTransforms.RemoveAt(i);
                    i--;
                }
                else
                {
                    Vector3 enPos = enemiesTransforms[i].position + Vector3.up;

                    float distance = Vector3.Distance(transform.position, enPos);

                    Vector3 direction = (enPos - transform.position).normalized;
                    float angle = Vector3.Angle(transform.forward, direction);

                    Debug.DrawLine(transform.position, enPos, Color.green);
                    Debug.DrawRay(transform.position, transform.forward * 5f, Color.yellow);
                    if (distance <= distanceDetection && angle <= angleDetection &&
                        !Physics.Linecast(transform.position, enPos, out hit, layerObstacles))
                    {
                        
                        enemiesTransforms[i].gameObject.GetComponent<BasicEnemyMovement>().Exorcite();
                    }
                }
            }
        }
    }

    private void addEnemy(GameObject enemy)
    {
        if (enemy.CompareTag("TallEnemy"))
        {
            enemiesTransforms.Add(enemy.transform);
        }
    }


    private void TrunFlashlight(InputAction.CallbackContext context)
    {
        FlashLight.SetActive(!FlashLight.activeSelf);

        AudioSource audioSource = gameObject.GetComponent<AudioSource>();

        audioSource.clip = turnSounds[(FlashLight.activeSelf) ? 0: 1];

        audioSource.Play();

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceDetection);
    }
}
